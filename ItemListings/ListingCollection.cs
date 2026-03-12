using SecondHandMarket.MainMenu;

namespace SecondHandMarket;

public class ListingCollection
{
    private List<Listings> _listings = new List<Listings>();
    private int _listingID;

    public Listings CreateListing(string username) //TODO: Fix username to be more smooth
    {
        Listings listing = new Listings(ListingGenerator.GenerateListing(username));
        _listings.Add(listing);
        Console.WriteLine("Listing created");
        return listing;
    }
    
    private Listings DuplicateListing(Listings listing)
    {
        _listings.Add(new Listings(listing));
        return listing;
    }
    public ListingCollection FilterListingsBy(ListingFilter filter)
    {
        ListingCollection results = new();
        foreach (Listings listing in _listings) 
        {
            if (filter.Invoke(listing))
            {
                results.DuplicateListing(listing);
            }
        }
        return results;
    }
    public List<Listings> GetAll()
    {
        return new List<Listings>(_listings);
    }

    
    

}