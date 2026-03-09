namespace SecondHandMarket;

public class Users
{
    private string _username;
    private string _password;
    
    public string Username => _username;
    public string Password => _password;

    // private List<Users> _userList = []; rather code for each instance of Users class.....
    private Users(string username, string password)
    {
        _username = username;
        _password = password;
    }

    public static Users CreateUser(string username, string password)
    {
        return new Users(username, password);
    }
    
}