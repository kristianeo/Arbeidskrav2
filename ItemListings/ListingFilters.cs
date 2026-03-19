using System.Runtime.InteropServices.ComTypes;
using SecondHandMarket.Database;

namespace SecondHandMarket.MainMenu;
/// <summary>
/// Deprecated. Used to create filters before the db was implemented.
/// </summary>
public delegate bool ListingFilter(Listings listing);

public abstract class ListingFilters
{
    /// <summary>
    /// Lets the user choose a category to filter listings by
    /// </summary>
    /// <param name="lc"></param>
    /// <returns>SQL statement to use in ShowListings()</returns>
    public static string CategoryFilter(ListingCollection lc)
    {
        Listings.Categories category = lc.ChooseCategory();
        return "SELECT * FROM listings " +
                     "JOIN users on listings.sellerID = users.userID " +
                     $"WHERE category = '{category}'";
    }
    /// <summary>
    /// Deprecated. Creates a ListingFilter to filter by category.
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public static ListingFilter CategoryFilter(Listings.Categories category)
    {
        return listing => listing.Category == category;
    }
    /// <summary>
    /// Lets the user search for a phrase in listing title or description.
    /// </summary>
    /// <returns>SQL statement to use in ShowListings()</returns>
    public static string SearchFilter()
    {
        Console.Write("Search: ");
        string userSearch = ValidEntryChecker.GetValidString(1, 200);
        return "SELECT * FROM listings " +
               "JOIN users on listings.sellerID = users.userID " +
               $"WHERE title LIKE '%{userSearch}%' OR description LIKE '%{userSearch}%'";
    }
    /// <summary>
    /// Deprecated. Creates a ListingFilter to search for a phrase in listing title or description.
    /// </summary>
    /// <returns></returns>
    public static ListingFilter Search()
    {
        Console.Write("Search: ");
        string userSearch = ValidEntryChecker.GetValidString(1, 200);
        return listing => listing.Title.ToLower().Contains(userSearch.ToLower()) 
                          || listing.Description.ToLower().Contains(userSearch.ToLower());
    }
    
}