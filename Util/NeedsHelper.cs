using Foodbank_Project.Data;
using Foodbank_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodbank_Project.Util;

public class NeedsHelper
{
    public static Need? GetNeed(string needStr, FoodbankContext ctx)
    {
        Need? need = ctx.Needs!.FirstOrDefault((f) => f.NeedStr == needStr);

        return need;
    }
}