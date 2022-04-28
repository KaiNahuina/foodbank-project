namespace Foodbank_Project.Models.Foodbank.Internal;

public class FoodbankNeed
{
    // ReSharper disable once UnusedMember.Global
    public DateTime? Created { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int NeedId { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public Need? Need { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int FoodbankId { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public Foodbank? Foodbank { get; set; }
}