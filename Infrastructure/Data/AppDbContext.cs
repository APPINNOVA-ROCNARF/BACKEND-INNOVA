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
        public DbSet<UsuarioWeb> UsuariosWeb   { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Permisos> Permisos { get; set; }
        public DbSet<Accion> Acciones { get; set; }
        public DbSet<RolPermisos> RolPermisos { get; set; }
        public DbSet<LogsAuditor> LogsAuditor { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioWeb>()
                .HasKey(wu => wu.Id);

            modelBuilder.Entity<UsuarioWeb>()
           .HasOne(wu => wu.Usuario)
           .WithOne(u => u.UsuarioWeb)
           .HasForeignKey<UsuarioWeb>(uw => uw.UsuarioId)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UsuarioWeb>()
                .HasOne(wu => wu.Rol)
                .WithMany()
                .HasForeignKey(wu => wu.RolId);

            modelBuilder.Entity<Rol>()
                .HasMany(r => r.UsuariosWeb) 
                .WithOne(uw => uw.Rol)         
                .HasForeignKey(uw => uw.RolId);

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
