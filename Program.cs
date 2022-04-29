#region

using Foodbank_Project.Data;
using Foodbank_Project.Jobs.Scraping;
using Microsoft.EntityFrameworkCore;
using Quartz;

#endregion

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<FoodbankContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Foodbanks") ?? string.Empty));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    var giveFoodJobKey = new JobKey("giveFoodJobKey");
    q.AddJob<GiveFoodApiFoodBanks>(opts => opts.WithIdentity(giveFoodJobKey));
    q.AddTrigger(opts => opts
        .ForJob(giveFoodJobKey)
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