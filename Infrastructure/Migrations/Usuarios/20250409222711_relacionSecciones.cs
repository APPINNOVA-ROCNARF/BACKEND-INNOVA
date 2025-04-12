using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Usuarios
{
    public partial class relacionSecciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuarioAppSeccion",
                columns: table => new
                {
                    UsuarioAppId = table.Column<int>(type: "integer", nullable: false),
                    SeccionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioAppSeccion", x => new { x.UsuarioAppId, x.SeccionId });

                    table.ForeignKey(
                        name: "FK_UsuarioAppSeccion_UsuariosApp_UsuarioAppId",
                        column: x => x.UsuarioAppId,
                        principalTable: "UsuariosApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        name: "FK_UsuarioAppSeccion_Secciones_SeccionId",
                        column: x => x.SeccionId,
                        principalTable: "Secciones",  // Esta tabla debe existir en la misma base
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict); // o Cascade, según tu lógica
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioAppSeccion_SeccionId",
                table: "UsuarioAppSeccion",
                column: "SeccionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioAppSeccion");
        }
    }
}
