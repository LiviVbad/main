using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppFramework.Migrations
{
    public partial class Add_AbpVersion_Field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AlgorithmValue",
                table: "AbpVersions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HashingAlgorithm",
                table: "AbpVersions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MinimumVersion",
                table: "AbpVersions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlgorithmValue",
                table: "AbpVersions");

            migrationBuilder.DropColumn(
                name: "HashingAlgorithm",
                table: "AbpVersions");

            migrationBuilder.DropColumn(
                name: "MinimumVersion",
                table: "AbpVersions");
        }
    }
}
