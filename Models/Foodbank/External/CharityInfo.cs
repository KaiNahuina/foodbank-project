using Newtonsoft.Json;

namespace Foodbank_Project.Models.Foodbank.External
{
    public class CharityInfo
    {
        [JsonProperty("registration_id")]
        public string? RegistrationId { get; set; }

        [JsonProperty("register_url")]
        public string? RegisterUrl { get; set; }
    }
}
