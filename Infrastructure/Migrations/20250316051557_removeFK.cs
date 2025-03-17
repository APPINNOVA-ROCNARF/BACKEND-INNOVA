using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuariosWeb_Roles_RolId1",
                table: "UsuariosWeb");

            migrationBuilder.DropIndex(
                name: "IX_UsuariosWeb_RolId1",
                table: "UsuariosWeb");

            migrationBuilder.DropColumn(
                name: "RolId1",
                table: "UsuariosWeb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RolId1",
                table: "UsuariosWeb",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosWeb_RolId1",
                table: "UsuariosWeb",
                column: "RolId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuariosWeb_Roles_RolId1",
                table: "UsuariosWeb",
                column: "RolId1",
                principalTable: "Roles",
                principalColumn: "Id");
        }
    }
}
