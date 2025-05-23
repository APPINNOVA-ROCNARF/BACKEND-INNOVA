﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.Sistema
{
    [DbContext(typeof(SistemaDbContext))]
    [Migration("20250409220454_secciones")]
    partial class secciones
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Sistema.Ciclo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Estado")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("FechaFin")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("FechaInicio")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Ciclos");
                });

            modelBuilder.Entity("Domain.Entities.Sistema.Fuerza", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Fuerzas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Codigo = "F1",
                            Nombre = "F1"
                        },
                        new
                        {
                            Id = 2,
                            Codigo = "F2",
                            Nombre = "F2"
                        },
                        new
                        {
                            Id = 3,
                            Codigo = "F3",
                            Nombre = "F3"
                        },
                        new
                        {
                            Id = 4,
                            Codigo = "F4",
                            Nombre = "F4"
                        });
                });

            modelBuilder.Entity("Domain.Entities.Sistema.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Regiones");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Nombre = "Sierra"
                        },
                        new
                        {
                            Id = 2,
                            Nombre = "Costa"
                        },
                        new
                        {
                            Id = 3,
                            Nombre = "Austro"
                        });
                });

            modelBuilder.Entity("Domain.Entities.Sistema.Seccion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<int>("FuerzaId")
                        .HasColumnType("integer");

                    b.Property<int>("RegionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FuerzaId");

                    b.HasIndex("RegionId");

                    b.ToTable("Secciones");
                });

            modelBuilder.Entity("Domain.Entities.Sistema.Seccion", b =>
                {
                    b.HasOne("Domain.Entities.Sistema.Fuerza", "Fuerza")
                        .WithMany("Secciones")
                        .HasForeignKey("FuerzaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Sistema.Region", "Region")
                        .WithMany("Secciones")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Fuerza");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("Domain.Entities.Sistema.Fuerza", b =>
                {
                    b.Navigation("Secciones");
                });

            modelBuilder.Entity("Domain.Entities.Sistema.Region", b =>
                {
                    b.Navigation("Secciones");
                });
#pragma warning restore 612, 618
        }
    }
}
