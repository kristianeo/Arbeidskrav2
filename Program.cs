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

        ui.Logout(interactor);
        Console.Clear();
        Console.WriteLine("===Second Hand Market ===");
        Console.Write("\n1. Register" +
                      "\n2. Login" +
                      "\n3. Exit" +
                      "\n\nSelect an option: ");
        switch (ValidEntryChecker.GetValidInt(1, 3))
        {
            case 1:
                Console.Clear();
                Console.WriteLine("=== Register ===\n");
                userCollection.RegisterUser(userCollection, interactor);
                break;
            case 2:
                Console.Clear();
                Console.WriteLine("=== Login ===");
                ui.Login(interactor);
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
                    Console.WriteLine("=== Create Listing ===\n");
                    listingCollection.CreateListing(interactor, listingCollection);
                    ui.GoBackToMainMenu();
                    break;
                case 2:
                    ui.Browse(interactor, userCollection);
                    break;
                case 3:
                    ui.Search(interactor, listingCollection, userCollection);
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine("=== My Profile ===\n");
                    ui.ShowUserProfile(interactor, userCollection, listingCollection);
                    break;
                case 5:
                    Console.Clear();
                    ui.Logout(interactor);
                    Console.WriteLine("\nThank you for shopping with us. Welcome back.");
                    Thread.Sleep(3000);
                    Environment.Exit(0);
                    break;
            }
        }
    }
}