#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Foodbank_Project.Pages.Admin;

[Authorize(Roles = "UsersAdmin,SiteAdmin")]
public class RoleModel : PageModel
{
    private readonly ILogger<RoleModel> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public RoleModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
        ILogger<RoleModel> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public string? Action { get; set; }

    public string? Name { get; set; }
    public string? Target { get; set; }

    public List<IdentityRole>? Roles { get; set; }

    public async Task OnGetAsync([FromQuery(Name = "Action")] string? action)
    {
        Action = action ?? "Update";
        switch (Action)
        {
            case "Search":
            {
                Name = Request.Query["Name"];
                Target = Request.Query["Target"];
                Roles = await _roleManager.Roles!.AsNoTracking().Where(n => n.Name!.Contains(Name)).ToListAsync();
                break;
            }
            default:
            {
                Target = Request.Query["Target"];
                Roles = await _roleManager.Roles!.AsNoTracking().ToListAsync();
                break;
            }
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Action = Request.Form["Action"].ToString() ?? "Update";
        switch (Action)
        {
            case "Remove":
            {
                Target = Request.Form["Target"];
                var id = Request.RouteValues["id"]?.ToString();
                var targetUser = await _userManager.FindByIdAsync(Target);

                var result = await _userManager.RemoveFromRoleAsync(targetUser, id);
                if (!result.Succeeded)
                {
                    foreach (var identityError in result.Errors)
                        ModelState.AddModelError(string.Empty, identityError.Code + " :: " + identityError.Description);

                    return Page();
                }

                _logger.Log(LogLevel.Warning, "User {UserName} removed role {Role} to {TargetUser}",
                    User.Identity?.Name, id, targetUser.UserName);

                return RedirectToPage("./User", routeValues: new { id = Target }, fragment: "roles", pageHandler: "");
            }
            case "Add":
            {
                Target = Request.Form["Target"];
                var id = Request.RouteValues["id"]?.ToString();
                var targetUser = await _userManager.FindByIdAsync(Target);

                var result = await _userManager.AddToRoleAsync(targetUser, id);
                if (!result.Succeeded)
                {
                    foreach (var identityError in result.Errors)
                        ModelState.AddModelError(string.Empty, identityError.Code + " :: " + identityError.Description);

                    return Page();
                }

                _logger.Log(LogLevel.Information, "User {UserName} added role {Role} to {TargetUser}",
                    User.Identity?.Name, id, targetUser.UserName);

                return RedirectToPage("./User", routeValues: new { id = Target }, fragment: "roles", pageHandler: "");
            }
        }

        return Page();
    }
}