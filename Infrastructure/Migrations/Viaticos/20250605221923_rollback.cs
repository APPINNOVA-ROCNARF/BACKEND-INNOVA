using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Viaticos
{
    /// <inheritdoc />
    public partial class rollback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "FacturasRequeridas",
                table: "SubcategoriaViatico");

            migrationBuilder.AddColumn<int>(
                name: "FacturaId",
                table: "Viaticos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Viaticos_FacturaId",
                table: "Viaticos",
                column: "FacturaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Viaticos_FacturasViatico_FacturaId",
                table: "Viaticos",
                column: "FacturaId",
                principalTable: "FacturasViatico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Viaticos_FacturasViatico_FacturaId",
                table: "Viaticos");

            migrationBuilder.DropIndex(
                name: "IX_Viaticos_FacturaId",
                table: "Viaticos");

            migrationBuilder.DropColumn(
                name: "FacturaId",
                table: "Viaticos");

            migrationBuilder.AddColumn<int>(
                name: "FacturasRequeridas",
                table: "SubcategoriaViatico",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "SubcategoriaViatico",
                keyColumn: "Id",
                keyValue: 1,
                column: "FacturasRequeridas",
                value: 2);

            migrationBuilder.CreateIndex(
                name: "IX_RelacionViaticoFacturas_FacturaId",
                table: "RelacionViaticoFacturas",
                column: "FacturaId");
        }
    }
}
