using Newtonsoft.Json;

namespace Foodbank_Project.Models.Foodbank.Internal
{
    public class CharityInfo
    {
        public int CharityInfoId { get; set; }

        public string? CharityNumber { get; set; }
        public string? CharityRegistarUrl { get; set; }

        public int FoodbankId { get; set; }
        public Foodbank? Foodbank;
    }
}
