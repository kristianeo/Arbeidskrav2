using SecondHandMarket.Database;

namespace SecondHandMarket;

public class ListingGenerator
{
    public static Listings GenerateListing(DbInteractor interactor, ListingCollection lc)
    {
        string seller = interactor.GetActiveUser();
        
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
}