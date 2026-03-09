namespace SecondHandMarket;

public class Listings
{
    private enum Status
    {
        Available,
        Sold
    }

    private Status _status;

    private Dictionary<string, string> _condition = new()
    {
        { "New", "Unused, still in original packaging" },
        { "Like New", "Used briefly, no visible wear" },
        { "Good", "Some signs of use, fully functional" },
        { "Fair", "Noticeable wear, but still works" }
    };
}