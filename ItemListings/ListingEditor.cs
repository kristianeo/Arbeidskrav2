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
    
    
    private (string, string) ChooseListingToEdit(ListingCollection lc)
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
        var (edit, newData) = ChooseListingToEdit(lc);

        SQLiteConnection myConn = interactor.GetConnection();
        string sql = $"UPDATE listings SET '{edit}' = '{newData}' WHERE listingID = '{listingId}'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();
        myConn.Close();
        Console.WriteLine($"{edit} has been edited to '{newData}'");
    }
}