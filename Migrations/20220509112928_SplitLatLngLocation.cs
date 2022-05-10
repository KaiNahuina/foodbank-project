using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodbank_Project.Migrations
{
    public partial class SplitLatLngLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LatLng",
                table: "Locations",
                newName: "Lng");

            migrationBuilder.AddColumn<string>(
                name: "Lat",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "Lng",
                table: "Locations",
                newName: "LatLng");
        }
    }
}
