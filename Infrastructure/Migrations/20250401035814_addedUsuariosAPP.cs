using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedUsuariosAPP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlataformaId",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Plataformas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plataformas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosApp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosApp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuariosApp_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuariosApp_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosApp_NombreUsuario",
                table: "UsuariosApp",
                column: "NombreUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosApp_RolId",
                table: "UsuariosApp",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosApp_UsuarioId",
                table: "UsuariosApp",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Plataformas_PlataformaId",
                table: "Roles",
                column: "PlataformaId",
                principalTable: "Plataformas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Plataformas_PlataformaId",
                table: "Roles");

            migrationBuilder.DropTable(
                name: "Plataformas");

            migrationBuilder.DropTable(
                name: "UsuariosApp");

            migrationBuilder.DropIndex(
                name: "IX_Roles_PlataformaId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "PlataformaId",
                table: "Roles");
        }
    }
}
