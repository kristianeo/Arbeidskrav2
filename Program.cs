using System.Data.SQLite;
using SecondHandMarket.Database;
using SecondHandMarket.MainMenu;

namespace SecondHandMarket;

class Program
{
    static void Main(string[] args)
    {
        DbInteractor interactor = new DbInteractor();
        ListingCollection listingCollection = new ListingCollection();
        UserCollection userCollection = new UserCollection();
        UserInterface ui = new UserInterface();

        Login.UserLogout(interactor);

        Console.WriteLine("===Second Hand Market ===");
        Console.Write("\n1. Register" +
                          "\n2. Login" +
                          "\n3. Exit" +
                          "\n\nSelect an option: ");
        switch (ValidEntryChecker.GetValidInt(1, 3))
        {
            case 1:
                Console.Clear();
                Console.WriteLine("=== Register ===");
                userCollection.RegisterUser(userCollection, interactor);
                break;
            case 2:
                Console.Clear();
                Console.WriteLine("=== Login ===");
                Login.UserLogin(interactor);
                break;
            case 3:
                Environment.Exit(0);
                break;
        }

        while (true)
        {
            Console.Clear();
            Console.Write("=== Main Menu ===\n" +
                              "\n1. Create Listing" +
                              "\n2. Browse Listings" +
                              "\n3. Search Listings" +
                              "\n4. My Profile" +
                              "\n5. Log Out" +
                              "\n\nSelect an option: ");
            switch (ValidEntryChecker.GetValidInt(1, 5))
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("=== Create Listing ===");
                    ui.CreateListing(interactor, listingCollection);
                    ui.GoBackToMainMenu();
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("=== Available listings ===");
                    interactor.ShowListing(interactor.OthersListings());
                    ui.PurchaseListing(interactor, listingCollection, userCollection);
                    ui.GoBackToMainMenu();
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("=== Search ===");
                    ui.SearchListings(listingCollection, interactor);
                    ui.PurchaseListing(interactor, listingCollection, userCollection);
                    ui.GoBackToMainMenu();
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine("=== My Profile ===");
                    ui.ViewOwnListings(interactor, listingCollection);
                    ui.GoBackToMainMenu();
                    Console.Clear();
                    Console.WriteLine("=== Purchase history ===");
                    interactor.ShowListing(userCollection.ShowPurchaseHistory(interactor));
                    ui.GoBackToMainMenu();
                    Console.Clear();
                    Console.WriteLine("=== My Reviews ===");
                    userCollection.ShowReviewHistory(interactor);
                    ui.GoBackToMainMenu();
                    break;
                case 5:
                    Console.Clear();
                    Login.UserLogout(interactor);
                    Console.WriteLine("Thank you for shopping with us. Welcome back.");
                    Thread.Sleep(3000);
                    Environment.Exit(0);
                    break;
            }
            
        }

    }
}