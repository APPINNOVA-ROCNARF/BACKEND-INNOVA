using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Viaticos
{
    /// <inheritdoc />
    public partial class modificarPlaca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehiculos_UsuarioAppId_Placa",
                table: "Vehiculos");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_Placa",
                table: "Vehiculos",
                column: "Placa",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehiculos_Placa",
                table: "Vehiculos");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_UsuarioAppId_Placa",
                table: "Vehiculos",
                columns: new[] { "UsuarioAppId", "Placa" },
                unique: true);
        }
    }
}
