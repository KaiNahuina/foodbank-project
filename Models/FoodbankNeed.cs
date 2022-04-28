using System.Text.Json.Serialization;

namespace Foodbank_Project.Models
{
    public class FoodbankNeed
    {
        [JsonPropertyName("id")]
        public string? Id;

        [JsonPropertyName("needs")]
        public string? Needs;

        [JsonPropertyName("created")]
        public DateTime Created;

        [JsonPropertyName("self")]
        public string? Self;
    }
}