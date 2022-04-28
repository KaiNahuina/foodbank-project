using Newtonsoft.Json;

namespace Foodbank_Project.Models.Foodbank.External
{
    public class Foodbank
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("alt_name")]
        public string? AltName { get; set; }

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
        public bool? Closed { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("lat_lng")]
        public string? LatLng { get; set; }

        [JsonProperty("network")]
        public string? Network { get; set; }

        [JsonProperty("created")]
        public DateTime? Created { get; set; }

        [JsonProperty("urls")]
        public Urls? Urls { get; set; }

        [JsonProperty("charity")]
        public CharityInfo? Charity { get; set; }

        [JsonProperty("locations")]
        public ICollection<Location>? Locations;

        [JsonProperty("need")]
        public Needs? Needs { get; set; }


        [JsonProperty("politcs")]
        public object? Politics { get; set; }

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
            this.Closed ??= giver.Closed;
            this.Country ??= giver.Country;
            this.LatLng ??= giver.LatLng;
            this.Network ??= giver.Network;
            this.Created ??= giver.Created;
            this.Urls ??= giver.Urls;
            this.Charity ??= giver.Charity;
            this.Locations ??= giver.Locations;
            this.Needs ??= giver.Needs;
            this.Politics ??= giver.Politics;
            this.NearbyFoodbanks ??= giver.NearbyFoodbanks;
        }
    }
}
