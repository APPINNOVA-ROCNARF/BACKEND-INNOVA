using Application.Interfaces.IArchivo;
using Application.Interfaces.IAuth;
using Application.Interfaces.IRol;
using Application.Interfaces.ISistema;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.IUsuario;
using Application.Interfaces.IViatico;
using Application.Options;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddDbContext<SistemaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
               .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddDbContext<ViaticosDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
               .LogTo(Console.WriteLine, LogLevel.Information));


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

    app.Run();