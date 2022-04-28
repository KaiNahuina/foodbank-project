using Newtonsoft.Json;

namespace Foodbank_Project.Models.Foodbank.Internal
{
    public class Urls
    {
        public int UrlsId { get; set; }

        public string? Homepage { get; set; }
        public string? ShoppingList { get; set; }

        public int FoodbankId { get; set; }
        public Foodbank? Foodbank { get; set; }

    }
}
