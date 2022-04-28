using Foodbank_Project.Models.Foodbank.Internal;
using Microsoft.EntityFrameworkCore;

namespace Foodbank_Project.Data
{
    public class FoodbankContext : DbContext
    {
        public FoodbankContext(DbContextOptions<FoodbankContext> options)
            : base(options)
        {
        }

        public DbSet<Foodbank>? foodbanks { get; set; }
        public DbSet<CharityInfo>? charityInfos { get; set; }
        public DbSet<Urls>? urls { get; set; }
        public DbSet<Location>? locations { get; set; }
        public DbSet<FoodbankNeed>? foodbankNeeds { get; set; }
        public DbSet<Need>? needs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<Foodbank>()
            .HasOne(f => f.Charity)
            .WithOne(fci => fci.Foodbank)
            .HasForeignKey<CharityInfo>(fci => fci.FoodbankId);

            modelBuilder.Entity<Foodbank>()
            .HasOne(f => f.Urls)
            .WithOne(fu => fu.Foodbank)
            .HasForeignKey<Urls>(fu => fu.FoodbankId);


            modelBuilder.Entity<Foodbank>().Property(f => f.Name).IsRequired();
            modelBuilder.Entity<Foodbank>().Property(f => f.Slug).IsRequired();
            modelBuilder.Entity<Foodbank>().Property(f => f.Phone).IsRequired();
            modelBuilder.Entity<Foodbank>().Property(f => f.Email).IsRequired();
            modelBuilder.Entity<Foodbank>().Property(f => f.Address).IsRequired();
            modelBuilder.Entity<Foodbank>().Property(f => f.Postcode).IsRequired();
            modelBuilder.Entity<Foodbank>().Property(f => f.Closed).IsRequired();
            modelBuilder.Entity<Foodbank>().Property(f => f.Country).IsRequired();
            modelBuilder.Entity<Foodbank>().Property(f => f.LatLng).IsRequired();
            modelBuilder.Entity<Foodbank>().Property(f => f.Network).IsRequired();
            modelBuilder.Entity<Foodbank>().Property(f => f.Created).IsRequired();
            modelBuilder.Entity<Foodbank>().Property(f => f.Protected).IsRequired();

            modelBuilder.Entity<Urls>().Property(fu => fu.Homepage).IsRequired();

            modelBuilder.Entity<Location>().Property(fl => fl.Slug).IsRequired();
            modelBuilder.Entity<Location>().Property(fl => fl.Name).IsRequired();
            modelBuilder.Entity<Location>().Property(fl => fl.Phone).IsRequired();
            modelBuilder.Entity<Location>().Property(fl => fl.LatLng).IsRequired();
            modelBuilder.Entity<Location>().Property(fl => fl.Address).IsRequired();
            modelBuilder.Entity<Location>().Property(fl => fl.Postcode).IsRequired();
        }
    }
}
