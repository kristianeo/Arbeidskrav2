using SecondHandMarket.Database;
using SecondHandMarket.MainMenu;

namespace SecondHandMarket.ItemListings;

public class DepricatedListingCollection
{
    private List<Listings> _listings = new List<Listings>();

    public Listings CreateListing(DbInteractor interactor, ListingCollection lc)
    {
        string username = interactor.GetActiveUser();
        Listings listing = new Listings(ListingGenerator.GenerateListing(interactor, lc));
        _listings.Add(listing);
        Console.WriteLine("Listing created");
        return listing;
    }
    
    private Listings DuplicateListing(Listings listing)
    {
        _listings.Add(new Listings(listing));
        return listing;
    }
    public DepricatedListingCollection FilterListingsBy(ListingFilter filter)
    {
        DepricatedListingCollection results = new();
        foreach (Listings listing in _listings) 
        {
            if (filter.Invoke(listing))
            {
                results.DuplicateListing(listing);
            }
        }
        return results;
    }

    public void ShowListings()
    {
        foreach (Listings listings in _listings)
        {
            Console.WriteLine(listings.ToString());
        }
    }
    public List<Listings> GetAll()
    {
        return new List<Listings>(_listings);
    }
}