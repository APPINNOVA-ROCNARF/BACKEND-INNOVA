using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.ViaticoDTO;
using Domain.Common;
using Domain.Entities.Viaticos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ViaticosDbContext : DbContext
    {
        public ViaticosDbContext(DbContextOptions<ViaticosDbContext> options) : base(options) { }

        public DbSet<Viatico> Viaticos { get; set; }
        public DbSet<SolicitudViatico> SolicitudesViatico { get; set; }
        public DbSet<CategoriaViatico> CategoriasViatico { get; set; }
        public DbSet<FacturaViatico> FacturasViatico { get; set; }
        public DbSet<ProveedorViatico> ProveedoresViatico { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<EstadisticaSolicitudViaticoDTO> EstadisticaSolicitudViatico { get; set; }
        public DbSet<VehiculoPrincipal> VehiculoPrincipal { get; set; }
        public DbSet<SolicitudVehiculoPrincipal> SolicitudVehiculoPrincipal { get; set; }
        public DbSet<CupoMensual> CupoMensual { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<EstadisticaSolicitudViaticoDTO>().HasNoKey().ToView(null);

            // SOLICITUD VIATICO
            modelBuilder.Entity<SolicitudViatico>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.FechaRegistro)
                      .HasColumnType("timestamp without time zone")
                      .IsRequired();

                entity.Property(s => s.FechaModificado)
                      .HasColumnType("timestamp without time zone")
                      .IsRequired();

                entity.Property(s => s.Estado)
                      .IsRequired();
            });

            // VIATICO
            modelBuilder.Entity<Viatico>(entity =>
            {
                entity.HasKey(v => v.Id);

                entity.Property(v => v.FechaRegistro)
                    .HasColumnType("timestamp without time zone")
                    .IsRequired();

                entity.Property(v => v.FechaModificado)
                    .HasColumnType("timestamp without time zone")
                    .IsRequired();

                entity.Property(v => v.EstadoViatico)
                      .IsRequired();

                entity.Property(v => v.Comentario);

                entity.Property(v => v.CamposRechazados)
                      .HasColumnType("jsonb");

                entity.HasOne(v => v.SolicitudViatico)
                      .WithMany(s => s.Viaticos)
                      .HasForeignKey(v => v.SolicitudViaticoId)
                      .OnDelete(DeleteBehavior.Restrict);

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
                      .HasForeignKey(v => v.VehiculoId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // CATEGORIA
            modelBuilder.Entity<CategoriaViatico>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Nombre).HasMaxLength(50).IsRequired();
                entity.Property(c => c.Estado).IsRequired();
                entity.HasData(
                    new CategoriaViatico { Id = 1, Nombre = "Movilización", Estado = true },
                    new CategoriaViatico { Id = 2, Nombre = "Alimentación", Estado = true },
                    new CategoriaViatico { Id = 3, Nombre = "Hospedaje", Estado = true }
                 );
            });

            // PROVEEDOR
            modelBuilder.Entity<ProveedorViatico>(entity =>
            {
                entity.HasKey(p => p.Ruc);
                entity.Property(p => p.Ruc).HasMaxLength(20).IsRequired();
                entity.Property(p => p.RazonSocial).HasMaxLength(255).IsRequired();
            });

            // FACTURA
            modelBuilder.Entity<FacturaViatico>(entity =>
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
                entity.HasKey(v => v.Id);

                entity.Property(v => v.Placa)
                      .HasMaxLength(10)
                      .IsRequired();

                entity.Property(v => v.Fabricante)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(v => v.Modelo)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(v => v.Color)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(v => v.FechaRegistro)
                      .HasColumnType("timestamp without time zone")
                      .IsRequired();

                entity.Property(v => v.UsuarioAppId)
                      .IsRequired();

                entity.HasIndex(v => v.Placa )
                      .IsUnique(); 
            });

            modelBuilder.Entity<VehiculoPrincipal>(entity =>
            {
                entity.HasKey(vp => vp.UsuarioAppId);

                entity.Property(vp => vp.UsuarioAppId)
                      .ValueGeneratedNever();

                entity.Property(vp => vp.FechaModificado)
                      .HasColumnType("timestamp without time zone")
                      .IsRequired();

                entity.HasOne(vp => vp.Vehiculo)
                      .WithMany()
                      .HasForeignKey(vp => vp.VehiculoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SolicitudVehiculoPrincipal>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.UsuarioAppId)
                      .IsRequired();

                entity.Property(s => s.VehiculoIdSolicitado)
                      .IsRequired();

                entity.Property(s => s.Motivo)
                      .HasMaxLength(500)
                      .IsRequired();

                entity.Property(s => s.FechaRegistro)
                      .HasColumnType("timestamp without time zone")
                      .IsRequired();

                entity.Property(s => s.Estado)
                      .HasConversion<int>()
                      .IsRequired();

                entity.Property(s => s.AprobadoPorUsuarioId)
                      .IsRequired(false); 

                entity.Property(s => s.FechaAprobacion)
                      .HasColumnType("timestamp without time zone")
                      .IsRequired(false);
            });

            modelBuilder.Entity<CupoMensual>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Categoria)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(c => c.MontoAsignado)
                      .HasColumnType("numeric(10,2)")
                      .IsRequired();

                entity.Property(c => c.FechaRegistro)
                      .HasColumnType("timestamp without time zone");

                entity.HasIndex(c => new { c.UsuarioId, c.CicloId, c.Categoria })
                      .IsUnique();
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
