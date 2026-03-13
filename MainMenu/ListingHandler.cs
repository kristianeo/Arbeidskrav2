using SecondHandMarket.Database;

namespace SecondHandMarket.MainMenu;

public class ListingHandler
{
    public static void CreateListing(Init db)
    {
        string username = db.GetActiveUser();
        Listings listing = ListingGenerator.GenerateListing(username);
        int userId = db.GetUserId();
        db.AddListingToDb(listing, userId);
    }
    
    //TODO: Add function to show a list of listings belonging to active user 
}