using Domain.Entities.Auditoria;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AuditoriaDbContext : DbContext
    {
        public AuditoriaDbContext(DbContextOptions<AuditoriaDbContext> options)
            : base(options)
        {
        }

        public DbSet<AuditoriaRegistro> Auditorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditoriaRegistro>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.TipoEvento)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Fecha)
                    .IsRequired();

                entity.Property(e => e.Datos)
                    .IsRequired();

                entity.Property(e => e.EntidadAfectada)
                    .HasMaxLength(100);

                entity.Property(e => e.Hash)
                    .HasMaxLength(256);

                entity.Property(e => e.UsuarioNombre)
                   .HasMaxLength(150);

                entity.Property(e => e.IpCliente)
                    .HasMaxLength(45); 

                entity.Property(e => e.MetodoHttp)
                    .HasMaxLength(10);

                entity.Property(e => e.RutaAccedida)
                    .HasMaxLength(300);
            });
        }
    }
}
