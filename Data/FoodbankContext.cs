#region

using Foodbank_Project.Models;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Foodbank_Project.Data;

public sealed class FoodbankContext : DbContext
{
    public FoodbankContext(DbContextOptions<FoodbankContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    // ReSharper disable once UnusedMember.Global
    public DbSet<Foodbank>? Foodbanks { get; set; }

    // ReSharper disable once UnusedMember.Global
    public DbSet<Location>? Locations { get; set; }

    // ReSharper disable once UnusedMember.Global
    public DbSet<Need>? Needs { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Foodbank>(f => f.HasKey(pl => pl.FoodbankId));
        modelBuilder.Entity<Location>(f => f.HasKey(pl => pl.LocationId));
        modelBuilder.Entity<Need>(f => f.HasKey(pl => pl.NeedId));

        modelBuilder.Entity<Foodbank>().Property(f => f.FoodbankId).ValueGeneratedOnAdd();
        modelBuilder.Entity<Location>().Property(f => f.LocationId).ValueGeneratedOnAdd();
        modelBuilder.Entity<Need>().Property(f => f.NeedId).ValueGeneratedOnAdd();

        modelBuilder.Entity<Location>()
            .HasOne(l => l.Foodbank)
            .WithMany(f => f.Locations);

        modelBuilder.Entity<Foodbank>().Property(f => f.Name).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Slug).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Email).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Address).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Postcode).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Closed).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Country).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.LatLng).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Network).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Created).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.Protected).IsRequired();
        modelBuilder.Entity<Foodbank>().Property(f => f.FoodbankId).IsRequired();


        modelBuilder.Entity<Location>().Property(fl => fl.Slug).IsRequired();
        modelBuilder.Entity<Location>().Property(fl => fl.Name).IsRequired();
        modelBuilder.Entity<Location>().Property(fl => fl.LatLng).IsRequired();
        modelBuilder.Entity<Location>().Property(fl => fl.Address).IsRequired();
        modelBuilder.Entity<Location>().Property(fl => fl.Postcode).IsRequired();
        modelBuilder.Entity<Location>().Property(fl => fl.LocationId).IsRequired();


        modelBuilder.Entity<Need>().Property(n => n.NeedId).IsRequired();
    }
}