using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Viaticos
{
    /// <inheritdoc />
    public partial class relacionViaticoFactura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "RelacionViaticoFacturas",
                columns: table => new
                {
                    ViaticoId = table.Column<int>(type: "integer", nullable: false),
                    FacturaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelacionViaticoFacturas", x => new { x.ViaticoId, x.FacturaId });
                    table.ForeignKey(
                        name: "FK_RelacionViaticoFacturas_FacturasViatico_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "FacturasViatico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelacionViaticoFacturas_Viaticos_ViaticoId",
                        column: x => x.ViaticoId,
                        principalTable: "Viaticos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RelacionViaticoFacturas_FacturaId",
                table: "RelacionViaticoFacturas",
                column: "FacturaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelacionViaticoFacturas");

            migrationBuilder.AddColumn<int>(
                name: "FacturaId",
                table: "Viaticos",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Viaticos_FacturaId",
                table: "Viaticos",
                column: "FacturaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Viaticos_FacturasViatico_FacturaId",
                table: "Viaticos",
                column: "FacturaId",
                principalTable: "FacturasViatico",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
