namespace SecondHandMarket;

public class ListingGenerator
{
    public static Listings GenerateListing(string username) //TODO: Clean up?
    {
        string seller = username;
        
        Console.Write("Title of listing: ");
        string title = Console.ReadLine();
        
        Console.Write("Description of listing: ");
        string description = Console.ReadLine();
        
        int i = 1;
        foreach (KeyValuePair<string, string> kvp in Listings._conditions)
        {
            Console.WriteLine($"{ i++ }. { kvp.Key }{ kvp.Value }");
        }
        Console.Write("Please choose the condition of the item: ");
        int choice = ValidEntryChecker.GetValidInt(1, 4);
        string condition = Listings._conditions.Keys.ElementAt(choice - 1);
        
        int j = 1;
        foreach (Enum value in Enum.GetValues(typeof(Listings.Categories)))
        {
            Console.WriteLine($"{ j++ }. {value.ToString()}");
        }
        Console.Write("Please choose the category of the item: ");
        int choice2 = ValidEntryChecker.GetValidInt(1, 6);
        Listings.Categories category = (Listings.Categories)choice2 - 1;
        
        Console.WriteLine("Please enter the price for the item: ");
        int price = ValidEntryChecker.GetValidInt(1, 100000);
        
        return new Listings(seller, title, description, category, condition, price);
    }
}