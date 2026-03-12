using SecondHandMarket.Database;
using SecondHandMarket.MainMenu;

namespace SecondHandMarket;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(Environment.CurrentDirectory);

        InitSql init = new InitSql();
        ListingCollection listingCollection = new ListingCollection();
        UserCollection userCollection = new UserCollection();
        
        //init.CreateDatabase();

        userCollection.RegisterUser();
        string activeUser = userCollection.GetActiveUser();

        listingCollection.CreateListing(activeUser);
        
        foreach (Listings listing in listingCollection
                     .FilterListingsBy(ListingFilters.Search())
                     .GetAll())
        {
            Console.WriteLine(listing.ToString());
        }
    }
}