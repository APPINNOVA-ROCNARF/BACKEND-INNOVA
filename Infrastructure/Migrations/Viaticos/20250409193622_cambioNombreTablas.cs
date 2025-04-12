using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations.Viaticos
{
    /// <inheritdoc />
    public partial class cambioNombreTablas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Viaticos_Categorias_CategoriaId",
                table: "Viaticos");

            migrationBuilder.DropForeignKey(
                name: "FK_Viaticos_Facturas_FacturaId",
                table: "Viaticos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.CreateTable(
                name: "CategoriasViatico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasViatico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProveedoresViatico",
                columns: table => new
                {
                    Ruc = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RazonSocial = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProveedoresViatico", x => x.Ruc);
                });

            migrationBuilder.CreateTable(
                name: "FacturasViatico",
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
                    table.PrimaryKey("PK_FacturasViatico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacturasViatico_ProveedoresViatico_RucProveedor",
                        column: x => x.RucProveedor,
                        principalTable: "ProveedoresViatico",
                        principalColumn: "Ruc",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CategoriasViatico",
                columns: new[] { "Id", "Estado", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "movilizacion" },
                    { 2, true, "alimentacion" },
                    { 3, true, "hospedaje" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacturasViatico_RucProveedor",
                table: "FacturasViatico",
                column: "RucProveedor");

            migrationBuilder.AddForeignKey(
                name: "FK_Viaticos_CategoriasViatico_CategoriaId",
                table: "Viaticos",
                column: "CategoriaId",
                principalTable: "CategoriasViatico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Viaticos_FacturasViatico_FacturaId",
                table: "Viaticos",
                column: "FacturaId",
                principalTable: "FacturasViatico",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Viaticos_CategoriasViatico_CategoriaId",
                table: "Viaticos");

            migrationBuilder.DropForeignKey(
                name: "FK_Viaticos_FacturasViatico_FacturaId",
                table: "Viaticos");

            migrationBuilder.DropTable(
                name: "CategoriasViatico");

            migrationBuilder.DropTable(
                name: "FacturasViatico");

            migrationBuilder.DropTable(
                name: "ProveedoresViatico");

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Estado = table.Column<bool>(type: "boolean", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
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
                name: "Facturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RucProveedor = table.Column<string>(type: "character varying(20)", nullable: false),
                    FechaFactura = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    NumeroFactura = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RutaImagen = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    SubtotalIva = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Total = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
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

            migrationBuilder.AddForeignKey(
                name: "FK_Viaticos_Categorias_CategoriaId",
                table: "Viaticos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Viaticos_Facturas_FacturaId",
                table: "Viaticos",
                column: "FacturaId",
                principalTable: "Facturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
