namespace Foodbank_Project.Models.Foodbank.Internal;

public class CharityInfo
{
    // ReSharper disable once UnassignedField.Global
    public Foodbank? Foodbank;

    // ReSharper disable once UnusedMember.Global
    public int CharityInfoId { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? CharityNumber { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? CharityRegisterUrl { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int FoodbankId { get; set; }
}