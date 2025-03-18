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
        public DbSet<Rol> Roles { get; set; }
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
