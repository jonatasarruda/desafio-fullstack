using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DesafioFullstack.Api.Migrations
{
    /// <inheritdoc />
    public partial class CriarEntidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "VARCHAR", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    DataInativacao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "VARCHAR", nullable: false),
                    Email = table.Column<string>(type: "VARCHAR", nullable: false),
                    Senha = table.Column<string>(type: "VARCHAR", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    DataInativacao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Atendimento",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TextoAberturaAtendimento = table.Column<string>(type: "VARCHAR", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    DataInativacao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atendimento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Atendimento_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Atendimento_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pareceres",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DescricaoParecer = table.Column<string>(type: "VARCHAR", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    AtendimentoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pareceres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pareceres_Atendimento_AtendimentoId",
                        column: x => x.AtendimentoId,
                        principalTable: "Atendimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pareceres_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atendimento_ClienteId",
                table: "Atendimento",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Atendimento_UsuarioId",
                table: "Atendimento",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Pareceres_AtendimentoId",
                table: "Pareceres",
                column: "AtendimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pareceres_UsuarioId",
                table: "Pareceres",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pareceres");

            migrationBuilder.DropTable(
                name: "Atendimento");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
