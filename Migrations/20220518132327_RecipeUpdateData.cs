using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodbank_Project.Migrations
{
    public partial class RecipeUpdateData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RecipeCategories",
                keyColumn: "RecipeCategoryId",
                keyValue: -2,
                column: "Name",
                value: "Vegetarian");

            migrationBuilder.UpdateData(
                table: "RecipeCategories",
                keyColumn: "RecipeCategoryId",
                keyValue: -1,
                column: "Name",
                value: "Meat");

            migrationBuilder.InsertData(
                table: "RecipeCategories",
                columns: new[] { "RecipeCategoryId", "Name" },
                values: new object[,]
                {
                    { -8, "Special Event" },
                    { -7, "Side" },
                    { -6, "Snack" },
                    { -5, "Soup" },
                    { -4, "Desert" },
                    { -3, "Fish" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RecipeCategories",
                keyColumn: "RecipeCategoryId",
                keyValue: -8);

            migrationBuilder.DeleteData(
                table: "RecipeCategories",
                keyColumn: "RecipeCategoryId",
                keyValue: -7);

            migrationBuilder.DeleteData(
                table: "RecipeCategories",
                keyColumn: "RecipeCategoryId",
                keyValue: -6);

            migrationBuilder.DeleteData(
                table: "RecipeCategories",
                keyColumn: "RecipeCategoryId",
                keyValue: -5);

            migrationBuilder.DeleteData(
                table: "RecipeCategories",
                keyColumn: "RecipeCategoryId",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "RecipeCategories",
                keyColumn: "RecipeCategoryId",
                keyValue: -3);

            migrationBuilder.UpdateData(
                table: "RecipeCategories",
                keyColumn: "RecipeCategoryId",
                keyValue: -2,
                column: "Name",
                value: "Cat2");

            migrationBuilder.UpdateData(
                table: "RecipeCategories",
                keyColumn: "RecipeCategoryId",
                keyValue: -1,
                column: "Name",
                value: "Cat1");
        }
    }
}
