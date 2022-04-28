namespace Foodbank_Project.Models.Foodbank
{
    public class FoodbankConverter
    {
        public static Internal.Foodbank Converter(External.Foodbank externalFoodbank)
        {
            Internal.Foodbank foodbank = new Internal.Foodbank();

            foodbank.Name = externalFoodbank.Name;
            foodbank.AltName = externalFoodbank.AltName;
            foodbank.Slug = externalFoodbank.Slug;
            foodbank.Phone = externalFoodbank.Phone;
            foodbank.SecondaryPhone = externalFoodbank.SecondaryPhone;
            foodbank.Email = externalFoodbank.Email;
            foodbank.Address = externalFoodbank.Address;
            foodbank.Postcode = externalFoodbank.Postcode;
            foodbank.Closed = externalFoodbank.Closed;
            foodbank.Country = externalFoodbank.Country;
            foodbank.LatLng = externalFoodbank.LatLng;
            foodbank.Network = externalFoodbank.Network;
            foodbank.Created = externalFoodbank.Created;

            foodbank.Urls = new Internal.Urls();
            foodbank.Urls.Homepage = externalFoodbank.Urls?.Homepage;
            foodbank.Urls.ShoppingList = externalFoodbank.Urls?.ShoppingList;

            foodbank.Charity = new Internal.CharityInfo();
            foodbank.Charity.CharityNumber = externalFoodbank.Charity?.RegistrationId;
            foodbank.Charity.CharityRegistarUrl = externalFoodbank.Charity?.RegisterUrl;

            foodbank.Locations = new List<Internal.Location>();

            foreach (var item in externalFoodbank.Locations ?? new List<External.Location>(0))
            {
                var location = new Internal.Location();
                location.Address = item.Address;
                location.LatLng = item.LatLng;
                location.Name = item.Name;
                location.Slug = item.Slug;
                location.Postcode = item.Postcode;
                location.Phone = item.Phone;

                location.Foodbank = foodbank;

                foodbank.Locations.Add(location);
            }

            foodbank.FoodbankNeeds = new List<Internal.FoodbankNeed>();
            var needs = externalFoodbank.Needs?.NeedsStr?.Split("\r\n") ?? Array.Empty<string>();
            foreach (var needsStr in needs)
            {
                var internalNeed = new Internal.Need();
                internalNeed.NeedStr = needsStr;
                
                var internalNeeds = new Internal.FoodbankNeed();
                internalNeeds.Need = internalNeed;
                internalNeeds.Foodbank = foodbank;

                internalNeed.FoodbankNeeds = new List<Internal.FoodbankNeed>();
                internalNeed.FoodbankNeeds.Add(internalNeeds);
                foodbank.FoodbankNeeds.Add(internalNeeds);
            } 

            return foodbank;
        }
    }
}
