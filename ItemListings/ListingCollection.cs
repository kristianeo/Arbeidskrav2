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

    public int Purchase(DbInteractor interactor, int listingId)
    {
        int buyerId = interactor.GetActiveUserId();   
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

    public void RemoveListing(DbInteractor interactor, int listingId) 
    {
        SQLiteConnection myConn = interactor.GetConnection();
        string sql = $"DELETE FROM listings WHERE listingID = '{listingId}'";
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