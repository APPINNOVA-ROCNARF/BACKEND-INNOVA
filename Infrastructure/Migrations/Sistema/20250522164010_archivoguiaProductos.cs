using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Sistema
{
    /// <inheritdoc />
    public partial class archivoguiaProductos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArchivoGuiaProducto_GuiasProducto_GuiaProductoId",
                table: "ArchivoGuiaProducto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArchivoGuiaProducto",
                table: "ArchivoGuiaProducto");

            migrationBuilder.RenameTable(
                name: "ArchivoGuiaProducto",
                newName: "ArchivosGuiaProducto");

            migrationBuilder.RenameIndex(
                name: "IX_ArchivoGuiaProducto_GuiaProductoId",
                table: "ArchivosGuiaProducto",
                newName: "IX_ArchivosGuiaProducto_GuiaProductoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArchivosGuiaProducto",
                table: "ArchivosGuiaProducto",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArchivosGuiaProducto_GuiasProducto_GuiaProductoId",
                table: "ArchivosGuiaProducto",
                column: "GuiaProductoId",
                principalTable: "GuiasProducto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArchivosGuiaProducto_GuiasProducto_GuiaProductoId",
                table: "ArchivosGuiaProducto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArchivosGuiaProducto",
                table: "ArchivosGuiaProducto");

            migrationBuilder.RenameTable(
                name: "ArchivosGuiaProducto",
                newName: "ArchivoGuiaProducto");

            migrationBuilder.RenameIndex(
                name: "IX_ArchivosGuiaProducto_GuiaProductoId",
                table: "ArchivoGuiaProducto",
                newName: "IX_ArchivoGuiaProducto_GuiaProductoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArchivoGuiaProducto",
                table: "ArchivoGuiaProducto",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArchivoGuiaProducto_GuiasProducto_GuiaProductoId",
                table: "ArchivoGuiaProducto",
                column: "GuiaProductoId",
                principalTable: "GuiasProducto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
