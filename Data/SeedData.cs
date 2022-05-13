#region

using Microsoft.AspNetCore.Identity;

#endregion

namespace Foodbank_Project.Data;

public class SeedData
{
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