using Newtonsoft.Json;

namespace Foodbank_Project.Models
{
    public class FoodbankCharityInfo
    {
        [JsonProperty("registration_id")]
        public string? RegistrationId;

        [JsonProperty("register_url")]
        public string? RegisterUrl;
    }
}
