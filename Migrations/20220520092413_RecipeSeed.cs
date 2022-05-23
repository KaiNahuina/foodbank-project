using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodbank_Project.Migrations
{
    public partial class RecipeSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DeleteData(
                table: "RecipeCategories",
                keyColumn: "RecipeCategoryId",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "RecipeCategories",
                keyColumn: "RecipeCategoryId",
                keyValue: -1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                    { -3, "Fish" },
                    { -2, "Vegetarian" },
                    { -1, "Meat" }
                });
        }
    }
}
