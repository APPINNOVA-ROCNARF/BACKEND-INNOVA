using Domain.Entities;
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
        public DbSet<Plataforma> Plataformas { get; set; }

        public DbSet<Modulo> Modulos { get; set; }
        public DbSet<RolModulos> RolModulos { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<Accion> Acciones { get; set; }
        public DbSet<RolPermisos> RolPermisos { get; set; }
        public DbSet<LogsAuditor> LogsAuditor { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación UsuarioWeb ↔ Usuario
            modelBuilder.Entity<UsuarioWeb>()
                .HasKey(wu => wu.Id);

            modelBuilder.Entity<UsuarioWeb>()
                .HasOne(wu => wu.Usuario)
                .WithOne(u => u.UsuarioWeb)
                .HasForeignKey<UsuarioWeb>(uw => uw.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación UsuarioWeb ↔ Rol
            modelBuilder.Entity<UsuarioWeb>()
                .HasOne(uw => uw.Rol)
                .WithMany(r => r.UsuariosWeb)
                .HasForeignKey(uw => uw.RolId);

            // Relación UsuarioMovil ↔ Usuario
            modelBuilder.Entity<UsuarioApp>()
                .HasKey(um => um.Id);

            modelBuilder.Entity<UsuarioApp>()
                .HasOne(um => um.Usuario)
                .WithOne(u => u.UsuarioApp)
                .HasForeignKey<UsuarioApp>(um => um.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación UsuarioMovil ↔ Rol
            modelBuilder.Entity<UsuarioApp>()
                .HasOne(um => um.Rol)
                .WithMany(r => r.UsuariosApp)
                .HasForeignKey(um => um.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            // Username único y obligatorio
            modelBuilder.Entity<UsuarioApp>()
                .HasIndex(um => um.NombreUsuario)
                .IsUnique();

            modelBuilder.Entity<UsuarioApp>()
                .Property(um => um.NombreUsuario)
                .HasMaxLength(5)
                .IsRequired();

            // Relación Rol ↔ Plataforma
            modelBuilder.Entity<Plataforma>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Plataforma>()
                .Property(p => p.Nombre)
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder.Entity<Rol>()
                .HasOne(r => r.Plataforma)
                .WithMany(p => p.Roles)
                .HasForeignKey(r => r.PlataformaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Plataforma>().HasData(
                new Plataforma { Id = 1, Nombre = "WEB" },
                new Plataforma { Id = 2, Nombre = "APP" },
                new Plataforma { Id = 3, Nombre = "MULTI" }
);


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
