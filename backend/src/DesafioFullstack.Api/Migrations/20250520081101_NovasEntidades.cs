using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesafioFullstack.Api.Migrations
{
    /// <inheritdoc />
    public partial class NovasEntidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataInativacao",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "DataInativacao",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "DataInativacao",
                table: "Atendimento");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Usuario",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Cliente",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Atendimento",
                type: "VARCHAR",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Atendimento");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInativacao",
                table: "Usuario",
                type: "TIMESTAMP",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInativacao",
                table: "Cliente",
                type: "TIMESTAMP",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInativacao",
                table: "Atendimento",
                type: "TIMESTAMP",
                nullable: true);
        }
    }
}
