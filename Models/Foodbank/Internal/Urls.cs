namespace Foodbank_Project.Models.Foodbank.Internal;

public class Urls
{
    // ReSharper disable once UnusedMember.Global
    public int UrlsId { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string? Homepage { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public string? ShoppingList { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int FoodbankId { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public Foodbank? Foodbank { get; set; }
}