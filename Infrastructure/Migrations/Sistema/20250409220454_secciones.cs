using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations.Sistema
{
    /// <inheritdoc />
    public partial class secciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fuerzas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fuerzas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regiones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regiones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Secciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    RegionId = table.Column<int>(type: "integer", nullable: false),
                    FuerzaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Secciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Secciones_Fuerzas_FuerzaId",
                        column: x => x.FuerzaId,
                        principalTable: "Fuerzas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Secciones_Regiones_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Fuerzas",
                columns: new[] { "Id", "Codigo", "Nombre" },
                values: new object[,]
                {
                    { 1, "F1", "F1" },
                    { 2, "F2", "F2" },
                    { 3, "F3", "F3" },
                    { 4, "F4", "F4" }
                });

            migrationBuilder.InsertData(
                table: "Regiones",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Sierra" },
                    { 2, "Costa" },
                    { 3, "Austro" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Secciones_FuerzaId",
                table: "Secciones",
                column: "FuerzaId");

            migrationBuilder.CreateIndex(
                name: "IX_Secciones_RegionId",
                table: "Secciones",
                column: "RegionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Secciones");

            migrationBuilder.DropTable(
                name: "Fuerzas");

            migrationBuilder.DropTable(
                name: "Regiones");
        }
    }
}
