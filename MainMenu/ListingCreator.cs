using SecondHandMarket.Database;

namespace SecondHandMarket.MainMenu;

public class ListingCreator
{
    public static void CreateListing(Init db)
    {
        string username = db.GetActiveUser();
        Listings listing = ListingGenerator.GenerateListing(username);
        int userId = db.GetUserId();
        db.AddListingToDb(listing, userId);
    }
}