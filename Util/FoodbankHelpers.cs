using System.Collections;
using System.Collections.ObjectModel;
using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodbank_Project.Util;

public static class FoodbankHelpers
{
    public static Foodbank? Convert(Models.External.Foodbank externalFoodbank)
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
            LatLng = externalFoodbank.LatLng,
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
                LatLng = item.LatLng,
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
        {
            foodbank.Needs.Add(need == "Unknown" ? new Need { NeedStr = null } : new Need { NeedStr = need });
        }

        return foodbank;
    }

    public static Foodbank? InsertOrUpdate(Foodbank target, FoodbankContext ctx, CancellationToken cancellationToken)
    {
        /*
                MergeProperties(target, from, "FoodbankId");
        
                // merge locations 
                if (from.Locations != null)
                {
                    foreach (var fromLocation in from.Locations)
                    {
                        Location? found = null;
                        if (target.Locations is not null)
                        {
                            foreach (var targetLocation in target.Locations)
                            {
                                if (targetLocation.Slug == fromLocation.Slug)
                                {
                                    found = targetLocation;
                                }
                            }
        
                            if (found != null)
                            {
                                MergeProperties(found, fromLocation, "LocationId");
                            }
                            else
                            {
                                target.Locations.Remove(fromLocation);
                            }
                        }
                    }
                }
        
                // clear all target needs
                target.FoodbankNeeds?.Clear();
        
                if (from.FoodbankNeeds != null)
                    foreach (var fromFoodbankNeed in from.FoodbankNeeds)
                    {
                        var need = NeedsHelper.GetNeed(fromFoodbankNeed.Need?.NeedStr, ctx);
        
                        if (need is null)
                        {
                            target.FoodbankNeeds?.Add(fromFoodbankNeed);
                        }
                        else
                        {
                            fromFoodbankNeed.Need = need;
                            target.FoodbankNeeds?.Add(fromFoodbankNeed);
                        }
                        
                    }
                */
        
        ctx.ChangeTracker.Clear(); // recreating context is a pain, clearing is easier since we are scope

        var dbFoodbank = ctx.Foodbanks!.Include(f => f.Locations).
            Include(f => f.Needs).FirstOrDefault((f) => f.Slug == target.Slug);

        if (dbFoodbank is not null && dbFoodbank.Protected)
        {
            return default;
        }


        if (dbFoodbank is null)
        {
            target.Needs = CompletePartialNeeds(target.Needs, ctx);
            target.Locations = CompletePartialLocations(target.Locations, ctx);

            target.FoodbankId = null;

            ctx.Foodbanks!.Update(target);

            ctx.SaveChanges();
            return target;
        }
        else
        {
            target.FoodbankId = dbFoodbank.FoodbankId;
            
            target.Needs = CompletePartialNeeds(target.Needs, ctx);
            target.Locations = CompletePartialLocations(target.Locations, ctx);
            
            ctx.Entry(dbFoodbank).CurrentValues.SetValues(target);

            ctx.SaveChanges();
            return dbFoodbank;
        }
    }
    
    private static ICollection<Need> CompletePartialNeeds(ICollection<Need> needs, FoodbankContext ctx)
    {
        ICollection<Need> completeNeeds = new Collection<Need>();
        foreach (var need in needs.ToArray())
        {
            var dbNeed = ctx.Needs!.FirstOrDefault(n => n.NeedStr == need.NeedStr);
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
        ctx.SaveChanges();
        return completeNeeds;
    }
    
    private static ICollection<Location> CompletePartialLocations(ICollection<Location> locations, FoodbankContext ctx)
    {
        ICollection<Location> completeLocations = new Collection<Location>();
        foreach (var location in locations.ToArray())
        {
            var dbLocation = ctx.Locations!.FirstOrDefault(l => l.Slug == location.Slug);
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
        ctx.SaveChanges();
        return completeLocations;
    } 
}