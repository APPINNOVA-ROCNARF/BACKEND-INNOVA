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
        }
    }
}
