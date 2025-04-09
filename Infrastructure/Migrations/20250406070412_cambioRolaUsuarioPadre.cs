using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class cambioRolaUsuarioPadre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuariosApp_Roles_RolId",
                table: "UsuariosApp");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuariosWeb_Roles_RolId",
                table: "UsuariosWeb");

            migrationBuilder.DropIndex(
                name: "IX_UsuariosWeb_RolId",
                table: "UsuariosWeb");

            migrationBuilder.DropIndex(
                name: "IX_UsuariosApp_RolId",
                table: "UsuariosApp");

            migrationBuilder.DropColumn(
                name: "RolId",
                table: "UsuariosWeb");

            migrationBuilder.DropColumn(
                name: "RolId",
                table: "UsuariosApp");

            migrationBuilder.AddColumn<int>(
                name: "RolId",
                table: "Usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios",
                column: "RolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Roles_RolId",
                table: "Usuarios",
                column: "RolId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Roles_RolId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "RolId",
                table: "Usuarios");

            migrationBuilder.AddColumn<int>(
                name: "RolId",
                table: "UsuariosWeb",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RolId",
                table: "UsuariosApp",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosWeb_RolId",
                table: "UsuariosWeb",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosApp_RolId",
                table: "UsuariosApp",
                column: "RolId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuariosApp_Roles_RolId",
                table: "UsuariosApp",
                column: "RolId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuariosWeb_Roles_RolId",
                table: "UsuariosWeb",
                column: "RolId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
