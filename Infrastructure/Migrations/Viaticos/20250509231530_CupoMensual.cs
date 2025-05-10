using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.Viaticos
{
    /// <inheritdoc />
    public partial class CupoMensual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CupoMensual",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    CicloId = table.Column<int>(type: "integer", nullable: false),
                    Categoria = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MontoAsignado = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CupoMensual", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CupoMensual_UsuarioId_CicloId_Categoria",
                table: "CupoMensual",
                columns: new[] { "UsuarioId", "CicloId", "Categoria" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CupoMensual");
        }
    }
}
