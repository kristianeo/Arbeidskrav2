using System.Security;

namespace SecondHandMarket;

public class UserCollection
{
    private List<Users> _usersList = new List<Users>();

    public Users RegisterUser() //TODO: Change back to secure password 
    {
        string username = ValidEntryChecker.GetValidUsername();
        SecureString password = ValidEntryChecker.GetConsoleSecurePassword();
        
        Users user = new Users(username, password);
        _usersList.Add(user);
        return user;
    }

    public string GetActiveUser()
    {
        IEnumerable<string> seller =
            from users in _usersList
            where users.IsActive()
            select users.Username;

        return seller.ToString();
    }


}