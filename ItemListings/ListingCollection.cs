using SecondHandMarket.MainMenu;

namespace SecondHandMarket;

public class ListingCollection
{
    private static List<Listings> _listings = new List<Listings>();
    private int _listingID;

    public Listings CreateListing()
    {
        Listings listing = new Listings(ListingGenerator.GenerateListing());
        _listings.Add(listing);
        return listing;
    }
    
    public Listings DuplicateListing(Listings listing)
    {
        _listings.Add(new Listings(listing));
        return listing;
    }
    public ListingCollection FilterListingsBy(ListingFilter filter)
    {
        ListingCollection results = new();
        foreach (Listings listing in ListingCollection._listings)
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