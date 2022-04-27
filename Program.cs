using Foodbank_Project;
using Foodbank_Project.Jobs;
using Foodbank_Project.Jobs.Scraping;
using Quartz;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeAreaFolder("Identity", "/Manage", "AtLeast21");
});

builder.Services.AddSignalR();

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    var giveFodJoobKey = new JobKey("giveFodJoobKey");
    q.AddJob<GiveFoodAPIFoodBanks>(opts => opts.WithIdentity(giveFodJoobKey));
    q.AddTrigger(opts => opts
        .ForJob(giveFodJoobKey)
        .WithIdentity("giveFoodJob-trigger")
        .WithSimpleSchedule(x => x
            .WithIntervalInHours(3)
            .RepeatForever()));


});

builder.Services.AddQuartzHostedService(
    q => q.WaitForJobsToComplete = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.Run();
