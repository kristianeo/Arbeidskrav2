using System.Security;
using SecondHandMarket.Database;

namespace SecondHandMarket.MainMenu;

public class Login
{
    public static void UserLogin(Init db)
    {
        string username = ValidEntryChecker.GetValidUsername();
        string password = ValidEntryChecker.GetConsoleSecurePassword();

        bool login = db.CheckUserCredentials(username, password);
        if (login)
        {
            db.SetUserAsActive(username);
        }
    }

    public static void UserLogout(Init db)
    {
        db.SetUserAsInactive();
    }

}