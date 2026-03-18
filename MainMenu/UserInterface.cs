using System.Data.SQLite;
using SecondHandMarket.Database;

namespace SecondHandMarket.MainMenu;

public class UserInterface
{
    public void CreateListing(DbInteractor interactor, ListingCollection lc)
    {
        Listings listing = ListingGenerator.GenerateListing(interactor, lc);
        interactor.AddListingToTable(listing);
        Console.WriteLine($"Listing {listing.Title} created!");
    }

    public void GoBackToMainMenu()
    {
        Console.Write("\nPress any key to go back to main menu: ");
        Console.ReadKey();
    }

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
                interactor.ShowListings(ListingFilters.CategoryFilter(lc));
                break;
            case 2:
                Console.Clear();
                Console.WriteLine("=== Search in title or description ===\n");
                interactor.ShowListings(ListingFilters.SearchFilter(lc));
                break;
            case 3:
                break;
        }
        
    }

    public void PurchaseListing(DbInteractor interactor, ListingCollection lc, UserCollection uc)
    {
        Start:
        Console.Write("\nSelect listing #: ");
        int listingId = ValidEntryChecker.GetValidInt(1, 200);
        if (!interactor.ShowListingById(listingId))
        {
            Console.WriteLine("Invalid listingID.");
            goto Start;
        }

        Console.Clear();
        interactor.ShowListingById(listingId);
        Console.Write("\nPurchase this listing?" +
                      "\n1. Yes" +
                      "\n2. No" +
                      "\nSelect an option: ");
        if (ValidEntryChecker.GetValidInt(1, 2) != 1) return;
        lc.Purchase(interactor, listingId);
        Console.WriteLine("Purchase complete!");
        uc.LeaveReview(interactor, listingId);
    }

    private bool EditListingChecker(DbInteractor interactor, int listingId)
    {
        if (!interactor.ShowListingById(listingId))
        {
            Console.WriteLine("There is not a listing with ID: " + listingId);
            return false;
        }

        if (!interactor.IsSeller(listingId))
        {
            Console.WriteLine("You cannot edit this listing.");
            return false;
        }

        return true;
    }

    private void ViewOwnListings(DbInteractor interactor, ListingCollection lc)
    {
        interactor.ShowListings(interactor.ActiveUserListings());
        Console.Write("\n#. Select listing to view (listing #): " +
                      "\n\nSelect an option: ");
        int listingId = ValidEntryChecker.GetValidInt(1, 200);

        if (!EditListingChecker(interactor, listingId)) return;
        
        Console.Write("\n1. Edit" +
                      "\n2. Remove" +
                      "\n3. Go back to main menu" +
                      "\n\nSelect an option: ");
        switch (ValidEntryChecker.GetValidInt(1, 3))
        {
            case 1:
                lc.EditListing(interactor, lc, listingId);
                break;
            case 2:
                lc.RemoveListing(interactor, listingId);
                break;
            case 3:
                break;
        }
    }
    
    public void ShowUserProfile(DbInteractor interactor, UserCollection uc, ListingCollection lc)
    {
        string user = interactor.GetActiveUsername();
        
        Console.Clear();
        Console.WriteLine($"=== My Profile: {user} ===\n");
        Console.WriteLine($"Average review score: {ShowAverageScore(interactor)}");

        Console.WriteLine("\n-Your listings-");
        if (!interactor.ShowListings(interactor.ActiveUserListings()))
        {
            Console.WriteLine("No listings found.");
        }

        Console.WriteLine("\n-Your purchases-");
        if (!interactor.ShowListings(uc.ShowPurchaseHistory(interactor)))
        {
            Console.WriteLine("No purchase history found.");
        }

        Console.WriteLine("\n-Your reviews-");
        if (!uc.ShowReviewHistory(interactor))
        {
            Console.WriteLine("No review history found.");
        }

        Console.Write("What would you like to do?" +
                      "\n1. View listing" +
                      "\n2. Go to main menu" +
                      "\nSelect an option: ");
        if (ValidEntryChecker.GetValidInt(1, 2)  != 1) return;
        
        ViewOwnListings(interactor, lc);
    }

    private int ShowAverageScore(DbInteractor interactor)
    {
        SQLiteConnection myConn = interactor.GetConnection();
        int user = interactor.GetActiveUserId();

        string sql = "SELECT avg(score) FROM reviews " +
                     $"WHERE sellerID = '{user}'";

        SQLiteCommand command = new SQLiteCommand(sql, myConn);

        int result = Convert.ToInt32(command.ExecuteScalar());
        myConn.Close();
        return result; //TODO: Must be float 
    }
}