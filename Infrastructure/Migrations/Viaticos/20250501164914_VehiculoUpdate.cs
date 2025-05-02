using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.Viaticos
{
    /// <inheritdoc />
    public partial class VehiculoUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Viaticos_Vehiculos_PlacaVehiculo",
                table: "Viaticos");

            migrationBuilder.DropIndex(
                name: "IX_Viaticos_PlacaVehiculo",
                table: "Viaticos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehiculos",
                table: "Vehiculos");

            migrationBuilder.DropColumn(
                name: "PlacaVehiculo",
                table: "Viaticos");

            migrationBuilder.AddColumn<int>(
                name: "VehiculoId",
                table: "Viaticos",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Placa",
                table: "Vehiculos",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Vehiculos",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Vehiculos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Fabricante",
                table: "Vehiculos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "Vehiculos",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Modelo",
                table: "Vehiculos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioAppId",
                table: "Vehiculos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehiculos",
                table: "Vehiculos",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SolicitudVehiculoPrincipal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioAppId = table.Column<int>(type: "integer", nullable: false),
                    VehiculoIdSolicitado = table.Column<int>(type: "integer", nullable: false),
                    Motivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AprobadoPorUsuarioId = table.Column<int>(type: "integer", nullable: true),
                    FechaAprobacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Estado = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudVehiculoPrincipal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehiculoPrincipal",
                columns: table => new
                {
                    UsuarioAppId = table.Column<int>(type: "integer", nullable: false),
                    VehiculoId = table.Column<int>(type: "integer", nullable: false),
                    FechaModificado = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehiculoPrincipal", x => x.UsuarioAppId);
                    table.ForeignKey(
                        name: "FK_VehiculoPrincipal_Vehiculos_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "Vehiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Viaticos_VehiculoId",
                table: "Viaticos",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_UsuarioAppId_Placa",
                table: "Vehiculos",
                columns: new[] { "UsuarioAppId", "Placa" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehiculoPrincipal_VehiculoId",
                table: "VehiculoPrincipal",
                column: "VehiculoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Viaticos_Vehiculos_VehiculoId",
                table: "Viaticos",
                column: "VehiculoId",
                principalTable: "Vehiculos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Viaticos_Vehiculos_VehiculoId",
                table: "Viaticos");

            migrationBuilder.DropTable(
                name: "SolicitudVehiculoPrincipal");

            migrationBuilder.DropTable(
                name: "VehiculoPrincipal");

            migrationBuilder.DropIndex(
                name: "IX_Viaticos_VehiculoId",
                table: "Viaticos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehiculos",
                table: "Vehiculos");

            migrationBuilder.DropIndex(
                name: "IX_Vehiculos_UsuarioAppId_Placa",
                table: "Vehiculos");

            migrationBuilder.DropColumn(
                name: "VehiculoId",
                table: "Viaticos");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Vehiculos");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Vehiculos");

            migrationBuilder.DropColumn(
                name: "Fabricante",
                table: "Vehiculos");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "Vehiculos");

            migrationBuilder.DropColumn(
                name: "Modelo",
                table: "Vehiculos");

            migrationBuilder.DropColumn(
                name: "UsuarioAppId",
                table: "Vehiculos");

            migrationBuilder.AddColumn<string>(
                name: "PlacaVehiculo",
                table: "Viaticos",
                type: "character varying(20)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Placa",
                table: "Vehiculos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehiculos",
                table: "Vehiculos",
                column: "Placa");

            migrationBuilder.CreateIndex(
                name: "IX_Viaticos_PlacaVehiculo",
                table: "Viaticos",
                column: "PlacaVehiculo");

            migrationBuilder.AddForeignKey(
                name: "FK_Viaticos_Vehiculos_PlacaVehiculo",
                table: "Viaticos",
                column: "PlacaVehiculo",
                principalTable: "Vehiculos",
                principalColumn: "Placa",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
