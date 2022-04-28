using Newtonsoft.Json;

namespace Foodbank_Project.Models.Foodbank.Internal
{
    public class Location
    {
        public int LocationId { get; set; }

        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Address { get; set; }
        public string? Postcode { get; set; }
        public string? LatLng { get; set; }
        public string? Phone { get; set; }

        public int FoodbankId { get; set; }
        public Foodbank? Foodbank { get; set; }
    }
}
