namespace SecondHandMarket;

public class Listings
{
    private Users _seller;
    private string _title;
    private string _description;
    private string _condition;
    private Categories _category;
    private decimal _price;
    private Status _status;

    private enum Status //TODO: Can be made an interface?
    {
        Available,
        Sold
    }

    public enum Categories
    {
        BooksAndMedia,
        ClothingAndAccessories,
        Electronics,
        FurnitureAndHome,
        SportsAndOutdoors,
        Other
    }

    public static Dictionary<string, string> _conditions = new()
    {
        { "New: ", "Unused, still in original packaging" },
        { "Like New: ", "Used briefly, no visible wear" },
        { "Good: ", "Some signs of use, fully functional" },
        { "Fair: ", "Noticeable wear, but still works" }
    };

    public string Title
    {
        get => _title;
        set => _title = value;
    }

    public string Description
    {
        get => _description;
        set => _description = value;
    }

    public decimal Price
    {
        get => _price;
        set => _price = value;
    }

    public Listings(Users seller, string title, string description, Categories category, string condition, decimal price)
    {
        _seller = seller;
        _title = title;
        _description = description;
        _category = category;
        _condition = condition;
        _price = price;
        _status = Status.Available;
    }

    public override string ToString()
    {
        return $"{_title} - {_description} - {_category} - {_condition} -  {_price}";
    }
}