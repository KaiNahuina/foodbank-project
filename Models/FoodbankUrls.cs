using Newtonsoft.Json;

namespace Foodbank_Project.Models
{
    public class FoodbankUrls
    {
        // Field not needed, Change to dispose on DB store
        [JsonProperty("self")]
        public string? Self;

        // Field not needed, Change to dispose on DB store
        [JsonProperty("html")]
        public string? Html;

        // Field not needed, Change to dispose on DB store
        [JsonProperty("map")]
        public string? Map;

        [JsonProperty("homepage")]
        public string? Homepage;

        // Do we need this?
        [JsonProperty("shopping_list")]
        public string? ShoppingList;
    }
}
