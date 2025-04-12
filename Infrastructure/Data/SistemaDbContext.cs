using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Sistema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SistemaDbContext : DbContext
    {
        public SistemaDbContext(DbContextOptions<SistemaDbContext> options) : base(options) { }

        public DbSet<Ciclo> Ciclos { get; set; }
        public DbSet<Region> Regiones { get; set; }
        public DbSet<Fuerza> Fuerzas { get; set; }
        public DbSet<Seccion> Secciones { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ciclo>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Nombre)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(c => c.FechaInicio).IsRequired()
                      .HasColumnType("timestamp without time zone");
                entity.Property(c => c.FechaFin).HasColumnType("timestamp without time zone");
                entity.Property(c => c.Estado).IsRequired();
            });

            // REGION
            modelBuilder.Entity<Region>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Nombre)
                      .HasMaxLength(100)
                      .IsRequired();
            });

            // FUERZA
            modelBuilder.Entity<Fuerza>(entity =>
            {
                entity.HasKey(f => f.Id);

                entity.Property(f => f.Codigo)
                      .HasMaxLength(10)
                      .IsRequired();

                entity.Property(f => f.Nombre)
                      .HasMaxLength(100)
                      .IsRequired();
            });

            // SECCION
            modelBuilder.Entity<Seccion>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Codigo)
                      .HasMaxLength(10)
                      .IsRequired();

                entity.HasOne(s => s.Region)
                      .WithMany(r => r.Secciones)
                      .HasForeignKey(s => s.RegionId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Fuerza)
                      .WithMany(f => f.Secciones)
                      .HasForeignKey(s => s.FuerzaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // REGIONES
            modelBuilder.Entity<Region>().HasData(
                new Region { Id = 1, Nombre = "Sierra" },
                new Region { Id = 2, Nombre = "Costa" },
                new Region { Id = 3, Nombre = "Austro" }
            );

            // FUERZAS
            modelBuilder.Entity<Fuerza>().HasData(
                new Fuerza { Id = 1, Codigo = "F1", Nombre = "F1" },
                new Fuerza { Id = 2, Codigo = "F2", Nombre = "F2" },
                new Fuerza { Id = 3, Codigo = "F3", Nombre = "F3" },
                new Fuerza { Id = 4, Codigo = "F4", Nombre = "F4" }
            );
        }
    }
}
