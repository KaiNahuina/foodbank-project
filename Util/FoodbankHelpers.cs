﻿#region

using System.Collections.ObjectModel;
using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Foodbank_Project.Util;

public static class FoodbankHelpers
{
    public static Foodbank Convert(Models.External.Foodbank externalFoodbank)
    {
        var foodbank = new Foodbank
        {
            Name = externalFoodbank.Name,
            AltName = externalFoodbank.AltName,
            Slug = externalFoodbank.Slug,
            Phone = externalFoodbank.Phone,
            SecondaryPhone = externalFoodbank.SecondaryPhone,
            Email = externalFoodbank.Email,
            Address = externalFoodbank.Address,
            Postcode = externalFoodbank.Postcode,
            Closed = externalFoodbank.Closed,
            Country = externalFoodbank.Country,
            Coord = new NetTopologySuite.Geometries.Point(double.Parse(externalFoodbank.LatLng?.Split(",")[1]), double.Parse(externalFoodbank.LatLng?.Split(",")[0])) { SRID = 4326 },
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
                Coord = new NetTopologySuite.Geometries.Point(double.Parse(item.LatLng?.Split(",")[1]), double.Parse(item.LatLng?.Split(",")[0])) { SRID = 4326 },
                Name = item.Name,
                Slug = item.Slug,
                Postcode = item.Postcode,
                Phone = item.Phone,
                Foodbank = foodbank
            };

            foodbank.Locations.Add(location);
        }

        foodbank.Needs = new List<Need>();
        var needs = externalFoodbank.Needs?.NeedsStr?.Split("\r\n") ?? Array.Empty<string>();
        foreach (var need in needs)
            foodbank.Needs.Add(need == "Unknown" ? new Need { NeedStr = null } : new Need { NeedStr = need });

        return foodbank;
    }

    public static async Task InsertOrUpdate(Foodbank target, ApplicationContext ctx, CancellationToken cancellationToken)
    {
        

        var dbFoodbank = await ctx.Foodbanks!.FirstOrDefaultAsync(f => f.Slug == target.Slug, cancellationToken);

        if (dbFoodbank is not null && dbFoodbank.Protected) return;


        if (dbFoodbank is null)
        {
            target.Needs = await CompletePartialNeeds(target.Needs, ctx, cancellationToken);
            target.Locations = await CompletePartialLocations(target.Locations, ctx, cancellationToken);

            target.FoodbankId = null;

            ctx.Foodbanks!.Update(target);

            await ctx.SaveChangesAsync(cancellationToken);
            return;
        }

        target.FoodbankId = dbFoodbank.FoodbankId;

        target.Needs = await CompletePartialNeeds(target.Needs, ctx, cancellationToken);
        target.Locations = await CompletePartialLocations(target.Locations, ctx, cancellationToken);

        ctx.Entry(dbFoodbank).CurrentValues.SetValues(target);

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
            var dbLocation = await ctx.Locations!.FirstOrDefaultAsync(l => l.Slug == location.Slug,
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