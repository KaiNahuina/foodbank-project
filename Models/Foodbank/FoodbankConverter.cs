#region

using Foodbank_Project.Models.Foodbank.Internal;

#endregion

namespace Foodbank_Project.Models.Foodbank;

public static class FoodbankConverter
{
    public static Internal.Foodbank Convert(External.Foodbank externalFoodbank)
    {
        var foodbank = new Internal.Foodbank
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
            Urls = new Urls
            {
                Homepage = externalFoodbank.Urls?.Homepage,
                ShoppingList = externalFoodbank.Urls?.ShoppingList
            },
            Charity = new CharityInfo
            {
                CharityNumber = externalFoodbank.Charity?.RegistrationId,
                CharityRegisterUrl = externalFoodbank.Charity?.RegisterUrl
            },
            Locations = new List<Location>()
        };

        foreach (var item in externalFoodbank.Locations ?? new List<External.Location>(0))
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
}