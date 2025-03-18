using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedModulos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "Modulo",
                table: "Permisos");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ModuloId",
                table: "Permisos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Modulos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modulos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolModulos",
                columns: table => new
                {
                    RolId = table.Column<int>(type: "int", nullable: false),
                    ModuloId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolModulos", x => new { x.RolId, x.ModuloId });
                    table.ForeignKey(
                        name: "FK_RolModulos_Modulos_ModuloId",
                        column: x => x.ModuloId,
                        principalTable: "Modulos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolModulos_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_ModuloId",
                table: "Permisos",
                column: "ModuloId");

            migrationBuilder.CreateIndex(
                name: "IX_RolModulos_ModuloId",
                table: "RolModulos",
                column: "ModuloId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permisos_Modulos_ModuloId",
                table: "Permisos",
                column: "ModuloId",
                principalTable: "Modulos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permisos_Modulos_ModuloId",
                table: "Permisos");

            migrationBuilder.DropTable(
                name: "RolModulos");

            migrationBuilder.DropTable(
                name: "Modulos");

            migrationBuilder.DropIndex(
                name: "IX_Permisos_ModuloId",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "ModuloId",
                table: "Permisos");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Permisos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Modulo",
                table: "Permisos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
