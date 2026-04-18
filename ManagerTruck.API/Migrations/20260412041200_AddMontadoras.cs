using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ManagerTruck.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMontadoras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Despesas");

            migrationBuilder.RenameColumn(
                name: "Ano",
                table: "Caminhoes",
                newName: "MontadoraId");

            migrationBuilder.AddColumn<int>(
                name: "AnoFabricacao",
                table: "Caminhoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnoModelo",
                table: "Caminhoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KmAtual",
                table: "Caminhoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KmUltimaManutencao",
                table: "Caminhoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Montadoras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Montadoras", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Montadoras",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Volvo" },
                    { 2, "Scania" },
                    { 3, "Mercedes-Benz" },
                    { 4, "DAF" },
                    { 5, "Iveco" },
                    { 6, "Volkswagen" },
                    { 7, "Ford" },
                    { 8, "MAN" },
                    { 9, "Hyundai" },
                    { 10, "Kia" },
                    { 11, "Isuzu" },
                    { 12, "Foton" },
                    { 13, "JAC Motors" },
                    { 14, "Agrale" },
                    { 15, "International" },
                    { 16, "Peterbilt" },
                    { 17, "Kenworth" },
                    { 18, "Freightliner" },
                    { 19, "Mack" },
                    { 20, "Renault Trucks" },
                    { 21, "Hino" },
                    { 22, "Tatra" },
                    { 23, "Sinotruk" },
                    { 24, "Shacman" },
                    { 25, "Dongfeng" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Caminhoes_MontadoraId",
                table: "Caminhoes",
                column: "MontadoraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Caminhoes_Montadoras_MontadoraId",
                table: "Caminhoes",
                column: "MontadoraId",
                principalTable: "Montadoras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Caminhoes_Montadoras_MontadoraId",
                table: "Caminhoes");

            migrationBuilder.DropTable(
                name: "Montadoras");

            migrationBuilder.DropIndex(
                name: "IX_Caminhoes_MontadoraId",
                table: "Caminhoes");

            migrationBuilder.DropColumn(
                name: "AnoFabricacao",
                table: "Caminhoes");

            migrationBuilder.DropColumn(
                name: "AnoModelo",
                table: "Caminhoes");

            migrationBuilder.DropColumn(
                name: "KmAtual",
                table: "Caminhoes");

            migrationBuilder.DropColumn(
                name: "KmUltimaManutencao",
                table: "Caminhoes");

            migrationBuilder.RenameColumn(
                name: "MontadoraId",
                table: "Caminhoes",
                newName: "Ano");

            migrationBuilder.CreateTable(
                name: "Despesas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Anexo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Data = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Descricao = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Detalhes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Valor = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Despesas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
