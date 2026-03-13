using System.Security;
using SecondHandMarket.Database;

namespace SecondHandMarket.MainMenu;

public class Login
{
    public static void UserLogin(Init db)
    {
        string username = ValidEntryChecker.GetValidUsername();
        string password = ValidEntryChecker.GetConsoleSecurePassword();

        if (db.CheckUserCredentials(username, password))
        {
            db.SetUserAsActive(username);
        }
    }

    public static void UserLogout(Init db)
    {
        db.SetUserAsInactive();
    }

}