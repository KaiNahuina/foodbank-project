using Newtonsoft.Json;

namespace Foodbank_Project.Models.Foodbank.Internal
{
    public class Foodbank
    {
        public int FoodbankId { get; set; }

        public string? Name { get; set; }
        public string? AltName { get; set; }
        public string? Notes { get; set; }
        public bool Protected { get; set; } = false;
        public string? Slug { get; set; }
        public string? Phone { get; set; }
        public string? SecondaryPhone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Postcode { get; set; }
        public bool? Closed { get; set; }
        public string? Country { get; set; }
        public string? LatLng { get; set; }
        public string? Network { get; set; }
        public DateTime? Created { get; set; }
        public Urls? Urls { get; set; }
        public Provider Provider { get; set; }
        public CharityInfo? Charity { get; set; }
        public ICollection<Location>? Locations;
        public ICollection<FoodbankNeed>? FoodbankNeeds { get; set; }
    }
}
