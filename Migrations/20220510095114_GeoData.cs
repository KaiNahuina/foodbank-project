using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Foodbank_Project.Migrations
{
    public partial class GeoData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Lng",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Foodbanks");

            migrationBuilder.DropColumn(
                name: "Lng",
                table: "Foodbanks");

            migrationBuilder.AddColumn<Point>(
                name: "Coord",
                table: "Locations",
                type: "geography",
                nullable: false);

            migrationBuilder.AddColumn<Point>(
                name: "Coord",
                table: "Foodbanks",
                type: "geography",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coord",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Coord",
                table: "Foodbanks");

            migrationBuilder.AddColumn<string>(
                name: "Lat",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Lng",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Lat",
                table: "Foodbanks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Lng",
                table: "Foodbanks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
