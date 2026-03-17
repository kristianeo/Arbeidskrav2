using System.Security;

namespace SecondHandMarket.Database;
using System.Data.SQLite;

public class Init
{
    public void CreateDatabase()
    {
        SQLiteConnection.CreateFile("SecondHandMarketDB.sqlite");
    }

    public SQLiteConnection GetConnection()
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
                     "buyerID INTEGER," +
                     "FOREIGN KEY(sellerID) REFERENCES users(userID))";

        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();

        myConn.Close();
    }

    public int GetActiveUserId()
    {
        SQLiteConnection myConn = GetConnection();

        string sql = "SELECT userID FROM users WHERE activeStatus = 1";

        SQLiteCommand command = new SQLiteCommand(sql, myConn);

        int result = Convert.ToInt32(command.ExecuteScalar());
        myConn.Close();
        return result;

    }
    public int GetSellerId() //TODO: Where to use this?
    {
        SQLiteConnection myConn = GetConnection();
        int user = GetActiveUserId();

        string sql = "SELECT sellerID FROM listings " +
                     "JOIN users on listings.sellerID = users.userID " +
                     $"WHERE sellerID = '{user}'";

        SQLiteCommand command = new SQLiteCommand(sql, myConn);

        int result = Convert.ToInt32(command.ExecuteScalar());
        myConn.Close();
        return result;
    }

    public void AddListingToDb(Listings listing)
    {
        int userId = GetActiveUserId();
        SQLiteConnection myConn = GetConnection();

        string sql =
            "INSERT INTO listings(sellerID, title, description, category, itemCondition, price, status) VALUES (" +
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
                     "activeStatus BOOL NOT NULL," +
                     "reviewScore INTEGER NOT NULL," +
                     "reviewNumber INTEGER NOT NULL)";

        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();

        myConn.Close();
    }

    public void AddUserToTable(User user)
    {
        SQLiteConnection myConn = GetConnection();

        string sql = "INSERT INTO users(username, password, activeStatus, reviewScore, reviewsNumber) VALUES (" +
                     $"'{user.Username}'," +
                     $"'{user.Password}'," +
                     "'0'," +
                     "'0'," +
                     "'0')";

        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();

        myConn.Close();
        Console.WriteLine($"{user.Username} added to table.");
    }

    public string GetActiveUser() //TODO: make failsafe so there is only one active user 
    {
        SQLiteConnection myConn = GetConnection();

        string sql = "SELECT username FROM users WHERE activeStatus = 1";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        var activeUser = command.ExecuteScalar();

        myConn.Close();
        if (activeUser == null)
        {
            Console.WriteLine("You are not logged in.");
            return "";
        }
        
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

    public bool CheckIfAvailableUsername(string username) //TODO: implement this!
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

    public bool ShowListing(string sql)
    {
        bool exists = false;
        SQLiteConnection myConn = GetConnection();

        using SQLiteCommand readThis = new SQLiteCommand(sql, myConn);
        using (SQLiteDataReader dataReader = readThis.ExecuteReader())
        {
            while (dataReader.Read())
            {
                int id = Convert.ToInt32(dataReader["listingID"]);
                string? name = dataReader["username"].ToString();
                string? title = dataReader["title"].ToString();
                string? description = dataReader["description"].ToString();
                string? category = dataReader["category"].ToString();
                string? itemCondition = dataReader["itemCondition"].ToString();
                string? availableStatus = dataReader["status"].ToString();
                decimal price = Convert.ToDecimal(dataReader["price"]);

                Console.WriteLine(
                    $"{id} {name} {title} {description} {category} {itemCondition} {availableStatus} {price}");
                exists = true;
            }
        }
        myConn.Close();
        return exists;
    }

    public string ShowAllListings()
    {
        return "SELECT * FROM listings " +
               "JOIN users on listings.sellerID = users.userID " +
               "WHERE status = 'Available'";
    }

    public string ShowUserListings()
    {
        int sellerId = GetActiveUserId();
        return "SELECT * FROM listings " +
               "JOIN users on listings.sellerID = users.userID " +
               $"WHERE sellerID = '{sellerId}'";
    }
    /// <summary>
    /// shows all listings except current user 
    /// </summary>
    public string ShowOtherListings()
    {
        int userId = GetActiveUserId();
        return "SELECT * FROM listings " +
                     "JOIN users on listings.sellerID = users.userID " +
                     $"WHERE listings.sellerID != '{userId}' " +
                     $"AND status = 'Available'";
    }
    public string ShowListingById(int listingId)
    {
        return "SELECT * FROM listings " +
                     $"WHERE listingID = '{listingId}'";
    }

    public bool IsSeller(int listingId)//TODO: fix this with interface 
    {
        int userId =  GetActiveUserId();
        SQLiteConnection myConn = GetConnection();
        string sql = $"SELECT sellerID FROM listings " +
                     $"WHERE listingID = '{listingId}'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        var sellerId = Convert.ToInt32(command.ExecuteScalar());

        if (userId == sellerId)
        {
            return true;
        }
        myConn.Close();
        return false;
    }
}