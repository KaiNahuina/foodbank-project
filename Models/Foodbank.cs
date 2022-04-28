using Newtonsoft.Json;

namespace Foodbank_Project.Models
{
    public class Foodbank
    {
        public int ID { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("alt_name")]
        public string? AltName { get; set; }

        // Field not needed, Change to dispose on DB store
        [JsonProperty("slug")]
        public string? Slug { get; set; }

        [JsonProperty("phone")]
        public string? Phone { get; set; }

        [JsonProperty("secondary_phone")]
        public string? SecondaryPhone { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("address")]
        public string? Address { get; set; }

        [JsonProperty("postcode")]
        public string? Postcode { get; set; }

        [JsonProperty("closed")]
        public bool Closed { get; set; }

        [JsonProperty("coutry")]
        public string? Country { get; set; }

        [JsonProperty("lat_lng")]
        public string? LatLng { get; set; }

        [JsonProperty("network")]
        public string? Network { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("urls")]
        public FoodbankUrls? Urls { get; set; }

        [JsonProperty("charity")]
        public FoodbankCharityInfo? Charity { get; set; }

        // Field not needed, Change to dispose on DB store
        [JsonProperty("politcs")]
        public object? Politics { get; set; }

        [JsonProperty("locations")]
        public List<FoodbankLocation>? Locations;

        [JsonProperty("need")]
        public FoodbankNeed? Need;

        // Field not needed, Change to dispose on DB store
        [JsonProperty("nearby_foodbanks")]
        public List<object>? NearbyFoodbanks;

        public void Merge(Foodbank giver)
        {
            this.Name ??= giver.Name;
            this.AltName ??= giver.AltName;
            this.Slug ??= giver.Slug;
            this.Phone ??= giver.Phone;
            this.SecondaryPhone ??= giver.SecondaryPhone;
            this.Email ??= giver.Email;
            this.Address ??= giver.Address;
            this.Postcode ??= giver.Postcode;
            this.Country ??= giver.Country;
            this.LatLng ??= giver.LatLng;
            this.Network ??= giver.Network;
            this.Urls ??= giver.Urls;
            this.Charity ??= giver.Charity;
            this.Locations ??= giver.Locations;
            this.Need ??= giver.Need;
        }
    }
}
