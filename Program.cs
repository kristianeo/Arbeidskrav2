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
            Console.Write("\n=== Main Menu ===" +
                              "\n1. Create Listing" +
                              "\n2. Browse Listings" +
                              "\n3. Search Listings" +
                              "\n4. My Listings" +
                              "\n5. My Purchases" +
                              "\n6. My Reviews" +
                              "\n7. Log Out" +
                              "\n\nSelect an option: ");
            switch (ValidEntryChecker.GetValidInt(1, 7))
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
                    ui.PurchaseListing(interactor, listingCollection, userCollection);
                    ui.GoBackToMainMenu();
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("=== Search available listings by title or description ===");
                    ui.SearchListings(listingCollection);
                    ui.GoBackToMainMenu();
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine("=== My Listings ===");
                    ui.ViewOwnListings(interactor, listingCollection);
                    ui.GoBackToMainMenu();
                    break;
                case 5:
                    Console.Clear();
                    Console.WriteLine("=== Purchase history ===");
                    userCollection.ShowPurchaseHistory(interactor);
                    ui.GoBackToMainMenu();
                    break;
                case 6:
                    Console.Clear();
                    Console.WriteLine("=== My Reviews ===");
                    userCollection.ShowReviewHistory(interactor);
                    ui.GoBackToMainMenu();
                    break;
                case 7:
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