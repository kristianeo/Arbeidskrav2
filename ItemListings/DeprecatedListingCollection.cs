using System.Data.SQLite;
using SecondHandMarket.Database;
using SecondHandMarket.MainMenu;

namespace SecondHandMarket.ItemListings;

public class DeprecatedListingCollection
{
    private List<Listings> _listings = new List<Listings>();
    /// <summary>
    /// Depricated
    /// Creates an instance of Listings and adds it to the _listings list.
    /// </summary>
    /// <param name="interactor"></param>
    /// <param name="lc"></param>
    /// <param name="ui"></param>
    /// <returns>instance of Listings</returns>
    public Listings CreateListing(DbInteractor interactor, ListingCollection lc, UserInterface ui)
    {
        Listings listing = new Listings(lc.GenerateListingInstance(interactor, lc));
        _listings.Add(listing);
        return listing;
    }
    /// <summary>
    /// Duplicates a listing for use in search results
    /// </summary>
    /// <param name="listing"></param>
    /// <returns></returns>
    private Listings DuplicateListing(Listings listing)
    {
        _listings.Add(new Listings(listing));
        return listing;
    }
    /// <summary>
    /// Returns a duplicate listing of listings matching the used filter.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public DeprecatedListingCollection FilterListingsBy(ListingFilter filter)
    {
        DeprecatedListingCollection results = new();
        foreach (Listings listing in _listings) 
        {
            if (filter.Invoke(listing))
            {
                results.DuplicateListing(listing);
            }
        }
        return results;
    }
    /// <summary>
    /// Writes a list of all the listings to the console
    /// </summary>
    public void ShowListings()
    {
        foreach (Listings listings in _listings)
        {
            Console.WriteLine(listings.ToString());
        }
    }
    /// <summary>
    /// Accesses the private list of listings in (Deprecated)ListingCollection
    /// </summary>
    /// <returns>A new list of listings</returns>
    public List<Listings> GetAll()
    {
        return new List<Listings>(_listings);
    }
    /// <summary>
    /// Edits the title of listing
    /// </summary>
    /// <param name="listing"></param>
    /// <returns></returns>
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
        foreach (KeyValuePair<string, string> kvp in Listings.Conditions)
        {
            Console.WriteLine($"{ i++ }. { kvp.Key }{ kvp.Value }");
        }
        Console.Write("Please choose the condition of the item: ");
        int choice = ValidEntryChecker.GetValidInt(1, 4);
        listing.Condition = Listings.Conditions.Keys.ElementAt(choice - 1);
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
    /// Deprecated
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
    /// Deprecated
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