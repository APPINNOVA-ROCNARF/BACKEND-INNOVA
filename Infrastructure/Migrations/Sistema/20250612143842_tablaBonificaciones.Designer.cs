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
    [Migration("20250612143842_tablaBonificaciones")]
    partial class tablaBonificaciones
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Sistema.ArchivoGuiaProducto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Activo")
                        .HasColumnType("boolean");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("GuiaProductoId")
                        .HasColumnType("integer");

                    b.Property<string>("NombreOriginal")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("RutaRelativa")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.HasKey("Id");

                    b.HasIndex("GuiaProductoId");

                    b.ToTable("ArchivoGuiaProducto");
                });

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
                        },
                        new
                        {
                            Id = 5,
                            Codigo = "BF",
                            Nombre = "BioFemme"
                        },
                        new
                        {
                            Id = 6,
                            Codigo = "DC",
                            Nombre = "Drocaras"
                        });
                });

            modelBuilder.Entity("Domain.Entities.Sistema.GuiaProducto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Activo")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("FuerzaId")
                        .HasColumnType("integer");

                    b.Property<string>("Marca")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("UrlVideo")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.HasKey("Id");

                    b.HasIndex("FuerzaId");

                    b.ToTable("GuiasProducto");
                });

            modelBuilder.Entity("Domain.Entities.Sistema.ParrillaPromocional", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<string>("ExtensionArchivo")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<DateTime>("FechaModificado")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("NombreArchivo")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("UrlArchivo")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.HasKey("Id");

                    b.ToTable("ParrillasPromocional");
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

            modelBuilder.Entity("Domain.Entities.Sistema.TablaBonificaciones", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<string>("ExtensionArchivo")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<DateTime>("FechaModificado")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("NombreArchivo")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("UrlArchivo")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.HasKey("Id");

                    b.ToTable("TablaBonificaciones");
                });

            modelBuilder.Entity("Domain.Entities.Sistema.ArchivoGuiaProducto", b =>
                {
                    b.HasOne("Domain.Entities.Sistema.GuiaProducto", "GuiaProducto")
                        .WithMany("Archivos")
                        .HasForeignKey("GuiaProductoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GuiaProducto");
                });

            modelBuilder.Entity("Domain.Entities.Sistema.GuiaProducto", b =>
                {
                    b.HasOne("Domain.Entities.Sistema.Fuerza", "Fuerza")
                        .WithMany("GuiasProducto")
                        .HasForeignKey("FuerzaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Fuerza");
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
                    b.Navigation("GuiasProducto");

                    b.Navigation("Secciones");
                });

            modelBuilder.Entity("Domain.Entities.Sistema.GuiaProducto", b =>
                {
                    b.Navigation("Archivos");
                });

            modelBuilder.Entity("Domain.Entities.Sistema.Region", b =>
                {
                    b.Navigation("Secciones");
                });
#pragma warning restore 612, 618
        }
    }
}
