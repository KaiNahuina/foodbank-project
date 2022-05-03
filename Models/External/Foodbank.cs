#region

using Newtonsoft.Json;

#endregion

namespace Foodbank_Project.Models.External;

public class Foodbank
{
    [JsonProperty("locations")] public ICollection<Location>? Locations;

    [JsonProperty("nearby_foodbanks")] public List<object>? NearbyFoodbanks;

    [JsonProperty("name")] public string? Name { get; set; }

    [JsonProperty("alt_name")] public string? AltName { get; set; }

    [JsonProperty("slug")] public string? Slug { get; set; }

    [JsonProperty("phone")] public string? Phone { get; set; }

    [JsonProperty("secondary_phone")] public string? SecondaryPhone { get; set; }

    [JsonProperty("email")] public string? Email { get; set; }

    [JsonProperty("address")] public string? Address { get; set; }

    [JsonProperty("postcode")] public string? Postcode { get; set; }

    [JsonProperty("closed")] public bool? Closed { get; set; }

    [JsonProperty("country")] public string? Country { get; set; }

    [JsonProperty("lat_lng")] public string? LatLng { get; set; }

    [JsonProperty("network")] public string? Network { get; set; }

    [JsonProperty("created")] public DateTime? Created { get; set; }

    [JsonProperty("urls")] public Urls? Urls { get; set; }

    [JsonProperty("charity")] public CharityInfo? Charity { get; set; }

    [JsonProperty("need")] public Needs? Needs { get; set; }


    [JsonProperty("politics")] public object? Politics { get; set; }

    public void Merge(Foodbank giver)
    {
        Name ??= giver.Name;
        AltName ??= giver.AltName;
        Slug ??= giver.Slug;
        Phone ??= giver.Phone;
        SecondaryPhone ??= giver.SecondaryPhone;
        Email ??= giver.Email;
        Address ??= giver.Address;
        Postcode ??= giver.Postcode;
        Closed ??= giver.Closed;
        Country ??= giver.Country;
        LatLng ??= giver.LatLng;
        Network ??= giver.Network;
        Created ??= giver.Created;
        Urls ??= giver.Urls;
        Charity ??= giver.Charity;
        Locations ??= giver.Locations;
        Needs ??= giver.Needs;
        Politics ??= giver.Politics;
        NearbyFoodbanks ??= giver.NearbyFoodbanks;
    }
}