using System.Data.SQLite;
using SecondHandMarket.Database;

namespace SecondHandMarket;

public class ListingEditor
{
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
    
    
    private (string, string, int) ChooseListingToEdit(DbInteractor interactor, ListingCollection lc)
    {
        interactor.ShowListing(interactor.ActiveUserListings());
        Console.Write("Please enter the ID of the listing you wish to edit: ");
        int choice = ValidEntryChecker.GetValidInt(1, 200);
        if (!interactor.ShowListing(interactor.ShowListingById(choice)))
        {
            Console.WriteLine("There is not a listing with ID: " + choice);
        }

        Console.WriteLine("What would you like to edit? " +
                          "\n1. Title" +
                          "\n2. Description" +
                          "\n3. Category" +
                          "\n4. Item condition" +
                          "\n5. Price");
        choice = ValidEntryChecker.GetValidInt(1, 6);

        string update;
        string newData;
        switch (choice)
        {
            case 1:
                update = "title";
                Console.Write("New title: ");
                newData = ValidEntryChecker.GetValidString(1, 20);
                break;
            case 2:
                update = "description";
                Console.Write("New description: ");
                newData = ValidEntryChecker.GetValidString(1, 200);
                break;
            case 3:
                update = "category";
                newData = lc.ChooseCategory().ToString();
                break;
            case 4:
                update = "itemCondition";
                newData = lc.ChooseItemCondition();
                break;
            case 5:
                update = "price";
                Console.Write("New price: ");
                newData = ValidEntryChecker.GetValidInt(1, 10000).ToString();
                break;
            default:
                update = "";
                newData = "";
                break;
        }
        return (update, newData, choice);
    }
    public void EditListing(DbInteractor interactor, ListingCollection lc)
    {
        var (edit, newData, listing) = ChooseListingToEdit(interactor, lc);
        
        if (!interactor.IsSeller(listing))
        {
            Console.WriteLine("You cannot edit this listing.");
        }

        SQLiteConnection myConn = interactor.GetConnection();
        string sql = $"UPDATE listings SET '{edit}' = '{newData}' WHERE listingID = '{listing}'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();
        myConn.Close();
    }
}