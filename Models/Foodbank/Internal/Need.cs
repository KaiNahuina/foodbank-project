namespace Foodbank_Project.Models.Foodbank.Internal
{
    public class Need
    {
        public int NeedId { get; set; }

        public string? NeedStr { get; set; }

        public ICollection<FoodbankNeed>? FoodbankNeeds { get; set; }
    }
}
