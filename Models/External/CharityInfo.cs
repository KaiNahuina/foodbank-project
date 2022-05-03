#region

using Newtonsoft.Json;

#endregion

namespace Foodbank_Project.Models.External;

public class CharityInfo
{
    [JsonProperty("registration_id")] public string? RegistrationId { get; set; }

    [JsonProperty("register_url")] public string? RegisterUrl { get; set; }
}