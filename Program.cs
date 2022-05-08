#region

using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Foodbank_Project.Services.Scraping;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

#endregion

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDb") ?? string.Empty));;

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationContext>();;

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDb") ?? string.Empty);
    options.EnableSensitiveDataLogging();
});


builder.Services.AddRazorPages();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddHostedService<GiveFoodApiService>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dataFContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    dataFContext.Database.Migrate();
}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();