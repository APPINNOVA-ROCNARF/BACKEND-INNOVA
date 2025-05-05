using Application.Audit;
using Application.EventHandlers.Auditoria;
using Application.Exceptions;
using Application.Interfaces.IArchivo;
using Application.Interfaces.IAuth;
using Application.Interfaces.IRol;
using Application.Interfaces.ISistema;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.IUsuario;
using Application.Interfaces.IVehiculo;
using Application.Interfaces.IViatico;
using Application.Options;
using Application.Services;
using Application.Validators.Vehiculo;
using Application.Validators.Viatico;
using Domain.Common;
using Domain.Events;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Audit;
using Infrastructure.Data;
using Infrastructure.Events;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
dataSourceBuilder.UseJsonNet(); 
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(dataSource).LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddDbContext<SistemaDbContext>(options =>
    options.UseNpgsql(dataSource).LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddDbContext<ViaticosDbContext>(options =>
    options.UseNpgsql(dataSource).LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddDbContext<AuditoriaDbContext>(options =>
    options.UseNpgsql(dataSource).LogTo(Console.WriteLine, LogLevel.Information));

// Configurar el Rate Limiter
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        var clientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(clientIp, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 200, // Máximo 100 peticiones
            Window = TimeSpan.FromMinutes(5), // cada 15 minutos
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0 // No guardar en cola si ya superó el límite
        });
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// Validators
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ViaticoCrearValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ActualizarEstadoViaticoRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegistrarVehiculoDTOValidator>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Backend API",
        Version = "v1",
        Description = "API para la gestión del sistema APP INNOVA"
    });
});

builder.Services.Configure<ArchivosOptions>(
    builder.Configuration.GetSection("Archivos"));


var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Registrar servicios
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISistemaService, SistemaService>();
builder.Services.AddScoped<IArchivoService, ArchivoService>();
builder.Services.AddScoped<IViaticoService, ViaticoService>();
builder.Services.AddScoped<ISolicitudViaticoService, SolicitudViaticoService>();
builder.Services.AddScoped<IVehiculoService, VehiculoService>();
builder.Services.AddScoped<IAuditoriaService, AuditoriaService>();


// Registrar repositorios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ISistemaRepository, SistemaRepository>();
builder.Services.AddScoped<IArchivoRepository, ArchivoRepository>();
builder.Services.AddScoped<IViaticoRepository, ViaticoRepository>();
builder.Services.AddScoped<IProveedorViaticoRepository, ProveedorViaticoRepository>();
builder.Services.AddScoped<IVehiculoRepository, VehiculoRepository>();
builder.Services.AddScoped<ISolicitudViaticoRepository, SolicitudViaticoRepository>();
builder.Services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();

// Registrar eventos
builder.Services.AddScoped<IDomainEventDispatcher, InMemoryDomainEventDispatcher>();
builder.Services.AddScoped<IDomainEventHandler<EstadoViaticoCambiadoEvent>, EstadoViaticoCambiadoAuditoriaHandler>();



//Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Registrar cache
builder.Services.AddMemoryCache();

builder.Services.AddScoped<JwtService>();

var app = builder.Build();

// Habilitar Swagger solo en modo desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.UseCors(x => x.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
    app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRateLimiter();

app.UseExceptionHandler(config =>
{
    config.Run(async context =>
    {
        var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;

        if (exception is BusinessException businessException)
        {
            context.Response.StatusCode = businessException.StatusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                success = false,
                message = businessException.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    });
});


app.Run();