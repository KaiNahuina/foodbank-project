using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodbank_Project.Migrations
{
    public partial class ChangeRecipeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeRecipeCategory_RecipeCategories_CategoryRecipeCategoryId",
                table: "RecipeRecipeCategory");

            migrationBuilder.RenameColumn(
                name: "CategoryRecipeCategoryId",
                table: "RecipeRecipeCategory",
                newName: "CategoriesRecipeCategoryId");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Foodbanks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Homepage",
                table: "Foodbanks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeRecipeCategory_RecipeCategories_CategoriesRecipeCategoryId",
                table: "RecipeRecipeCategory",
                column: "CategoriesRecipeCategoryId",
                principalTable: "RecipeCategories",
                principalColumn: "RecipeCategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeRecipeCategory_RecipeCategories_CategoriesRecipeCategoryId",
                table: "RecipeRecipeCategory");

            migrationBuilder.RenameColumn(
                name: "CategoriesRecipeCategoryId",
                table: "RecipeRecipeCategory",
                newName: "CategoryRecipeCategoryId");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Foodbanks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Homepage",
                table: "Foodbanks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeRecipeCategory_RecipeCategories_CategoryRecipeCategoryId",
                table: "RecipeRecipeCategory",
                column: "CategoryRecipeCategoryId",
                principalTable: "RecipeCategories",
                principalColumn: "RecipeCategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
