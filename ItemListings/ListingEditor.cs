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
}