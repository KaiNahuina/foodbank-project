#region

using Newtonsoft.Json;

#endregion

namespace Foodbank_Project.Models.External;

public class Location
{
    [JsonProperty("name")] public string? Name { get; set; }

    [JsonProperty("slug")] public string? Slug { get; set; }

    [JsonProperty("address")] public string? Address { get; set; }

    [JsonProperty("postcode")] public string? Postcode { get; set; }

    [JsonProperty("lat_lng")] public string? LatLng { get; set; }

    [JsonProperty("phone")] public string? Phone { get; set; }

    [JsonProperty("politics")] public object? Politics { get; set; }
}