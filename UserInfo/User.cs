namespace SecondHandMarket;

public class User:IActiveUser
{
    private string _username;
    private string _password;
    private int _score;
    
    public string Username => _username;
    public string Password => _password;
    public int Score => _score;

    public User(string username, string password)
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