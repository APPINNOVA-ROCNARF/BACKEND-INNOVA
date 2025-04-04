using System;
using System.Collections.Generic;
using Domain.Entities.Viaticos;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations.Viaticos
{
    /// <inheritdoc />
    public partial class initViaticos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    Ruc = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RazonSocial = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.Ruc);
                });

            migrationBuilder.CreateTable(
                name: "Vehiculos",
                columns: table => new
                {
                    Placa = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculos", x => x.Placa);
                });

            migrationBuilder.CreateTable(
                name: "Facturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumeroFactura = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FechaFactura = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RucProveedor = table.Column<string>(type: "character varying(20)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    SubtotalIva = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Total = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    RutaImagen = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facturas_Proveedores_RucProveedor",
                        column: x => x.RucProveedor,
                        principalTable: "Proveedores",
                        principalColumn: "Ruc",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Viaticos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    CicloId = table.Column<int>(type: "integer", nullable: false),
                    CategoriaId = table.Column<int>(type: "integer", nullable: false),
                    FacturaId = table.Column<int>(type: "integer", nullable: true),
                    PlacaVehiculo = table.Column<string>(type: "character varying(20)", nullable: true),
                    EstadoViatico = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    EstadoCiclo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Comentario = table.Column<string>(type: "text", nullable: true),
                    CamposRechazados = table.Column<List<CampoRechazado>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Viaticos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Viaticos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Viaticos_Facturas_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "Facturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Viaticos_Vehiculos_PlacaVehiculo",
                        column: x => x.PlacaVehiculo,
                        principalTable: "Vehiculos",
                        principalColumn: "Placa",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Estado", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "movilizacion" },
                    { 2, true, "alimentacion" },
                    { 3, true, "hospedaje" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_RucProveedor",
                table: "Facturas",
                column: "RucProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Viaticos_CategoriaId",
                table: "Viaticos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Viaticos_FacturaId",
                table: "Viaticos",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_Viaticos_PlacaVehiculo",
                table: "Viaticos",
                column: "PlacaVehiculo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Viaticos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.DropTable(
                name: "Vehiculos");

            migrationBuilder.DropTable(
                name: "Proveedores");
        }
    }
}
