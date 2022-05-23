#region

using Foodbank_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Foodbank_Project.Data;

public class ApplicationContext : IdentityDbContext<IdentityUser>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    // ReSharper disable once UnusedMember.Global
    public DbSet<Foodbank>? Foodbanks { get; set; }

    // ReSharper disable once UnusedMember.Global
    public DbSet<Location>? Locations { get; set; }

    // ReSharper disable once UnusedMember.Global
    public DbSet<Need>? Needs { get; set; }

    public DbSet<Content>? Contents { get; set; }

    public DbSet<Recipe>? Recipes { get; set; }

    public DbSet<RecipeCategory>? RecipeCategories { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Foodbank>(f => f.HasKey(pl => pl.FoodbankId));
        modelBuilder.Entity<Location>(f => f.HasKey(pl => pl.LocationId));
        modelBuilder.Entity<Need>(f => f.HasKey(pl => pl.NeedId));
        modelBuilder.Entity<Content>(f => f.HasKey(pl => pl.ContentId));
        modelBuilder.Entity<Recipe>(f => f.HasKey(pl => pl.RecipeId));
        modelBuilder.Entity<RecipeCategory>(f => f.HasKey(pl => pl.RecipeCategoryId));

        modelBuilder.Entity<Foodbank>().Property(f => f.FoodbankId).ValueGeneratedOnAdd();
        modelBuilder.Entity<Location>().Property(f => f.LocationId).ValueGeneratedOnAdd();
        modelBuilder.Entity<Need>().Property(f => f.NeedId).ValueGeneratedOnAdd();
        modelBuilder.Entity<Recipe>().Property(f => f.RecipeId).ValueGeneratedOnAdd();
        modelBuilder.Entity<Content>().Property(f => f.ContentId).ValueGeneratedOnAdd();
        modelBuilder.Entity<RecipeCategory>().Property(f => f.RecipeCategoryId).ValueGeneratedOnAdd();

        modelBuilder.Entity<Location>()
            .HasOne(l => l.Foodbank)
            .WithMany(f => f.Locations)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Recipe>()
            .HasMany(l => l.Category)
            .WithMany(f => f.Recipes);


        modelBuilder.Entity<Foodbank>().Property(f => f.Name).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Slug).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Email).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Address).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Postcode).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Closed).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Country).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Coord).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Network).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Created).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Protected).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.FoodbankId).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Status).IsRequired();


        modelBuilder.Entity<Location>().Property(fl => fl.Slug).IsRequired();
        modelBuilder.Entity<Location>().Property(fl => fl.Name).IsRequired();
        modelBuilder.Entity<Location>().Property(fl => fl.Coord).IsRequired();
        modelBuilder.Entity<Location>().Property(fl => fl.Address).IsRequired();
        modelBuilder.Entity<Location>().Property(fl => fl.Postcode).IsRequired();
        modelBuilder.Entity<Location>().Property(fl => fl.LocationId).IsRequired();

        modelBuilder.Entity<Need>().Property(n => n.NeedId).IsRequired();

        modelBuilder.Entity<Recipe>().Property(fl => fl.Method).IsRequired();
        modelBuilder.Entity<Recipe>().Property(fl => fl.Ingredients).IsRequired();
        modelBuilder.Entity<Recipe>().Property(fl => fl.Name).IsRequired();
        modelBuilder.Entity<Recipe>().Property(fl => fl.Status).IsRequired();
        modelBuilder.Entity<Recipe>().Property(fl => fl.Serves);
        modelBuilder.Entity<Recipe>().Property(fl => fl.Notes);

        modelBuilder.Entity<Content>().Property(fl => fl.Blob).IsRequired();
        modelBuilder.Entity<Content>().Property(fl => fl.Name).IsRequired();

        modelBuilder.Entity<RecipeCategory>().Property(fl => fl.Name).IsRequired();

        // modelBuilder.Entity<RecipeCategory>().HasData(new RecipeCategory
        // {
        //     RecipeCategoryId = -1,
        //     Name = "Meat"
        // }, new RecipeCategory
        // {
        //     RecipeCategoryId = -2,
        //     Name = "Vegetarian"
        // }, new RecipeCategory
        // {
        //     RecipeCategoryId = -3,
        //     Name = "Fish"
        // }, new RecipeCategory
        // {
        //     RecipeCategoryId = -4,
        //     Name = "Desert"
        // }, new RecipeCategory
        // {
        //     RecipeCategoryId = -5,
        //     Name = "Soup"
        // }, new RecipeCategory
        // {
        //     RecipeCategoryId = -6,
        //     Name = "Snack"
        // }, new RecipeCategory
        // {
        //     RecipeCategoryId = -7,
        //     Name = "Side"
        // }, new RecipeCategory
        // {
        //     RecipeCategoryId = -8,
        //     Name = "Special Event"
        // });


        base.OnModelCreating(modelBuilder);
    }
}