using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePlataforma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Plataformas_PlataformaId",
                table: "Roles");

            migrationBuilder.DropTable(
                name: "Plataformas");

            migrationBuilder.DropIndex(
                name: "IX_Roles_PlataformaId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "PlataformaId",
                table: "Roles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlataformaId",
                table: "Roles",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Plataformas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plataformas", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Plataformas",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "WEB" },
                    { 2, "APP" },
                    { 3, "MULTI" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_PlataformaId",
                table: "Roles",
                column: "PlataformaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Plataformas_PlataformaId",
                table: "Roles",
                column: "PlataformaId",
                principalTable: "Plataformas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
