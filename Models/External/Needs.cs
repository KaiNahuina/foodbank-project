#region

using Newtonsoft.Json;

#endregion

namespace Foodbank_Project.Models.External;

public class Needs
{
    [JsonProperty("id")] // Id from API is commit SHA
    public object? Id { get; set; }

    [JsonProperty("needs")] public string? NeedsStr { get; set; }

    [JsonProperty("created")] public DateTime? Created { get; set; }

    [JsonProperty("self")] public string? Self { get; set; }
}