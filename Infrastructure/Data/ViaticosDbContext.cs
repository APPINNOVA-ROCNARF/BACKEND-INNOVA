using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Viaticos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ViaticosDbContext : DbContext
    {
        public ViaticosDbContext(DbContextOptions<ViaticosDbContext> options) : base(options) { }

        public DbSet<Viatico> Viaticos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // VIATICO
            modelBuilder.Entity<Viatico>(entity =>
            {
                entity.HasKey(v => v.Id);

                entity.Property(v => v.FechaRegistro)
                .HasColumnType("timestamp without time zone")
                .IsRequired();
                entity.Property(v => v.IdUsuario).IsRequired();
                entity.Property(v => v.CicloId).IsRequired();

                entity.Property(v => v.EstadoCiclo)
                      .HasConversion<string>()
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(v => v.EstadoViatico)
                      .HasConversion<string>()
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(v => v.Comentario);

                entity.Property(v => v.CamposRechazados)
                      .HasColumnType("jsonb");

                entity.HasOne(v => v.Factura)
                      .WithMany(f => f.Viaticos)
                      .HasForeignKey(v => v.FacturaId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(v => v.Categoria)
                      .WithMany(c => c.Viaticos)
                      .HasForeignKey(v => v.CategoriaId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(v => v.Vehiculo)
                      .WithMany(vh => vh.Viaticos)
                      .HasForeignKey(v => v.PlacaVehiculo)
                      .HasPrincipalKey(vh => vh.Placa)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // CATEGORIA
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Nombre).HasMaxLength(50).IsRequired();
                entity.Property(c => c.Estado).IsRequired();
                entity.HasData(
                    new Categoria { Id = 1, Nombre = "movilizacion", Estado = true },
                    new Categoria { Id = 2, Nombre = "alimentacion", Estado = true },
                    new Categoria { Id = 3, Nombre = "hospedaje", Estado = true }
                 );
            });

            // PROVEEDOR
            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.HasKey(p => p.Ruc);
                entity.Property(p => p.Ruc).HasMaxLength(20).IsRequired();
                entity.Property(p => p.RazonSocial).HasMaxLength(255).IsRequired();
            });

            // FACTURA
            modelBuilder.Entity<Factura>(entity =>
            {
                entity.HasKey(f => f.Id);

                entity.Property(f => f.NumeroFactura).HasMaxLength(50).IsRequired();
                entity.Property(f => f.FechaFactura)
                .HasColumnType("timestamp without time zone")
                .IsRequired();
                entity.Property(f => f.Subtotal).HasColumnType("numeric(10,2)");
                entity.Property(f => f.SubtotalIva).HasColumnType("numeric(10,2)");
                entity.Property(f => f.Total).HasColumnType("numeric(10,2)");
                entity.Property(f => f.RutaImagen).HasMaxLength(500).IsRequired();

                entity.HasOne(f => f.Proveedor)
                      .WithMany(p => p.Facturas)
                      .HasForeignKey(f => f.RucProveedor)
                      .HasPrincipalKey(p => p.Ruc)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // VEHICULO
            modelBuilder.Entity<Vehiculo>(entity =>
            {
                entity.HasKey(v => v.Placa);
                entity.Property(v => v.Placa).HasMaxLength(20).IsRequired();
            });
        }
    }
}
