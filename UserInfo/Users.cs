using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security;
using SecondHandMarket.MainMenu;

namespace SecondHandMarket;

public class Users:IActiveUser
{
    private string _username;
    private SecureString _password;
    
    public string Username => _username;
    public SecureString Password => _password;

    public Users(string username, SecureString password)
    {
        _username = username;
        _password = password;
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