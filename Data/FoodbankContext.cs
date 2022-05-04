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
        this.Database.EnsureCreated();
    }

    // ReSharper disable once UnusedMember.Global
    public DbSet<Foodbank>? Foodbanks { get; set; }

    // ReSharper disable once UnusedMember.Global
    public DbSet<Location>? Locations { get; set; }

    // ReSharper disable once UnusedMember.Global
    public DbSet<FoodbankNeed>? FoodbankNeeds { get; set; }

    // ReSharper disable once UnusedMember.Global
    public DbSet<Need>? Needs { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Foodbank>(f => f.HasKey(pl => pl.FoodbankId));
        modelBuilder.Entity<Location>(f => f.HasKey(pl => pl.LocationId));
        modelBuilder.Entity<Need>(f => f.HasKey(pl => pl.NeedId));
        
        
        modelBuilder.Entity<Location>()
            .HasOne(fl => fl.Foodbank)
            .WithMany(f => f.Locations)
            .HasForeignKey(fl => fl.FoodbankId);

        modelBuilder.Entity<FoodbankNeed>().HasKey(fn => new { fn.FoodbankId, fn.NeedId });

        modelBuilder.Entity<FoodbankNeed>()
            .HasOne(fn => fn.Foodbank)
            .WithMany(f => f.FoodbankNeeds)
            .HasForeignKey(fn => fn.FoodbankId);

        modelBuilder.Entity<FoodbankNeed>()
            .HasOne(fn => fn.Need)
            .WithMany(n => n.FoodbankNeeds)
            .HasForeignKey(n => n.NeedId);


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


        modelBuilder.Entity<Location>().Property(fl => fl.Slug).IsRequired();
        modelBuilder.Entity<Location>().Property(fl => fl.Name).IsRequired();
        modelBuilder.Entity<Location>().Property(fl => fl.LatLng).IsRequired();
        modelBuilder.Entity<Location>().Property(fl => fl.Address).IsRequired();
        modelBuilder.Entity<Location>().Property(fl => fl.Postcode).IsRequired();
    }
}