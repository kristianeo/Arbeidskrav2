using System.Data.SQLite;
using SecondHandMarket.Database;
using SecondHandMarket.MainMenu;

namespace SecondHandMarket.ItemListings;

public class DepricatedListingCollection
{
    private List<Listings> _listings = new List<Listings>();

    public Listings CreateListing(DbInteractor interactor, ListingCollection lc)
    {
        string username = interactor.GetActiveUsername();
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
    public static Listings EditTitle(Listings listing)
    {
        listing.Title = ValidEntryChecker.GetValidString(1, 20);
        return listing;
    }

    public static Listings EditPrice(Listings listing)
    {
        listing.Price = ValidEntryChecker.GetValidInt(1, 100000);
        return listing;
    }

    public static Listings EditDescription(Listings listing)
    {
        listing.Description = ValidEntryChecker.GetValidString(0, 200);
        return listing;
    }

    public static Listings EditCondition(Listings listing)
    {
        int i = 1;
        foreach (KeyValuePair<string, string> kvp in Listings._conditions)
        {
            Console.WriteLine($"{ i++ }. { kvp.Key }{ kvp.Value }");
        }
        Console.Write("Please choose the condition of the item: ");
        int choice = ValidEntryChecker.GetValidInt(1, 4);
        listing.Condition = Listings._conditions.Keys.ElementAt(choice - 1);
        return listing;
    }

    public static Listings EditCategory(Listings listing)
    {
        int i = 1;
        foreach (Enum value in Enum.GetValues(typeof(Listings.Categories)))
        {
            Console.WriteLine($"{ i++ }. {value.ToString()}");
        }
        Console.Write("Please choose the category of the item: ");
        int choice = ValidEntryChecker.GetValidInt(1, 6);
        listing.Category = (Listings.Categories)choice - 1;
        return listing;
    }
    
    /// <summary>
    /// Depricated
    /// </summary>
    /// <param name="db"></param>
    /// <param name="listingId"></param>
    public static void EditListingTitle(Init db, int listingId)
    {
        string newTitle = ValidEntryChecker.GetValidString(1, 20);
        SQLiteConnection myConn = db.GetConnection();
        string sql = $"UPDATE listings SET title = '{newTitle}' WHERE listingID = '{listingId}'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();
        myConn.Close();
    }
    /// <summary>
    /// Depricated
    /// </summary>
    /// <param name="db"></param>
    /// <param name="listingId"></param>
    public static void EditListingDescription(DbInteractor interactor, int listingId)
    {
        if (!interactor.IsSeller(listingId))
        {
            Console.WriteLine("You cannot edit this listing.");
        }

        string newDescription = ValidEntryChecker.GetValidString(1, 200);
        SQLiteConnection myConn = interactor.GetConnection();
        string sql = $"UPDATE listings SET description = '{newDescription}' WHERE listingID = '{listingId}'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();
        myConn.Close();
    }


}