namespace SecondHandMarket;

public class Listings
{
    private string _title;
    private string _description;
    private string _condition;
    private Categories _category;
    private decimal _price;
    private Status _status;

    private enum Status
    {
        Available,
        Sold
    }

    private enum Categories
    {
        BooksAndMedia,
        ClothingAndAccessories,
        Electronics,
        FurnitureAndHome,
        SportsAndOutdoors,
        Other
    }

    private Dictionary<string, string> _conditions = new()
    {
        { "New", "Unused, still in original packaging" },
        { "Like New", "Used briefly, no visible wear" },
        { "Good", "Some signs of use, fully functional" },
        { "Fair", "Noticeable wear, but still works" }
    };

    public string Title => _title;
    public string Description => _description;
    public decimal Price => _price;

    private Listings(string title, string description, Categories category, string condition, decimal price)
    {
        _title = title;
        _description = description;
        _category = category;
        _condition = condition;
        _price = price;
        _status = Status.Available;
    }
    
  
}