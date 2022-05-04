using System.Collections;
using Foodbank_Project.Data;
using Foodbank_Project.Models;

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

        foodbank.FoodbankNeeds = new List<FoodbankNeed>();
        var needs = externalFoodbank.Needs?.NeedsStr?.Split("\r\n") ?? Array.Empty<string>();
        foreach (var needsStr in needs)
        {
            var internalNeed = new Need
            {
                NeedStr = needsStr
            };
            if (internalNeed.NeedStr == "Unknown") internalNeed.NeedStr = null;

            var internalNeeds = new FoodbankNeed
            {
                Need = internalNeed,
                Foodbank = foodbank
            };

            internalNeed.FoodbankNeeds = new List<FoodbankNeed>();
            internalNeed.FoodbankNeeds.Add(internalNeeds);
            foodbank.FoodbankNeeds.Add(internalNeeds);
        }

        return foodbank;
    }

    // minor case of insanity here
    // Uses Reflection.Emit :(
    public static void MergeToEntity(Foodbank target, Foodbank from, FoodbankContext ctx)
    {
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

        foreach (var fneed in from.FoodbankNeeds)
        {
            foreach (var tneed in target.FoodbankNeeds)
            {
                
            }
        }
    }
    
    private static void MergeProperties<T>(T target, T from, string exclude)
    {
        Type t = typeof(Foodbank);

        var properties = t.GetProperties().Where(prop =>
            prop.CanRead && prop.CanWrite && prop.PropertyType != typeof(ICollection));

        foreach (var prop in properties)
        {
            var value = prop.GetValue(from, null);
            var value2 = prop.GetValue(target, null);
            if (prop.Name == exclude && value != value2)
            {
                prop.SetValue(target, value, null);
            }
        }
    }
}