namespace Foodbank_Project.Models;

public class Need
{
    // ReSharper disable once UnusedMember.Global
    public int NeedId { get; set; }

    public string? NeedStr { get; init; }

    public List<Foodbank> Foodbanks { get; set; } = new();
}