#region

using System.Collections.ObjectModel;
using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Location = Foodbank_Project.Models.Location;

#endregion

namespace Foodbank_Project.Util;

public static class FoodbankHelpers
{
    public static Foodbank ApplySlug(Foodbank foodbank)
    {
        foodbank.Slug = foodbank.Name?.ToLower().Replace(" ", "-");
        return foodbank;
    }
    public static Location ApplySlug(Location foodbank)
    {
        foodbank.Slug = foodbank.Name?.ToLower().Replace(" ", "-");
        return foodbank;
    }

    public static Foodbank ApplyFinalize(Foodbank foodbank)
    {
        foodbank.Locations ??= new List<Location>();
        foodbank.Needs ??= new List<Need>();

        if (foodbank.Locations.Count == 0)
        {
            foodbank.Locations.Add(new Location
            {
                Name = foodbank.Name,
                Address = foodbank.Address,
                Coord = foodbank.Coord,
                Foodbank = foodbank,
                Phone = foodbank.Phone,
                Postcode = foodbank.Postcode,
                Slug = foodbank.Slug
            });
        }

        return foodbank;
    }

    public static Foodbank Convert(Models.External.Foodbank externalFoodbank)
    {
        var foodbank = new Foodbank
        {
            Name = externalFoodbank.Name,
            AltName = externalFoodbank.AltName,
            Slug = externalFoodbank.Slug,
            Phone = externalFoodbank.Phone?.Replace("%2F", " / ").Replace("or", " / "),
            SecondaryPhone = externalFoodbank.SecondaryPhone,
            Email = externalFoodbank.Email,
            Address = externalFoodbank.Address?.Replace(externalFoodbank.Postcode!, ""),
            Postcode = externalFoodbank.Postcode,
            Closed = externalFoodbank.Closed,
            Country = externalFoodbank.Country,
            Coord = new Point(double.Parse(externalFoodbank.LatLng!.Split(",")[1]),
                    double.Parse(externalFoodbank.LatLng!.Split(",")[0]))
                { SRID = 4326 },
            Network = externalFoodbank.Network,
            Created = externalFoodbank.Created,
            Homepage = externalFoodbank.Urls?.Homepage,
            ShoppingList = externalFoodbank.Urls?.ShoppingList,
            CharityNumber = externalFoodbank.Charity?.RegistrationId,
            CharityRegisterUrl = externalFoodbank.Charity?.RegisterUrl,
            Locations = new List<Location>()
        };

        foreach (var item in externalFoodbank.Locations ?? new List<Models.External.Location>(0))
        {
            var location = new Location
            {
                Address = item.Address,
                Coord = new Point(double.Parse(item.LatLng!.Split(",")[1]), double.Parse(item.LatLng!.Split(",")[0]))
                    { SRID = 4326 },
                Name = item.Name,
                Slug = item.Slug,
                Postcode = item.Postcode,
                Phone = item.Phone?.Replace("%2F", " / ").Replace("or", " / "),
                Foodbank = foodbank
            };

            foodbank.Locations.Add(location);
        }

        if (foodbank.Locations.Count == 0)
        {
            foodbank.Locations.Add(new Location
            {
                Name = foodbank.Name,
                Address = foodbank.Address,
                Coord = foodbank.Coord,
                Foodbank = foodbank,
                Phone = foodbank.Phone,
                Postcode = foodbank.Postcode,
                Slug = foodbank.Slug
            });
        }

        foodbank.Needs = new List<Need>();
        var needs = externalFoodbank.Needs?.NeedsStr?.Split("\r\n") ?? Array.Empty<string>();
        foreach (var need in needs)
        {
            var trimNeed = need.Replace("\u200B", "");
            foodbank.Needs.Add(trimNeed == "Unknown" || string.IsNullOrWhiteSpace(trimNeed)
                ? new Need { NeedStr = null }
                : new Need { NeedStr = trimNeed });
        }


        return foodbank;
    }

    public static async Task InsertOrUpdate(Foodbank target, ApplicationContext ctx,
        CancellationToken cancellationToken)
    {
        var dbFoodbank = await ctx.Foodbanks!.Include(f => f.Locations)
            .Include(f => f.Needs)
            .FirstOrDefaultAsync(f => f.Slug == target.Slug && f.Postcode == target.Postcode && f.Phone == target.Phone,
                cancellationToken);

        if (dbFoodbank is not null && dbFoodbank.Protected) return;


        if (dbFoodbank is null)
        {
            target.Needs = await CompletePartialNeeds(target.Needs!, ctx, cancellationToken);
            target.Locations = await CompletePartialLocations(target.Locations!, ctx, cancellationToken);

            target.FoodbankId = null;

            ctx.Foodbanks!.Update(target);

            await ctx.SaveChangesAsync(cancellationToken);
            return;
        }

        target.FoodbankId = dbFoodbank.FoodbankId;

        target.Needs = await CompletePartialNeeds(target.Needs!, ctx, cancellationToken);
        target.Locations = await CompletePartialLocations(target.Locations!, ctx, cancellationToken);

        ctx.Entry(dbFoodbank).CurrentValues.SetValues(target);

        dbFoodbank.Locations?.Clear();
        dbFoodbank.Needs?.Clear();


        foreach (var location in target.Locations) dbFoodbank.Locations?.Add(location);

        foreach (var need in target.Needs) dbFoodbank.Needs?.Add(need);

        await ctx.SaveChangesAsync(cancellationToken);
    }

    private static async Task<ICollection<Need>> CompletePartialNeeds(IEnumerable<Need> needs, ApplicationContext ctx,
        CancellationToken cancellationToken)
    {
        ICollection<Need> completeNeeds = new Collection<Need>();
        foreach (var need in needs.ToArray())
        {
            var dbNeed = await ctx.Needs!.FirstOrDefaultAsync(n => n.NeedStr == need.NeedStr,
                cancellationToken);
            if (dbNeed is null)
            {
                completeNeeds.Add(need);
            }
            else
            {
                need.NeedId = dbNeed.NeedId;
                need.Foodbanks = dbNeed.Foodbanks;
                ctx.Entry(dbNeed).CurrentValues.SetValues(need);
                completeNeeds.Add(dbNeed);
            }
        }

        await ctx.SaveChangesAsync(cancellationToken);
        return completeNeeds;
    }

    private static async Task<ICollection<Location>> CompletePartialLocations(IEnumerable<Location> locations,
        ApplicationContext ctx, CancellationToken cancellationToken)
    {
        ICollection<Location> completeLocations = new Collection<Location>();
        foreach (var location in locations.ToArray())
        {
            var dbLocation = await ctx.Locations!.FirstOrDefaultAsync(
                l => l.Slug == location.Slug && l.Postcode == location.Postcode && l.Phone == location.Phone,
                cancellationToken);
            if (dbLocation is null)
            {
                completeLocations.Add(location);
            }
            else
            {
                location.LocationId = dbLocation.LocationId;
                location.Foodbank = dbLocation.Foodbank;
                ctx.Entry(dbLocation).CurrentValues.SetValues(location);
                completeLocations.Add(dbLocation);
            }
        }

        await ctx.SaveChangesAsync(cancellationToken);
        return completeLocations;
    }
}