using SecondHandMarket.Database;

namespace SecondHandMarket.MainMenu;

public class UserInterface
{
    public void CreateListing(DbInteractor interactor, ListingCollection lc)
    {
        Listings listing = ListingGenerator.GenerateListing(interactor, lc);
        interactor.AddListingToTable(listing);
    }
}