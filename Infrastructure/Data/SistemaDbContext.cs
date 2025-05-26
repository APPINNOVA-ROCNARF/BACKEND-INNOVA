using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
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
        public DbSet<GuiaProducto> GuiasProducto { get; set; }
        public DbSet<ArchivoGuiaProducto> ArchivoGuiaProducto { get; set; }
        public DbSet<ParrillaPromocional> ParrillasPromocional { get; set; }


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

            // GUIA PRODUCTO

            modelBuilder.Entity<GuiaProducto>(entity =>
            {
                entity.HasKey(g => g.Id);

                entity.Property(g => g.Marca)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(g => g.Nombre)
                      .HasMaxLength(150)
                      .IsRequired();

                entity.Property(g => g.UrlVideo)
                      .HasMaxLength(500)
                      .IsRequired();

                entity.Property(g => g.Activo)
                      .IsRequired();

                entity.Property(g => g.FechaRegistro)
                      .HasColumnType("timestamp without time zone")
                      .IsRequired();

                entity.HasOne(g => g.Fuerza)
                      .WithMany(f => f.GuiasProducto)
                      .HasForeignKey(g => g.FuerzaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ARCHIVO GUIA PRODUCTO
            modelBuilder.Entity<ArchivoGuiaProducto>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.NombreOriginal)
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(a => a.RutaRelativa)
                      .HasMaxLength(500)
                      .IsRequired();

                entity.Property(a => a.Extension)
                      .HasMaxLength(10)
                      .IsRequired();

                entity.Property(a => a.Activo)
                      .IsRequired();

                entity.Property(a => a.FechaRegistro)
                      .HasColumnType("timestamp without time zone")
                      .IsRequired();

                entity.HasOne(a => a.GuiaProducto)
                      .WithMany(g => g.Archivos)
                      .HasForeignKey(a => a.GuiaProductoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ParrillaPromocional>(entity =>
            {

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.Descripcion)
                      .IsRequired()
                      .HasMaxLength(1000);

                entity.Property(e => e.FechaModificado)
                      .IsRequired()
                      .HasColumnType("timestamp without time zone");

                entity.Property(e => e.NombreArchivo)
                      .HasMaxLength(255);

                entity.Property(e => e.ExtensionArchivo)
                      .HasMaxLength(10);

                entity.Property(e => e.UrlArchivo)
                      .HasMaxLength(500);
            });
        }

        public override int SaveChanges()
        {
            AuditarFechas();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AuditarFechas();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AuditarFechas()
        {
            var now = DateTime.Now;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is ICreado creado && entry.State == EntityState.Added)
                {
                    creado.FechaRegistro = now;
                }

                if (entry.Entity is IModificado modificado &&
                    (entry.State == EntityState.Added || entry.State == EntityState.Modified))
                {
                    modificado.FechaModificado = now;
                }
            }
        }
    }


}
