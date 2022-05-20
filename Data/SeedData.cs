﻿#region

using Foodbank_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

#endregion

namespace Foodbank_Project.Data;

public static class SeedData
{
    public static async Task SeedRecipesAsync(ApplicationContext ctx)
    {
        var meatCategory = await ctx.RecipeCategories.FirstOrDefaultAsync(c => c.RecipeCategoryId == 1) ?? ctx.RecipeCategories.Add(new RecipeCategory
        {
            RecipeCategoryId = 1,
            Name = "Meat"
        }).Entity ;

        await using (var transaction = await ctx.Database.BeginTransactionAsync())
        {
            await ctx.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[RecipeCategories] ON");
            await ctx.SaveChangesAsync();
            await ctx.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[RecipeCategories] OFF");
            await transaction.CommitAsync();
        };
        
        
        var _ = await ctx.Recipes.FirstOrDefaultAsync(r => r.RecipeId == 1) ?? ctx.Recipes.Add(new Recipe
        {
            Name = "Steak and Chips",
            RecipeId = 1,
            Ingredients = "Steak \n Chips",
            Status = Status.Approved,
            Method = "Grill Steak\nCook Chips\nServe",
            Category = new List<RecipeCategory> { meatCategory },
            Notes = "",
            Serves = "9 People",
            Image = await File.ReadAllBytesAsync("./wwwroot/img/bag-icon.png")
        }).Entity;

        await using (var transaction = await ctx.Database.BeginTransactionAsync())
        {
            await ctx.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Recipes] ON");
            await ctx.SaveChangesAsync();
            await ctx.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Recipes] OFF");
            await transaction.CommitAsync();
        }
        
    }
    
    public static async Task SeedRolesAsync(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole("FoodbankAdmin"));
        await roleManager.CreateAsync(new IdentityRole("SiteAdmin"));
        await roleManager.CreateAsync(new IdentityRole("FoodbanksAdmin"));
        await roleManager.CreateAsync(new IdentityRole("UsersAdmin"));
        await roleManager.CreateAsync(new IdentityRole("ContentAdmin"));
        await roleManager.CreateAsync(new IdentityRole("RecipeAdmin"));
        await roleManager.CreateAsync(new IdentityRole("LoggingAdmin"));
    }


    public static async Task SeedBasicUserAsync(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        // Seed Basic User
        var defaultUser = new IdentityUser
        {
            UserName = "user@user.com",
            Email = "user@user.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "236!User?339");
                await userManager.AddToRoleAsync(defaultUser, "FoodbankAdmin");
            }
        }
    }

    public static async Task SeedAdminUserAsync(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        // Seed SuperAdmin User
        var defaultUser = new IdentityUser
        {
            UserName = "admin@admin.com",
            Email = "admin@admin.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "236!Admin?339");
                await userManager.AddToRoleAsync(defaultUser, "SiteAdmin");
            }
        }
    }
}