using System.Security;
using SecondHandMarket.Database;

namespace SecondHandMarket.MainMenu;

public class Login
{
    public static void UserLogin(DbInteractor interactor)
    {
        string username = ValidEntryChecker.GetValidUsername();
        string password = ValidEntryChecker.GetConsoleSecurePassword();

        if (interactor.CheckUserCredentials(username, password))
        {
            interactor.SetUserAsActive(username);
        }
    }

    public static void UserLogout(DbInteractor interactor)
    {
        interactor.SetUserAsInactive();
    }

}