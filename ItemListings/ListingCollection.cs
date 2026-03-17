using System.Data.SQLite;
using SecondHandMarket.Database;
using SecondHandMarket.ItemListings;
using SecondHandMarket.MainMenu;

namespace SecondHandMarket;

public class ListingCollection
{
    
    public Listings.Categories ChooseCategory()
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

    public string ChooseItemCondition()
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
    public (string, string, int) ChooseListingToEdit(DbInteractor interactor)
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
                newData = ChooseCategory().ToString();
                break;
            case 4:
                update = "itemCondition";
                newData = ChooseItemCondition();
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
    public void EditListing(DbInteractor interactor)
    {
        var (edit, newData, listing) = ChooseListingToEdit(interactor);
        
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

    public int BuyListing(DbInteractor interactor)
    {
        interactor.ShowListing(interactor.OthersListings());
        int buyerId = interactor.GetActiveUserId();
        Console.Write("Please chose listing you wish to buy: ");
        int listingId = ValidEntryChecker.GetValidInt(1, 200);

        if (!interactor.ShowListing(interactor.ShowListingById(listingId)))
        {
            Console.WriteLine("The listing does not exist.");
        }
        
        if (interactor.IsSeller(listingId))
        {
            Console.WriteLine("You cannot buy your own listing.");
        }
        
        SQLiteConnection myConn = interactor.GetConnection();
        string sql = $"UPDATE listings SET status = 'Sold', buyerID = '{buyerId}' " +
                     $"WHERE listingID = '{listingId}'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();
        myConn.Close();
        return listingId;
    }

    private void RemoveListing(DbInteractor interactor, int listingId)
    {
        if (!interactor.IsSeller(listingId))
        {
            Console.WriteLine("You cannot remove this listing.");
        }
        SQLiteConnection myConn = interactor.GetConnection();
        string sql = $"DELETE FROM listings WHERE listingID = '{listingId}'" +
                     $"LIMIT 1;";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();
        myConn.Close();
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