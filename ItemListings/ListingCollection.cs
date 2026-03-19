using System.Data.SQLite;
using SecondHandMarket.Database;

namespace SecondHandMarket;

public class ListingCollection
{
    /// <summary>
    /// Prints the listing categories and lets the user choose one
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// Prints item condition and description, lets the user choose one
    /// </summary>
    /// <returns>Item condition without the description</returns>
    public string ChooseItemCondition()
    {
        int i = 1;
        foreach (KeyValuePair<string, string> kvp in Listings.Conditions)
        {
            Console.WriteLine($"{ i++ }. { kvp.Key }: { kvp.Value }");
        }
        Console.Write("Item condition: ");
        int choice = ValidEntryChecker.GetValidInt(1, 4);
        return Listings.Conditions.Keys.ElementAt(choice - 1);
    }
    /// <summary>
    /// Lets the user generate a listing with all required parameters.
    /// </summary>
    /// <param name="interactor"></param>
    /// <param name="lc"></param>
    /// <returns>Instance of Listings</returns>
    public Listings GenerateListingInstance(DbInteractor interactor, ListingCollection lc)
    {
        string seller = interactor.GetActiveUsername();
        
        Console.Write("Title of listing: ");
        string title = ValidEntryChecker.GetValidString(1, 20);
        
        Console.Write("Description of listing: ");
        string description = ValidEntryChecker.GetValidString(0, 200);
        
        string condition = lc.ChooseItemCondition();

        Listings.Categories category = lc.ChooseCategory();
        
        Console.Write("Price: ");
        int price = ValidEntryChecker.GetValidInt(1, 100000);
        
        return new Listings(seller, title, description, category, condition, price);
    }
    /// <summary>
    /// Creates instance of Listings from GenerateListingInstance
    /// and adds it to the database
    /// </summary>
    /// <param name="interactor"></param>
    /// <param name="lc"></param>
    public void CreateListing(DbInteractor interactor, ListingCollection lc)
    {
        Listings listing = GenerateListingInstance(interactor, lc);
        interactor.AddListingToTable(listing);
        Console.WriteLine($"Listing {listing.Title} created!");
    }
    /// <summary>
    /// Removes given listing from database
    /// </summary>
    /// <param name="interactor"></param>
    /// <param name="listingId"></param>
    public void RemoveListing(DbInteractor interactor, int listingId) 
    {
        SQLiteConnection myConn = interactor.GetConnection();
        string sql = $"DELETE FROM listings WHERE listingID = '{listingId}'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();
        myConn.Close();
        Console.WriteLine("Listing has been removed. ");
    }
    /// <summary>
    /// Lets user choose which parameter of the listing to edit,
    /// and input the new data for this parameter.
    /// </summary>
    /// <param name="lc"></param>
    /// <returns>Which parameter to edit and what it should contain</returns>
    private (string, string) ChooseParamToEdit()
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
                newData = ChooseCategory().ToString();
                break;
            case 4:
                edit = "itemCondition";
                newData = ChooseItemCondition();
                break;
            case 5:
                edit = "price";
                Console.Write("New price: ");
                newData = ValidEntryChecker.GetValidInt(1, 10000).ToString();
                break;
        }
        return (edit, newData);
    }
    /// <summary>
    /// Updates the database for given listing with the data from ChooseParamToEdit()
    /// </summary>
    /// <param name="interactor"></param>
    /// <param name="lc">ListingCollection instance</param>
    /// <param name="listingId"></param>
    public void EditListing(DbInteractor interactor, int listingId)
    {
        var (edit, newData) = ChooseParamToEdit();

        SQLiteConnection myConn = interactor.GetConnection();
        string sql = $"UPDATE listings SET '{edit}' = '{newData}' WHERE listingID = '{listingId}'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();
        myConn.Close();
        Console.WriteLine("Listing has been updated. ");
    }
    
}