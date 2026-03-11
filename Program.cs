using SecondHandMarket.MainMenu;

namespace SecondHandMarket;

class Program
{
    static void Main(string[] args)
    {
        ListingCollection listingCollection = new ListingCollection();
        UserCollection userCollection = new UserCollection();

        userCollection.RegisterUser();
        string activeUser = userCollection.GetActiveUser();

        listingCollection.CreateListing(activeUser);
        
        foreach (Listings listing in listingCollection
                     .FilterListingsBy(ListingFilters.CategoryFilter(Listings.Categories.Electronics))
                     .GetAll())
        {
            Console.WriteLine(listing.ToString());
        }
    }
}