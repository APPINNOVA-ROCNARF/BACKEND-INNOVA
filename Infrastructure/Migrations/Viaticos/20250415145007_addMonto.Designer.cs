﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Domain.Entities.Viaticos;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.Viaticos
{
    [DbContext(typeof(ViaticosDbContext))]
    [Migration("20250415145007_addMonto")]
    partial class addMonto
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Viaticos.CategoriaViatico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Estado")
                        .HasColumnType("boolean");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("CategoriasViatico");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Estado = true,
                            Nombre = "Movilización"
                        },
                        new
                        {
                            Id = 2,
                            Estado = true,
                            Nombre = "Alimentación"
                        },
                        new
                        {
                            Id = 3,
                            Estado = true,
                            Nombre = "Hospedaje"
                        });
                });

            modelBuilder.Entity("Domain.Entities.Viaticos.FacturaViatico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("FechaFactura")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("NumeroFactura")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("RucProveedor")
                        .IsRequired()
                        .HasColumnType("character varying(20)");

                    b.Property<string>("RutaImagen")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<decimal>("Subtotal")
                        .HasColumnType("numeric(10,2)");

                    b.Property<decimal>("SubtotalIva")
                        .HasColumnType("numeric(10,2)");

                    b.Property<decimal>("Total")
                        .HasColumnType("numeric(10,2)");

                    b.HasKey("Id");

                    b.HasIndex("RucProveedor");

                    b.ToTable("FacturasViatico");
                });

            modelBuilder.Entity("Domain.Entities.Viaticos.ProveedorViatico", b =>
                {
                    b.Property<string>("Ruc")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("RazonSocial")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Ruc");

                    b.ToTable("ProveedoresViatico");
                });

            modelBuilder.Entity("Domain.Entities.Viaticos.SolicitudViatico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CicloId")
                        .HasColumnType("integer");

                    b.Property<int>("Estado")
                        .HasColumnType("integer");

                    b.Property<DateTime>("FechaModificado")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("Monto")
                        .HasColumnType("numeric");

                    b.Property<int>("UsuarioAppId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("SolicitudesViatico");
                });

            modelBuilder.Entity("Domain.Entities.Viaticos.Vehiculo", b =>
                {
                    b.Property<string>("Placa")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Placa");

                    b.ToTable("Vehiculos");
                });

            modelBuilder.Entity("Domain.Entities.Viaticos.Viatico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<List<CampoRechazado>>("CamposRechazados")
                        .HasColumnType("jsonb");

                    b.Property<int>("CategoriaId")
                        .HasColumnType("integer");

                    b.Property<string>("Comentario")
                        .HasColumnType("text");

                    b.Property<int>("EstadoViatico")
                        .HasColumnType("integer");

                    b.Property<int?>("FacturaId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("FechaModificado")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("Monto")
                        .HasColumnType("numeric");

                    b.Property<string>("PlacaVehiculo")
                        .HasColumnType("character varying(20)");

                    b.Property<int>("SolicitudViaticoId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("FacturaId");

                    b.HasIndex("PlacaVehiculo");

                    b.HasIndex("SolicitudViaticoId");

                    b.ToTable("Viaticos");
                });

            modelBuilder.Entity("Domain.Entities.Viaticos.FacturaViatico", b =>
                {
                    b.HasOne("Domain.Entities.Viaticos.ProveedorViatico", "Proveedor")
                        .WithMany("Facturas")
                        .HasForeignKey("RucProveedor")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Proveedor");
                });

            modelBuilder.Entity("Domain.Entities.Viaticos.Viatico", b =>
                {
                    b.HasOne("Domain.Entities.Viaticos.CategoriaViatico", "Categoria")
                        .WithMany("Viaticos")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Viaticos.FacturaViatico", "Factura")
                        .WithMany("Viaticos")
                        .HasForeignKey("FacturaId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Domain.Entities.Viaticos.Vehiculo", "Vehiculo")
                        .WithMany("Viaticos")
                        .HasForeignKey("PlacaVehiculo")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Domain.Entities.Viaticos.SolicitudViatico", "SolicitudViatico")
                        .WithMany("Viaticos")
                        .HasForeignKey("SolicitudViaticoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Categoria");

                    b.Navigation("Factura");

                    b.Navigation("SolicitudViatico");

                    b.Navigation("Vehiculo");
                });

            modelBuilder.Entity("Domain.Entities.Viaticos.CategoriaViatico", b =>
                {
                    b.Navigation("Viaticos");
                });

            modelBuilder.Entity("Domain.Entities.Viaticos.FacturaViatico", b =>
                {
                    b.Navigation("Viaticos");
                });

            modelBuilder.Entity("Domain.Entities.Viaticos.ProveedorViatico", b =>
                {
                    b.Navigation("Facturas");
                });

            modelBuilder.Entity("Domain.Entities.Viaticos.SolicitudViatico", b =>
                {
                    b.Navigation("Viaticos");
                });

            modelBuilder.Entity("Domain.Entities.Viaticos.Vehiculo", b =>
                {
                    b.Navigation("Viaticos");
                });
#pragma warning restore 612, 618
        }
    }
}
