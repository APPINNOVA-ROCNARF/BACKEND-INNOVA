using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Usuarios
{
    /// <inheritdoc />
    public partial class cambioRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Roles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RolSecciones",
                columns: table => new
                {
                    RolId = table.Column<int>(type: "integer", nullable: false),
                    SeccionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolSecciones", x => new { x.RolId, x.SeccionId });

                    table.ForeignKey(
                        name: "FK_RolSecciones_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        name: "FK_RolSecciones_Secciones_SeccionId",
                        column: x => x.SeccionId,
                        principalTable: "Secciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict); // o Cascade si así lo deseas
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolSecciones_SeccionId",
                table: "RolSecciones",
                column: "SeccionId");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolSecciones");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Roles");
        }
    }
}
