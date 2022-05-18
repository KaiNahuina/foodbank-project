using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodbank_Project.Migrations
{
    public partial class CascadeLocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Foodbanks_FoodbankId",
                table: "Locations");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Foodbanks_FoodbankId",
                table: "Locations",
                column: "FoodbankId",
                principalTable: "Foodbanks",
                principalColumn: "FoodbankId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Foodbanks_FoodbankId",
                table: "Locations");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Foodbanks_FoodbankId",
                table: "Locations",
                column: "FoodbankId",
                principalTable: "Foodbanks",
                principalColumn: "FoodbankId");
        }
    }
}
