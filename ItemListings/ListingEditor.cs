namespace SecondHandMarket;

public class ListingEditor
{
    // TODO: Add IActiveUser or smth and if IActiveuser == listings.seller .........
    public static Listings EditTitle(Listings listings)
    {
        listings.Title = ValidEntryChecker.GetValidString(1, 20);
        return listings;
    }

    public static Listings EditPrice(Listings listings)
    {
        listings.Price = ValidEntryChecker.GetValidInt(1, 100000);
        return listings;
    }

    public static Listings EditDescription(Listings listings)
    {
        listings.Description = ValidEntryChecker.GetValidString(0, 200);
        return listings;
    }
}