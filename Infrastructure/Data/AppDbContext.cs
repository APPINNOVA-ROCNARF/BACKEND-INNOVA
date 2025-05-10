using Domain.Entities;
using Domain.Entities.Usuarios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioWeb> UsuariosWeb { get; set; }
        public DbSet<UsuarioApp> UsuariosApp { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Modulo> Modulos { get; set; }
        public DbSet<RolModulos> RolModulos { get; set; }
        public DbSet<RolSeccion> RolSecciones { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<Accion> Acciones { get; set; }
        public DbSet<RolPermisos> RolPermisos { get; set; }
        public DbSet<LogsAuditor> LogsAuditor { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Usuarios
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Estado)
                .HasDefaultValue(true);
            
            modelBuilder.Entity<Usuario>()
                .Property(u => u.CreadoEn)
                .HasDefaultValueSql("NOW()")
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<Usuario>()
                .Property(u => u.ModificadoEn)
                .HasDefaultValueSql("NOW()")
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);


            // Relación UsuarioWeb ↔ Usuario
            modelBuilder.Entity<UsuarioWeb>()
                .HasKey(wu => wu.Id);

            modelBuilder.Entity<UsuarioWeb>()
                .HasOne(wu => wu.Usuario)
                .WithOne(u => u.UsuarioWeb)
                .HasForeignKey<UsuarioWeb>(uw => uw.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación UsuarioMovil ↔ Usuario
            modelBuilder.Entity<UsuarioApp>()
                .HasKey(um => um.Id);

            modelBuilder.Entity<UsuarioApp>()
                .HasOne(um => um.Usuario)
                .WithOne(u => u.UsuarioApp)
                .HasForeignKey<UsuarioApp>(um => um.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Username único y obligatorio
            modelBuilder.Entity<UsuarioApp>()
                .HasIndex(um => um.NombreUsuario)
                .IsUnique();

            modelBuilder.Entity<UsuarioApp>()
                .Property(um => um.NombreUsuario)
                .HasMaxLength(5)
                .IsRequired();


            modelBuilder.Entity<UsuarioApp>()
                .Property(um => um.SeccionId)
                .IsRequired();

            modelBuilder.Entity<UsuarioAppSeccion>(entity =>
            {
                entity.HasKey(us => new { us.UsuarioAppId, us.SeccionId });

                entity.HasOne(us => us.UsuarioApp)
                      .WithMany(ua => ua.UsuarioSecciones)
                      .HasForeignKey(us => us.UsuarioAppId)
                      .OnDelete(DeleteBehavior.Cascade);

            });

            // Roles
            modelBuilder.Entity<Rol>()
                .Property(r => r.Estado)
                .HasDefaultValue(true);

            modelBuilder.Entity<RolSeccion>(entity =>
            {
                entity.HasKey(rs => new { rs.RolId, rs.SeccionId });

                entity.HasOne(rs => rs.Rol)
                      .WithMany(r => r.RolSecciones)
                      .HasForeignKey(rs => rs.RolId)
                      .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Rol>()
                .Property(r => r.Tipo)
                .HasConversion<int>(); 


            //Modulos
            modelBuilder.Entity<Modulo>()
                .Property(m => m.Estado)
                .HasDefaultValue(true);

            // Relación Rol ↔ Modulo (Muchos a Muchos con tabla intermedia `RolModulos`)
            modelBuilder.Entity<RolModulos>()
                .HasKey(rm => new { rm.RolId, rm.ModuloId });

            modelBuilder.Entity<RolModulos>()
                .HasOne(rm => rm.Rol)
                .WithMany(r => r.RolModulos)
                .HasForeignKey(rm => rm.RolId);

            modelBuilder.Entity<RolModulos>()
                .HasOne(rm => rm.Modulo)
                .WithMany(m => m.RolModulos)
                .HasForeignKey(rm => rm.ModuloId);

            // Relación Permiso ↔ Modulo (Un Módulo tiene varios permisos)
            modelBuilder.Entity<Permiso>()
                .HasOne(p => p.Modulo)
                .WithMany(m => m.Permisos)
                .HasForeignKey(p => p.ModuloId);

            // Relación Rol ↔ Permiso ↔ Accion (Muchos a Muchos con `RolPermisos`)
            modelBuilder.Entity<RolPermisos>().HasKey(rp => new { rp.RolId, rp.PermisoId, rp.AccionId });

            modelBuilder.Entity<RolPermisos>()
                .HasOne(rp => rp.Rol)
                .WithMany(r => r.RolPermisos)
                .HasForeignKey(rp => rp.RolId);

            modelBuilder.Entity<RolPermisos>()
                .HasOne(rp => rp.Permiso)
                .WithMany(p => p.RolPermisos)
                .HasForeignKey(rp => rp.PermisoId);

            modelBuilder.Entity<RolPermisos>()
                .HasOne(rp => rp.Accion)
                .WithMany()
                .HasForeignKey(rp => rp.AccionId);
        }
    }
}
