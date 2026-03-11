namespace SecondHandMarket.MainMenu;

public delegate bool ListingFilter(Listings listing);

public class ListingFilters
{
    public static ListingFilter CategoryTicketFilter(Listings.Categories category)
    {
        return (listing) => listing.Category == category;
    }
    
}