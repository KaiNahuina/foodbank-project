namespace Foodbank_Project.Models;

public class Need
{
    // ReSharper disable once UnusedMember.Global
    public int NeedId { get; set; }

    public string? NeedStr { get; set; }

    public ICollection<FoodbankNeed>? FoodbankNeeds { get; set; }
}