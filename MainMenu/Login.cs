using System.Security;
using SecondHandMarket.Database;

namespace SecondHandMarket.MainMenu;

public class Login
{
    public static void UserLogin(DbInteractor interactor)
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

    public static void UserLogout(DbInteractor interactor)
    {
        interactor.SetUserAsInactive();
    }

}