using System.Data.SQLite;
using SecondHandMarket.Database;
using SecondHandMarket.MainMenu;

namespace SecondHandMarket;

public class ListingCollection
{
    private List<Listings> _listings = new List<Listings>();
    private int _listingID; //TODO: do something with this?

    public Listings CreateListing(Init db)
    {
        string username = db.GetActiveUser();
        Listings listing = new Listings(ListingGenerator.GenerateListing(db));
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

    public static Listings.Categories ChooseCategory()
    {
        int i = 1;
        foreach (Enum value in Enum.GetValues(typeof(Listings.Categories)))
        {
            Console.WriteLine($"{i++}. {value.ToString()}");
        }

        Console.Write("Item category: ");
        int choice = ValidEntryChecker.GetValidInt(1, 6);
        return (Listings.Categories)choice - 1;
    }

    public static string ChooseItemCondition()
    {
        int i = 1;
        foreach (KeyValuePair<string, string> kvp in Listings._conditions)
        {
            Console.WriteLine($"{ i++ }. { kvp.Key }: { kvp.Value }");
        }
        Console.Write("Item condition: ");
        int choice = ValidEntryChecker.GetValidInt(1, 4);
        return Listings._conditions.Keys.ElementAt(choice - 1);
    }
    public static (string, string) EditListing(Init db)
    {
        db.ShowUserListings();
        Console.Write("Please enter the ID of the listing you wish to edit: ");
        int choice = ValidEntryChecker.GetValidInt(1, 200);
        if (!db.ShowListingById(choice))
        {
            Console.WriteLine("There is not a listing with ID: " + choice);
        }

        Console.WriteLine("What would you like to edit? " +
                          "\n1. Title" +
                          "\n2. Description" +
                          "\n3. Category" +
                          "\n4. Item condition" +
                          "\n5. Price");
        int choice2 = ValidEntryChecker.GetValidInt(1, 6);

        string update;
        string newData;
        switch (choice2)
        {
            case '1':
                update = "title";
                newData = ValidEntryChecker.GetValidString(1, 20);
                break;
            case '2':
                update = "description";
                newData = ValidEntryChecker.GetValidString(1, 200);
                break;
            case '3':
                update = "category";
                newData = ChooseCategory().ToString();
                break;
            case '4':
                update = "itemCondition";
                newData = ChooseItemCondition();
                break;
            case '5':
                update = "price";
                newData = ValidEntryChecker.GetValidInt(1, 10000).ToString();
                break;
            default:
                update = "";
                newData = "";
                break;
        }
        return (update, newData);
    }
    public static void EditListingGeneric(Init db, int listingId)
    {
        if (!db.IsSeller(listingId))
        {
            Console.WriteLine("You cannot edit this listing.");
        }

        string edit = EditListing(db).Item1;
        string newData = EditListing(db).Item2;

        SQLiteConnection myConn = db.GetConnection();
        string sql = $"UPDATE listings SET '{edit}' = '{newData}' WHERE listingID = '{listingId}'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();
        myConn.Close();
    }

}