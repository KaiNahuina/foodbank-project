using Newtonsoft.Json;

namespace Foodbank_Project.Models
{
    public class FoodbankLocation
    {
        [JsonProperty("name")]
        public string? Name;

        [JsonProperty("slug")]
        public string? Slug;

        [JsonProperty("address")]
        public string? Address;

        [JsonProperty("postcode")]
        public string? Postcode;

        [JsonProperty("lat_lng")]
        public string? LatLng;

        [JsonProperty("phone")]
        public string? Phone;

        // Field not needed, Change to dispose on DB store
        [JsonProperty("politics")]
        public object? Politics;
    }
}
