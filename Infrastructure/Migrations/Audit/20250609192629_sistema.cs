﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.Audit
{
    /// <inheritdoc />
    public partial class sistema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditoriaSistema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoEvento = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioNombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    EntidadAfectada = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EntidadId = table.Column<int>(type: "integer", nullable: true),
                    Datos = table.Column<string>(type: "text", nullable: false),
                    Hash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IpCliente = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    MetodoHttp = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    RutaAccedida = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriaSistema", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditoriaSistema");
        }
    }
}
