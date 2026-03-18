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
        Console.Write("Select a listing: ");
        int listingId = ValidEntryChecker.GetValidInt(1, 200);
        if (!interactor.ShowListing(interactor.ShowListingById(listingId)))
        {
            Console.WriteLine("Invalid listingID.");
            goto Start;
        }

        Console.Write("Purchase this listing?" +
                      "\n1. Yes" +
                      "\n2. No");
        if (ValidEntryChecker.GetValidInt(1, 2) != 1) return;
        lc.Purchase(interactor, listingId);
        uc.LeaveReview(interactor, listingId);
    }
}