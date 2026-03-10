using System.Globalization;

namespace SecondHandMarket;

public class Users
{
    private string _username;
    private string _password;
    
    public string Username => _username;
    public string Password => _password;
    
    private List<Listings> _listings;

    // private List<Users> _userList = []; rather code for each instance of Users class.....
    public Users(string username, string password)
    {
        _username = username;
        _password = password;
    }
    
    public static Users RegisterUser() //TODO: Add secure password thingy
    {
        string username = ValidEntryChecker.GetValidUsername();
        string password = ValidEntryChecker.GetValidPassword();
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
}