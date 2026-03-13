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
        
        // db.CreateDatabase();
        // db.CreateUserTable();
        // db.CreateListingsTable();
        
        // UserCreator.CreateUser(userCollection, db);
        // Login.UserLogout(db);
        // UserCreator.CreateUser(userCollection, db);
        // Login.UserLogout(db);
        
        Login.UserLogin(db);

        ListingHandler.CreateListing(db);

    }
}