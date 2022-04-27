using System.Text.Json.Serialization;

namespace Foodbank_Project.Models
{

    public class Foodbank
    {
        [JsonPropertyName("name")]
        public string? Name;

        [JsonPropertyName("alt_name")]
        public string? AltName;

        [JsonPropertyName("slug")]
        public string? Slug;

        [JsonPropertyName("phone")]
        public string? Phone;

        [JsonPropertyName("secondary_phone")]
        public string? SecondaryPhone;

        [JsonPropertyName("email")]
        public string? Email;

        [JsonPropertyName("address")]
        public string? Address;

        [JsonPropertyName("postcode")]
        public string? Postcode;

        [JsonPropertyName("closed")]
        public bool Closed;

        [JsonPropertyName("country")]
        public string? Country;

        [JsonPropertyName("lat_lng")]
        public string? LatLng;

        [JsonPropertyName("network")]
        public string? Network;

        [JsonPropertyName("created")]
        public DateTime Created;

        [JsonPropertyName("urls")]
        public UrlsObj? Urls;

        [JsonPropertyName("charity")]
        public CharityObj? Charity;

        [JsonPropertyName("politics")]
        public PoliticsObj? Politics;

        public override string ToString()
        {
            return $"Name: {Name}, Alt Name: {AltName}, Slug: {Slug}, Phone: {Phone}, Secondary Phone: {SecondaryPhone}, Email: {Email}" +
                $", Address: {Address}, Postcode: {Postcode}, Closed:{Closed}, Country:{Country}, Network: {Network}";
        }

        public class UrlsObj
        {
            [JsonPropertyName("self")]
            public string? Self;

            [JsonPropertyName("html")]
            public string? Html;

            [JsonPropertyName("homepage")]
            public string? Homepage;

            [JsonPropertyName("shopping_list")]
            public string? ShoppingList;
        }

        public class CharityObj
        {
            [JsonPropertyName("registration_id")]
            public string? RegistrationId;

            [JsonPropertyName("register_url")]
            public string? RegisterUrl;
        }

        public class PoliticsObj
        {
            [JsonPropertyName("parliamentary_constituency")]
            public string? ParliamentaryConstituency;

            [JsonPropertyName("mp")]
            public string? Mp;

            [JsonPropertyName("mp_party")]
            public string? MpParty;

            [JsonPropertyName("mp_parl_id")]
            public int? MpParlId;

            [JsonPropertyName("ward")]
            public string? Ward;

            [JsonPropertyName("district")]
            public string? District;

            [JsonPropertyName("urls")]
            public UrlsObj? Urls;
        }

    }


}
