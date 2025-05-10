using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Audit
{
    /// <inheritdoc />
    public partial class CambiosTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpCliente",
                table: "Auditorias",
                type: "character varying(45)",
                maxLength: 45,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetodoHttp",
                table: "Auditorias",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RutaAccedida",
                table: "Auditorias",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioNombre",
                table: "Auditorias",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpCliente",
                table: "Auditorias");

            migrationBuilder.DropColumn(
                name: "MetodoHttp",
                table: "Auditorias");

            migrationBuilder.DropColumn(
                name: "RutaAccedida",
                table: "Auditorias");

            migrationBuilder.DropColumn(
                name: "UsuarioNombre",
                table: "Auditorias");
        }
    }
}
