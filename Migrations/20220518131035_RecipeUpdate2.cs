using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodbank_Project.Migrations
{
    public partial class RecipeUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_RecipeCategories_CategoryRecipeCategoryId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_CategoryRecipeCategoryId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "CategoryRecipeCategoryId",
                table: "Recipes");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Recipes",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Serves",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RecipeRecipeCategory",
                columns: table => new
                {
                    CategoryRecipeCategoryId = table.Column<int>(type: "int", nullable: false),
                    RecipesRecipeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeRecipeCategory", x => new { x.CategoryRecipeCategoryId, x.RecipesRecipeId });
                    table.ForeignKey(
                        name: "FK_RecipeRecipeCategory_RecipeCategories_CategoryRecipeCategoryId",
                        column: x => x.CategoryRecipeCategoryId,
                        principalTable: "RecipeCategories",
                        principalColumn: "RecipeCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeRecipeCategory_Recipes_RecipesRecipeId",
                        column: x => x.RecipesRecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "RecipeCategories",
                columns: new[] { "RecipeCategoryId", "Name" },
                values: new object[] { -2, "Cat2" });

            migrationBuilder.InsertData(
                table: "RecipeCategories",
                columns: new[] { "RecipeCategoryId", "Name" },
                values: new object[] { -1, "Cat1" });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeRecipeCategory_RecipesRecipeId",
                table: "RecipeRecipeCategory",
                column: "RecipesRecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeRecipeCategory");

            migrationBuilder.DeleteData(
                table: "RecipeCategories",
                keyColumn: "RecipeCategoryId",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "RecipeCategories",
                keyColumn: "RecipeCategoryId",
                keyValue: -1);

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Serves",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Recipes");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Recipes",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryRecipeCategoryId",
                table: "Recipes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CategoryRecipeCategoryId",
                table: "Recipes",
                column: "CategoryRecipeCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_RecipeCategories_CategoryRecipeCategoryId",
                table: "Recipes",
                column: "CategoryRecipeCategoryId",
                principalTable: "RecipeCategories",
                principalColumn: "RecipeCategoryId");
        }
    }
}
