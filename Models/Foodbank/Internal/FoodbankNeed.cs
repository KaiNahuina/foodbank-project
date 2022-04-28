using Newtonsoft.Json;

namespace Foodbank_Project.Models.Foodbank.Internal
{
    public class FoodbankNeed
    {
        public DateTime? Created { get; set; }

        public int NeedId { get; set; }
        public Need? Need { get; set; }

        public int FoodbankId { get; set; }
        public Foodbank? Foodbank { get; set; }
    }
}
