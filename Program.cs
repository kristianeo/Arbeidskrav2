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

        Console.WriteLine("===Second Hand Market ===");
        Console.WriteLine("\n1. Register" +
                          "\n2. Login" +
                          "\n3. Exit");
        switch (ValidEntryChecker.GetValidInt(1, 3))
        {
            case 1:
                userCollection.RegisterUser(userCollection, interactor);
                break;
            case 2:
                Login.UserLogin(interactor);
                break;
            case 3:
                Environment.Exit(0);
                break;
        }

    }
}