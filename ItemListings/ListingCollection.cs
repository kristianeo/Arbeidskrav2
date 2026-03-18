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
        if (!interactor.ShowListingById(listingId))
        {
            Console.WriteLine("The listing does not exist.");
        }
        
        if (interactor.IsSeller(listingId))
        {
            Console.WriteLine("You cannot buy your own listing.");
        }
        
        SQLiteConnection myConn = interactor.GetConnection();
        string sql = $"UPDATE listings SET status = 'Sold', buyerID = '{buyerId}', dateOfPurchase = '{DateTime.Now:yyyy-MM-dd}' " +
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
        Console.WriteLine("Listing has been removed. ");
    }
    
    private (string, string) ChooseParamToEdit(ListingCollection lc)
    {
        Console.Write("What would you like to edit? " +
                      "\n1. Title" +
                      "\n2. Description" +
                      "\n3. Category" +
                      "\n4. Item condition" +
                      "\n5. Price" +
                      "\n\nSelect an option: ");

        string edit = "";
        string newData = "";
        switch (ValidEntryChecker.GetValidInt(1, 5))
        {
            case 1:
                edit = "title";
                Console.Write("New title: ");
                newData = ValidEntryChecker.GetValidString(1, 20);
                break;
            case 2:
                edit = "description";
                Console.Write("New description: ");
                newData = ValidEntryChecker.GetValidString(0, 200);
                break;
            case 3:
                edit = "category";
                newData = lc.ChooseCategory().ToString();
                break;
            case 4:
                edit = "itemCondition";
                newData = lc.ChooseItemCondition();
                break;
            case 5:
                edit = "price";
                Console.Write("New price: ");
                newData = ValidEntryChecker.GetValidInt(1, 10000).ToString();
                break;
        }
        return (edit, newData);
    }
    public void EditListing(DbInteractor interactor, ListingCollection lc, int listingId)
    {
        var (edit, newData) = ChooseParamToEdit(lc);

        SQLiteConnection myConn = interactor.GetConnection();
        string sql = $"UPDATE listings SET '{edit}' = '{newData}' WHERE listingID = '{listingId}'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();
        myConn.Close();
        Console.WriteLine("Listing has been updated. ");
    }
    
}