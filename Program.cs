using SecondHandMarket.Database;
using SecondHandMarket.MainMenu;

namespace SecondHandMarket;

class Program
{
    static void Main(string[] args)
    {
        InitSql init = new InitSql();
        ListingCollection listingCollection = new ListingCollection();
        UserCollection userCollection = new UserCollection();

        string username = init.GetActiveUser();
        Listings listing = listingCollection.CreateListing(username);
        int userId = init.GetUserId(userCollection);
        init.AddListingToDb(listing, userId);
        
    }
}