using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.Viaticos
{
    /// <inheritdoc />
    public partial class Subcategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubcategoriaId",
                table: "Viaticos",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubcategoriaViatico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CategoriaId = table.Column<int>(type: "integer", nullable: false),
                    FacturasRequeridas = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubcategoriaViatico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubcategoriaViatico_CategoriasViatico_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "CategoriasViatico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "SubcategoriaViatico",
                columns: new[] { "Id", "CategoriaId", "FacturasRequeridas", "Nombre" },
                values: new object[] { 1, 1, 2, "Mantenimiento" });

            migrationBuilder.CreateIndex(
                name: "IX_Viaticos_SubcategoriaId",
                table: "Viaticos",
                column: "SubcategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_SubcategoriaViatico_CategoriaId",
                table: "SubcategoriaViatico",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Viaticos_SubcategoriaViatico_SubcategoriaId",
                table: "Viaticos",
                column: "SubcategoriaId",
                principalTable: "SubcategoriaViatico",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Viaticos_SubcategoriaViatico_SubcategoriaId",
                table: "Viaticos");

            migrationBuilder.DropTable(
                name: "SubcategoriaViatico");

            migrationBuilder.DropIndex(
                name: "IX_Viaticos_SubcategoriaId",
                table: "Viaticos");

            migrationBuilder.DropColumn(
                name: "SubcategoriaId",
                table: "Viaticos");
        }
    }
}
