using SecondHandMarket.Database;

namespace SecondHandMarket.MainMenu;

public class UserCreator
{
    public static void CreateUser(UserCollection uc, Init db)
    {
        User user = uc.RegisterUser();
        db.AddUserToTable(user);
    }
}