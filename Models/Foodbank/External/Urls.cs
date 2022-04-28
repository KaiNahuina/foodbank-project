#region

using Newtonsoft.Json;

#endregion

namespace Foodbank_Project.Models.Foodbank.External;

public class Urls
{
    [JsonProperty("self")] public string? Self { get; set; }

    [JsonProperty("html")] public string? Html { get; set; }

    [JsonProperty("map")] public string? Map { get; set; }

    [JsonProperty("homepage")] public string? Homepage { get; set; }

    [JsonProperty("shopping_list")] public string? ShoppingList { get; set; }
}