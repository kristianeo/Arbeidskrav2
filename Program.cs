using System.Data.SQLite;
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

        while (true)
        {
            Login.UserLogout(db);
            Login.UserLogin(db);

            int listingId = listingCollection.BuyListing(db);
            userCollection.LeaveReview(db, listingId);
        }
    }
}