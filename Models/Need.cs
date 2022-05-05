namespace Foodbank_Project.Models;

public class Need
{
    // ReSharper disable once UnusedMember.Global
    public int NeedId { get; set; }

    public string? NeedStr { get; set; }

    public List<Foodbank> Foodbanks { get; set; } = new List<Foodbank>();
}