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

    private List<Listings> _listings = new List<Listings>();

    // private List<Users> _userList = []; rather code for each instance of Users class.....
    public Users(string username, string password)
    {
        _username = username;
        _password = password;
    }
    
    public static Users RegisterUser() //TODO: Add secure password thingy
    {
        string username = ValidEntryChecker.GetValidUsername();
        //SecureString password = ValidEntryChecker.GetConsoleSecurePassword();
        
        string password = ValidEntryChecker.GetValidString(8, 30);
        return new Users(username, password);
    }

    public void AddListing(Listings listing)
    {
        _listings.Add(listing);
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