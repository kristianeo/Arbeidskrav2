using System.Runtime.InteropServices.ComTypes;
using SecondHandMarket.Database;

namespace SecondHandMarket.MainMenu;

public delegate bool ListingFilter(Listings listing);

public abstract class ListingFilters
{
    public static string CategoryFilter(ListingCollection lc)
    {
        Listings.Categories category = lc.ChooseCategory();
        return "SELECT * FROM listings " +
                     "JOIN users on listings.sellerID = users.userID " +
                     $"WHERE category = '{category}'";
    }
    
    
    public static ListingFilter CategoryFilter(Listings.Categories category)
    {
        return listing => listing.Category == category;
    }
    
    public static string SearchFilter(ListingCollection lc)
    {
        Console.Write("Search: ");
        string userSearch = ValidEntryChecker.GetValidString(1, 200);
        return "SELECT * FROM listings " +
               "JOIN users on listings.sellerID = users.userID " +
               $"WHERE title LIKE '%{userSearch}%' OR description LIKE '%{userSearch}%'";
    }

    public static ListingFilter Search()
    {
        Console.Write("Search: ");
        string userSearch = ValidEntryChecker.GetValidString(1, 200);
        return listing => listing.Title.ToLower().Contains(userSearch.ToLower()) 
                          || listing.Description.ToLower().Contains(userSearch.ToLower());
    }
    
}