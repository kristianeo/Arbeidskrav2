using System.Security;

namespace SecondHandMarket.Database;
using System.Data.SQLite;

public class Init
{
    public void CreateDatabase()
    {
        SQLiteConnection.CreateFile("SecondHandMarketDB.sqlite");

    }

    private SQLiteConnection GetConnection()
    {
        
        SQLiteConnection myConn = new SQLiteConnection("Data Source=SecondHandMarketDB.sqlite;Version=3;");
        myConn.Open();
        return myConn;
    }

    public void CreateListingsTable()
    {
        SQLiteConnection myConn = GetConnection();

        string sql = "DROP TABLE IF EXISTS listings;" +
                     "CREATE TABLE IF NOT EXISTS listings(" +
                     "listingID INTEGER PRIMARY KEY," +
                     "sellerID INTEGER NOT NULL," +
                     "title TEXT NOT NULL," +
                     "description TEXT," +
                     "category TEXT NOT NULL," +
                     "itemCondition TEXT NOT NULL," +
                     "price INT NOT NULL," +
                     "status TEXT NOT NULL," +
                     "FOREIGN KEY(sellerID) REFERENCES users(userID))";
        
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();

        myConn.Close();
    }

    public int GetUserId(UserCollection userCollection)
    {
        SQLiteConnection myConn = GetConnection();
        string seller = GetActiveUser();

        string sql = $"SELECT userID FROM users WHERE username = '{seller}'";
        
        SQLiteCommand command = new SQLiteCommand(sql, myConn);

        //int userId = Convert.ToInt32(command.ExecuteScalar());

        Console.WriteLine("User id acquired.");
        int result = Convert.ToInt32(command.ExecuteScalar());
        myConn.Close();
        return result;

    }

    public void AddListingToDb(Listings listing, int userId)
    {
        SQLiteConnection myConn = GetConnection();

        string sql = "INSERT INTO listings(sellerID, title, description, category, itemCondition, price, status) VALUES (" +
                     $"'{userId}'," +
                     $"'{listing.Title}'," +
                     $"'{listing.Description}'," +
                     $"'{listing.Category}'," +
                     $"'{listing.Condition}'," +
                     $"'{listing.Price}'," +
                     $"'{listing.CurrentStatus}')";
        
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();

        myConn.Close();
        Console.WriteLine("Listing added to database");
    }

    public void CreateUserTable()
    {
        SQLiteConnection myConn = GetConnection();

        string sql = "DROP TABLE IF EXISTS users;" +
                     "CREATE TABLE IF NOT EXISTS users(" +
                     "userID INTEGER PRIMARY KEY NOT NULL," +
                     "username TEXT NOT NULL," +
                     "password TEXT NOT NULL," +
                     "activeStatus BOOL NOT NULL)";
        
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();
        
        myConn.Close();
    }

    public void AddUserToTable(User user)
    {
        SQLiteConnection myConn = GetConnection();
        
        string sql = "INSERT INTO users(username, password, activeStatus) VALUES (" +
                     $"'{user.Username}'," +
                     $"'{user.Password}'," +
                     "'1')";
        
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();

        myConn.Close();
        Console.WriteLine($"{user.Username} added to table.");
    }

    public string GetActiveUser()
    {
        SQLiteConnection myConn = GetConnection();

        string sql = "SELECT username FROM users WHERE activeStatus = 1";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        var activeUser = command.ExecuteScalar();
        
        myConn.Close();

        Console.WriteLine("Active user acquired.");
        
        return activeUser.ToString();
    }

    public bool CheckUserCredentials(string username, string password)
    {
        SQLiteConnection myConn = GetConnection();
        string sql = $"SELECT username FROM users WHERE username = '{username}'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        var check = command.ExecuteScalar();

        if (check == null)
        {
            Console.WriteLine("Invalid username");
            return false;
        }
        
        sql = $"SELECT password FROM users WHERE password = '{password}'";
        SQLiteCommand command2 = new SQLiteCommand(sql, myConn);
        var check2 = command2.ExecuteScalar();

        if (check2 == null)
        {
            Console.WriteLine("Invalid password");
            return false;
        }
        
        myConn.Close();
        return true;
    }

    public bool CheckIfAvailableUsername(string username)
    {
        SQLiteConnection myConn = GetConnection();
        string sql = "SELECT username FROM users WHERE username = 'input'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        var check = command.ExecuteScalar();

        if (check != null)
        {
            Console.WriteLine("The username is already taken.");
            return false;
        }
        myConn.Close();
        return true;
    }
    
    public void SetUserAsActive(string username)
    {
        SQLiteConnection myConn = GetConnection();
        string sql = $"UPDATE users SET activeStatus = 1 WHERE username = '{username}'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();
        myConn.Close();
    }

    public void SetUserAsInactive()
    {
        SQLiteConnection myConn = GetConnection();
        string sql = "UPDATE users SET activeStatus = 0";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();
        myConn.Close();
    }
    
}
