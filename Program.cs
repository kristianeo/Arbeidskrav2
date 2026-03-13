using SecondHandMarket.Database;
using SecondHandMarket.MainMenu;

namespace SecondHandMarket;

class Program
{
    static void Main(string[] args)
    {
        Init db = new Init();
        ListingCollection listingCollection = new ListingCollection();
        UserCollection userCollection = new UserCollection();

        // string username = db.GetActiveUser();
        // Listings listing = listingCollection.CreateListing(username);
        // int userId = db.GetUserId(userCollection);
        // db.AddListingToDb(listing, userId);

    }
}