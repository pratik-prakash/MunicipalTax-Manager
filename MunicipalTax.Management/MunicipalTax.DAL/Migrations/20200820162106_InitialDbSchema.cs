using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MunicipalTax.DAL.Migrations
{
    public partial class InitialDbSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MunicipalityTaxes",
                columns: table => new
                {
                    MunicipalityTaxId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MunicipalityName = table.Column<string>(nullable: true),
                    TaxType = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Tax = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalityTaxes", x => x.MunicipalityTaxId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MunicipalityTaxes");
        }
    }
}
