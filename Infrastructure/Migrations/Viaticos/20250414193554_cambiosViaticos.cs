using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.Viaticos
{
    /// <inheritdoc />
    public partial class cambiosViaticos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CicloId",
                table: "Viaticos");

            migrationBuilder.DropColumn(
                name: "EstadoCiclo",
                table: "Viaticos");

            migrationBuilder.RenameColumn(
                name: "IdUsuario",
                table: "Viaticos",
                newName: "SolicitudViaticoId");

            migrationBuilder.DropColumn(
                name: "EstadoViatico",
                table: "Viaticos");

            migrationBuilder.AddColumn<int>(
                name: "EstadoViatico",
                table: "Viaticos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificado",
                table: "Viaticos",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "SolicitudesViatico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioAppId = table.Column<int>(type: "integer", nullable: false),
                    CicloId = table.Column<int>(type: "integer", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FechaModificado = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Estado = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesViatico", x => x.Id);

                    table.ForeignKey(
                        name: "FK_SolicitudesViatico_UsuariosApp_UsuarioAppId",
                        column: x => x.UsuarioAppId,
                        principalTable: "UsuariosApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_SolicitudesViatico_Ciclos_CicloId",
                        column: x => x.CicloId,
                        principalTable: "Ciclos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesViatico_UsuarioAppId",
                table: "SolicitudesViatico",
                column: "UsuarioAppId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesViatico_CicloId",
                table: "SolicitudesViatico",
                column: "CicloId");

            migrationBuilder.UpdateData(
                table: "CategoriasViatico",
                keyColumn: "Id",
                keyValue: 1,
                column: "Nombre",
                value: "Movilización");

            migrationBuilder.UpdateData(
                table: "CategoriasViatico",
                keyColumn: "Id",
                keyValue: 2,
                column: "Nombre",
                value: "Alimentación");

            migrationBuilder.UpdateData(
                table: "CategoriasViatico",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nombre",
                value: "Hospedaje");

            migrationBuilder.CreateIndex(
                name: "IX_Viaticos_SolicitudViaticoId",
                table: "Viaticos",
                column: "SolicitudViaticoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Viaticos_SolicitudesViatico_SolicitudViaticoId",
                table: "Viaticos",
                column: "SolicitudViaticoId",
                principalTable: "SolicitudesViatico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Viaticos_SolicitudesViatico_SolicitudViaticoId",
                table: "Viaticos");

            migrationBuilder.DropTable(
                name: "SolicitudesViatico");

            migrationBuilder.DropIndex(
                name: "IX_Viaticos_SolicitudViaticoId",
                table: "Viaticos");

            migrationBuilder.DropColumn(
                name: "FechaModificado",
                table: "Viaticos");

            migrationBuilder.RenameColumn(
                name: "SolicitudViaticoId",
                table: "Viaticos",
                newName: "IdUsuario");

            migrationBuilder.AlterColumn<string>(
                name: "EstadoViatico",
                table: "Viaticos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "CicloId",
                table: "Viaticos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EstadoCiclo",
                table: "Viaticos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "CategoriasViatico",
                keyColumn: "Id",
                keyValue: 1,
                column: "Nombre",
                value: "movilizacion");

            migrationBuilder.UpdateData(
                table: "CategoriasViatico",
                keyColumn: "Id",
                keyValue: 2,
                column: "Nombre",
                value: "alimentacion");

            migrationBuilder.UpdateData(
                table: "CategoriasViatico",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nombre",
                value: "hospedaje");
        }
    }
}
