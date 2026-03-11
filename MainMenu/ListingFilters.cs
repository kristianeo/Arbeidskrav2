namespace SecondHandMarket.MainMenu;

public delegate bool ListingFilter(Listings listing);

public abstract class ListingFilters
{
    public static ListingFilter CategoryFilter(Listings.Categories category)
    {
        return listing => listing.Category == category;
    }

    public static ListingFilter Search()
    {
        string userSearch = ValidEntryChecker.GetValidString(1, 200);
        return listing => listing.Title.Contains(userSearch);
    }
    
}