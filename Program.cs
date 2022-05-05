#region

using Foodbank_Project.Data;
using Foodbank_Project.Services.Scraping;
using Microsoft.EntityFrameworkCore;

#endregion

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<FoodbankContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Foodbanks") ?? string.Empty);
    options.EnableSensitiveDataLogging();
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddHostedService<GiveFoodApiService>();

var app = builder.Build();

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