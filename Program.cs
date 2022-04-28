using Foodbank_Project.Data;
using Foodbank_Project.Jobs.Scraping;
using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
    //options.Conventions.AuthorizeAreaFolder("Identity", "/Manage", "AtLeast21");
});

builder.Services.AddDbContext<FoodbankContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Foodbanks")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

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

builder.Services.AddSignalR();

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
app.UseAuthentication(); ;

app.UseAuthorization();

app.MapRazorPages();

app.Run();
