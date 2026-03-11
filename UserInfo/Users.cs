using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security;
using SecondHandMarket.MainMenu;

namespace SecondHandMarket;

public class Users:IActiveUser
{
    private string _username;
    private string _password;
    
    public string Username => _username;
    public string Password => _password;

    public static List<Users> _users = new List<Users>();

    // private List<Users> _userList = []; rather code for each instance of Users class.....
    public Users(string username, string password)
    {
        _username = username;
        _password = password;
        _users.Add(new Users(username, password));
    }
    
    public static Users RegisterUser() //TODO: Add secure password thingy
    {
        string username = ValidEntryChecker.GetValidUsername();
        //SecureString password = ValidEntryChecker.GetConsoleSecurePassword();
        
        string password = ValidEntryChecker.GetValidString(8, 30);
        return new Users(username, password);
    }

    public override string ToString()
    {
        return "Username: " + _username;
    }

    public bool IsActive()
    {
        return true;
    }
}