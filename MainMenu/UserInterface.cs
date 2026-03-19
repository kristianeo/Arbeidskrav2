using System.Data.SQLite;
using SecondHandMarket.Database;

namespace SecondHandMarket.MainMenu;

public class UserInterface
{
    /// <summary>
    /// Lets the user log in, checks for valid username and password input,
    /// then sets the user as active
    /// </summary>
    /// <param name="interactor"></param>
    public void UserLogin(DbInteractor interactor)
    {
        Start:
        Console.WriteLine();
        string username = ValidEntryChecker.GetValidUsername(interactor);
        string password = ValidEntryChecker.GetConsoleSecurePassword();

        if (!interactor.CheckUserCredentials(username, password))
        {
            goto Start;
        }
        interactor.SetUserAsActive(username);
    }
    /// <summary>
    /// Logs out current user by setting it as inactive.
    /// </summary>
    /// <param name="interactor"></param>
    public void UserLogout(DbInteractor interactor)
    {
        interactor.SetUserAsInactive();
    }
    /// <summary>
    /// Lets the user view the screen after certain actions before
    /// returning to the main menu
    /// </summary>
    public void GoBackToMainMenu()
    {
        Console.Write("\nPress any key to go back to main menu: ");
        Console.ReadKey();
    }
    /// <summary>
    /// Prints one or more listings from database,
    /// dependent on which method is used to get SQL statement 
    /// </summary>
    /// <param name="interactor"></param>
    /// <param name="sql">SQL statement returned from other method</param>
    /// <returns>True if there is one or more listing to view</returns>
    public bool ShowListings(DbInteractor interactor, string sql)
    {
        Console.WriteLine("  #  Title                 Category     Condition  Price");
        bool exists = false;
        SQLiteConnection myConn = interactor.GetConnection();

        using SQLiteCommand readThis = new SQLiteCommand(sql, myConn);
        using (SQLiteDataReader dataReader = readThis.ExecuteReader())
        {
            while (dataReader.Read())
            {
                int id = Convert.ToInt32(dataReader["listingID"]);
                string? title = dataReader["title"].ToString();
                string? category = dataReader["category"].ToString();
                string? itemCondition = dataReader["itemCondition"].ToString();
                decimal price = Convert.ToDecimal(dataReader["price"]);

                Console.WriteLine(
                    $"  {id.ToString(),-2} {title,-21} {category,-12} {itemCondition,-10} {price} kr");
                exists = true;
            }
        }
        myConn.Close();
        return exists;
    }
    /// <summary>
    /// Shows additional data from selected listing
    /// </summary>
    /// <param name="interactor"></param>
    /// <param name="listingId"></param>
    /// <returns>True if listing exists</returns>
    private bool ShowListingById(DbInteractor interactor, int listingId)
    {
        bool exists = false;
        string sql = "SELECT * FROM listings " +
                     "JOIN users on listings.sellerID = users.userID " +
                     $"WHERE listingID = '{listingId}'";

        SQLiteConnection myConn = interactor.GetConnection();

        using SQLiteCommand readThis = new SQLiteCommand(sql, myConn);
        using (SQLiteDataReader dataReader = readThis.ExecuteReader())
        {
            while (dataReader.Read())
            {
                string? name = dataReader["username"].ToString();
                string? title = dataReader["title"].ToString();
                string? description = dataReader["description"].ToString();
                string? category = dataReader["category"].ToString();
                string? itemCondition = dataReader["itemCondition"].ToString();
                decimal price = Convert.ToDecimal(dataReader["price"]);

                Console.WriteLine(
                    $"\n=== {title} ===" +
                    $"\nSeller:      {name}" +
                    $"\nCategory:    {category} " +
                    $"\nCondition:   {itemCondition} " +
                    $"\nPrice:       {price} kr" +
                    $"\nDescription: {description}");
                exists = true;
            }
        }
        myConn.Close();
        return exists;
    }
    /// <summary>
    /// Lets the user select to search by category or title/description.
    /// Shows listings relevant to search. 
    /// </summary>
    /// <param name="lc"></param>
    /// <param name="interactor"></param>
    public void SearchListings(ListingCollection lc, DbInteractor interactor)
    {
        Console.Write("\n1. Category" +
                      "\n2. Title or description" +
                      "\n3. Go back to main menu" +
                      "\n\nSelect an option: ");
        switch (ValidEntryChecker.GetValidInt(1, 3))
        {
            case 1:
                Console.Clear();
                Console.WriteLine("=== Search by category ===\n");
                if (!ShowListings(interactor, ListingFilters.CategoryFilter(lc)))
                {
                    Console.WriteLine("No listings in this category.");
                }
                break;
            case 2:
                Console.Clear();
                Console.WriteLine("=== Search in title or description ===\n");
                if (!ShowListings(interactor, ListingFilters.SearchFilter()))
                {
                    Console.WriteLine("No results for your search.");
                }
                break;
            case 3:
                break;
        }
    }
    /// <summary>
    /// Prompts the user if they want to view a specific listing or go back to main menu.
    /// </summary>
    /// <returns>True if user wants to view listing</returns>
    public bool ViewListing()
    {
        Console.Write("\n1. View listing" +
                          "\n2. Return to main menu" +
                          "\nSelect an option: ");
        if (ValidEntryChecker.GetValidInt(1, 2) == 1) return true;
        return false;
    }
    /// <summary>
    /// Edits the status of purchased listing in the database
    /// to 'Sold' and inserts buyerID and date of purchase.
    /// </summary>
    /// <param name="interactor"></param>
    /// <param name="listingId"></param>
    private void Purchase(DbInteractor interactor, int listingId)
    {
        int buyerId = interactor.GetActiveUserId();   
        
        SQLiteConnection myConn = interactor.GetConnection();
        string sql = $"UPDATE listings SET status = 'Sold', buyerID = '{buyerId}', dateOfPurchase = '{DateTime.Now:yyyy-MM-dd}' " +
                     $"WHERE listingID = '{listingId}'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();
        myConn.Close();
    }
    /// <summary>
    /// Checks that the current user is not purchasing their own listing or a non-existent one.
    /// </summary>
    /// <param name="interactor"></param>
    /// <returns>Valid listingID when conditions are met.</returns>
    private int PurchaseListingChecker(DbInteractor interactor)
    {
        while (true)
        {
            try
            {
                Console.Write("\nSelect listing #: ");
                int listingId = ValidEntryChecker.GetValidInt(1, 200);
                if (!interactor.IsSeller(listingId) && ShowListingById(interactor, listingId))
                {
                    return listingId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Please enter a valid listing #");
            }
        }
    }
    /// <summary>
    /// Promts the user if they want to purchase the listing they're viewing.
    /// If yes, runs Purchase() and prompts if the user wishes to leave a review. 
    /// </summary>
    /// <param name="interactor"></param>
    /// <param name="uc"></param>
    public void PurchaseListing(DbInteractor interactor, UserCollection uc)
    {
        int listingId = PurchaseListingChecker(interactor); 
        Console.Write("\nPurchase this listing?" +
                      "\n1. Yes" +
                      "\n2. No" +
                      "\nSelect an option: ");
        if (ValidEntryChecker.GetValidInt(1, 2) != 1) return;
        Purchase(interactor, listingId);
        Console.WriteLine("Purchase complete!");
        
        Console.Write("\nDo you wish to leave a review? " +
                      "\n1. Yes" +
                      "\n2. No" +
                      "\nSelect an option: ");
        if (ValidEntryChecker.GetValidInt(1, 2) == 2) return;

        uc.LeaveReview(interactor, listingId);
    }//TODO: Make it go back to listings theyre viewing 
    /// <summary>
    /// Checks if viewed listing can be edited by logged-in user 
    /// </summary>
    /// <param name="interactor"></param>
    /// <param name="listingId"></param>
    /// <returns></returns>
    private int EditListingChecker(DbInteractor interactor)
    {
        while (true)
        {
            try
            {
                Console.Write("\nSelect listing to view (listing #): ");
                int listingId = ValidEntryChecker.GetValidInt(1, 200);
                if (interactor.IsSeller(listingId) && ShowListingById(interactor, listingId))
                {
                    return listingId;
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine("Error: Please enter a valid listing #");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Listing # is not valid");
            }
        }
    }
    /// <summary>
    /// Gives user prompt to edit or remove viewed listing, or go back to main menu.
    /// Calls EditListing() or RemoveListing() (or none).
    /// </summary>
    /// <param name="interactor"></param>
    /// <param name="lc"></param>
    private void ViewOwnListings(DbInteractor interactor, ListingCollection lc)
    {
        Console.Clear();
        ShowListings(interactor, interactor.ActiveUserListings());
        int listingId = EditListingChecker(interactor);
        
        Console.Write("\n1. Edit" +
                      "\n2. Remove" +
                      "\n3. Go back to main menu" +
                      "\nSelect an option: ");
        switch (ValidEntryChecker.GetValidInt(1, 3))
        {
            case 1:
                lc.EditListing(interactor, listingId);
                GoBackToMainMenu();
                break;
            case 2:
                lc.RemoveListing(interactor, listingId);
                GoBackToMainMenu();
                break;
            case 3:
                break;
        }
    }
    /// <summary>
    /// Prints user profile. Gives prompt to view a listing(and then edit/remove it) or return to main menu,
    /// </summary>
    /// <param name="interactor"></param>
    /// <param name="uc"></param>
    /// <param name="lc"></param>
    public void ShowUserProfile(DbInteractor interactor, UserCollection uc, ListingCollection lc)
    {
        string user = interactor.GetActiveUsername();
        
        Console.Clear();
        Console.WriteLine($"=== My Profile: {user} ===\n");
        Console.WriteLine($"Average review score: {ShowAverageScore(interactor)}");

        Console.WriteLine("\n-Your active listings-");
        if (!ShowListings(interactor, interactor.ActiveUserListings()))
        {
            Console.WriteLine("You have no active listings.");
        }
        
        Console.WriteLine("\n-Your sold listings-");
        if (!ShowListings(interactor, interactor.ActiveUserListings()))
        {
            Console.WriteLine("You have no sold listings.");
        }

        Console.WriteLine("\n-Your purchases-");
        if (!ShowListings(interactor, uc.ShowPurchaseHistory(interactor)))
        {
            Console.WriteLine("No purchase history found.");
        }

        Console.WriteLine("\n-Your reviews-");
        if (!uc.ShowReviewHistory(interactor))
        {
            Console.WriteLine("No review history found.");
        }

        Console.Write("\nWhat would you like to do?" +
                      "\n1. View your listings" +
                      "\n2. Go to main menu" +
                      "\nSelect an option: ");
        if (ValidEntryChecker.GetValidInt(1, 2)  != 1) return;
        
        ViewOwnListings(interactor, lc);
    }
    /// <summary>
    /// Calculates the average review score from the reviews in database for active user 
    /// </summary>
    /// <param name="interactor"></param>
    /// <returns>Float score average</returns>
    private double ShowAverageScore(DbInteractor interactor)
    {
        double result = 0.0;
        SQLiteConnection myConn = interactor.GetConnection();
        int user = interactor.GetActiveUserId();

        string sql = "SELECT avg(score) FROM reviews " +
                     $"WHERE sellerID = '{user}'";

        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        var check = command.ExecuteScalar();
        if (check.ToString().Length != 0)
        {
            result = Convert.ToDouble(check.ToString());
        }
        myConn.Close();
        return result;
    }
}