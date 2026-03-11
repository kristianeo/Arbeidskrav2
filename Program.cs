using SecondHandMarket.MainMenu;

namespace SecondHandMarket;

class Program
{
    static void Main(string[] args)
    {
        ListingCollection collection = new ListingCollection();
        PremadeListings.Premade();
        foreach (Listings listings1 in ListingCollection._listings)
        {
            Console.WriteLine(listings1);
        }
        Users user = Users.RegisterUser();
        Console.WriteLine(user.ToString());
        Listings listings = ListingGenerator.CreateListing(user);
        user.AddListing(listings);
        Console.WriteLine(listings.ToString());

        foreach (Listings listing in collection
                     .FilterListingsBy(ListingFilters.CategoryTicketFilter(Listings.Categories.Electronics))
                     .GetAll())
        {
            Console.WriteLine(listing.ToString());
        }
    }
}