using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.Sistema
{
    /// <inheritdoc />
    public partial class guiaProductos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GuiasProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Marca = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    UrlVideo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FuerzaId = table.Column<int>(type: "integer", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuiasProducto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuiasProducto_Fuerzas_FuerzaId",
                        column: x => x.FuerzaId,
                        principalTable: "Fuerzas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArchivoGuiaProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuiaProductoId = table.Column<int>(type: "integer", nullable: false),
                    NombreOriginal = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    RutaRelativa = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Extension = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchivoGuiaProducto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArchivoGuiaProducto_GuiasProducto_GuiaProductoId",
                        column: x => x.GuiaProductoId,
                        principalTable: "GuiasProducto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArchivoGuiaProducto_GuiaProductoId",
                table: "ArchivoGuiaProducto",
                column: "GuiaProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_GuiasProducto_FuerzaId",
                table: "GuiasProducto",
                column: "FuerzaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArchivoGuiaProducto");

            migrationBuilder.DropTable(
                name: "GuiasProducto");
        }
    }
}
