using SecondHandMarket.Database;

namespace SecondHandMarket.MainMenu;

public class ListingHandler
{
    public static void CreateListing(Init db, ListingCollection lc)
    {
        Listings listing = lc.CreateListing(db);
        int userId = db.GetUserId();
        db.AddListingToDb(listing, userId);
    }

    public static void ShowUserListings(Init db, ListingCollection lc)
    {
        string username = db.GetActiveUser();
        List<Listings> newList = lc.GetAll();
        foreach (Listings l in newList)
        {
            if (l.Seller == username)
            {
                Console.WriteLine(l);
            }
        }
    }
}