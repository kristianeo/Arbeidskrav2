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
        Console.Write("Press any key to go back to main menu: ");
        Console.ReadKey();
    }

    public void SearchListings(ListingCollection lc)
    {
        Console.Write("Search by: " +
                      "\n1. Category" +
                      "\n2. Title or description" +
                      "\n3. Go back to main menu" +
                      "\nPick an option: ");
        switch (ValidEntryChecker.GetValidInt(0, 2))
        {
            case 1:
                ListingFilters.CategoryFilter(lc);
                break;
            case 2:
                ListingFilters.SearchFilter(lc);
                break;
            case 3:
                break;
        }
        
    }

    public void BuyListing(DbInteractor interactor, ListingCollection lc, UserCollection uc) //TODO: Make this return to listing as option if there is time 
    {
        Start:
        interactor.ShowListing(interactor.OthersListings());
        Console.Write("\nSelect a listingID: ");
        int listingId = ValidEntryChecker.GetValidInt(1, 200);
        if (!interactor.ShowListing(interactor.ShowListingById(listingId)))
        {
            Console.WriteLine("Invalid listingID.");
            goto Start;
        }

        Console.Clear();
        Console.Write("Purchase this listing?" +
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
        if (!interactor.ShowListing(interactor.ShowListingById(listingId)))
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

    public void ViewOwnListings(DbInteractor interactor, ListingCollection lc)
    {
        interactor.ShowListing(interactor.ActiveUserListings());
        Console.Write("\n0. Go back to main menu" +
                      "\n1. Select listing to view (listing #): " +
                      "\n\nSelect an option: ");
        int listingId = ValidEntryChecker.GetValidInt(1, 200);
        
        if (listingId == 0) return;

        if (!EditListingChecker(interactor, listingId)) return;
        
        Console.Write("\n1. Edit" +
                      "\n2. Remove" +
                      "\n3. Go back to main menu" +
                      "\n\nSelect an option: ");
        switch (ValidEntryChecker.GetValidInt(1, 3))
        {
            case 1:
                ListingEditor le = new ListingEditor(); //TODO: Move to lc 
                le.EditListing(interactor, lc, listingId);
                break;
            case 2:
                lc.RemoveListing(interactor, listingId);
                break;
            case 3:
                break;
        }
        
        


        
        
        
        


    }
}