using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Viaticos
{
    /// <inheritdoc />
    public partial class solicitudVehiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SolicitudVehiculoPrincipal_VehiculoIdSolicitado",
                table: "SolicitudVehiculoPrincipal",
                column: "VehiculoIdSolicitado");

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitudVehiculoPrincipal_Vehiculos_VehiculoIdSolicitado",
                table: "SolicitudVehiculoPrincipal",
                column: "VehiculoIdSolicitado",
                principalTable: "Vehiculos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolicitudVehiculoPrincipal_Vehiculos_VehiculoIdSolicitado",
                table: "SolicitudVehiculoPrincipal");

            migrationBuilder.DropIndex(
                name: "IX_SolicitudVehiculoPrincipal_VehiculoIdSolicitado",
                table: "SolicitudVehiculoPrincipal");
        }
    }
}
