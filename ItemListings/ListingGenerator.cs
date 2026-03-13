namespace SecondHandMarket;

public class ListingGenerator
{
    public static Listings GenerateListing(string username) //TODO: Clean up?
    {
        string seller = username;
        
        Console.Write("Title of listing: ");
        string title = ValidEntryChecker.GetValidString(1, 20);
        
        Console.Write("Description of listing: ");
        string description = ValidEntryChecker.GetValidString(0, 200);
        
        int i = 1;
        foreach (KeyValuePair<string, string> kvp in Listings._conditions)
        {
            Console.WriteLine($"{ i++ }. { kvp.Key }{ kvp.Value }");
        }
        Console.Write("Please choose the condition of the item: ");
        int choice = ValidEntryChecker.GetValidInt(1, 4);
        string condition = Listings._conditions.Keys.ElementAt(choice - 1);
        
        i = 1;
        foreach (Enum value in Enum.GetValues(typeof(Listings.Categories)))
        {
            Console.WriteLine($"{ i++ }. {value.ToString()}");
        }
        Console.Write("Please choose the category of the item: ");
        int choice2 = ValidEntryChecker.GetValidInt(1, 6);
        Listings.Categories category = (Listings.Categories)choice2 - 1;
        
        Console.Write("Please enter the price for the item: ");
        int price = ValidEntryChecker.GetValidInt(1, 100000);
        
        return new Listings(seller, title, description, category, condition, price);
    }
}