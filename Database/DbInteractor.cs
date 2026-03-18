using System.Data.SQLite;

namespace SecondHandMarket.Database;

public class DbInteractor
{
    public SQLiteConnection GetConnection()
    {
        SQLiteConnection myConn = new SQLiteConnection("Data Source=SecondHandMarketDB.sqlite;Version=3;");
        myConn.Open();
        return myConn;
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
    }
    
    public bool CheckUserCredentials(string username, string password)
    {
        SQLiteConnection myConn = GetConnection();
        string sql = $"SELECT username FROM users WHERE username = '{username}'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        var check = command.ExecuteScalar();

        sql = $"SELECT password FROM users WHERE password = '{password}'";
        SQLiteCommand command2 = new SQLiteCommand(sql, myConn);
        var check2 = command2.ExecuteScalar();

        if (check == null || check2 == null)
        {
            Console.WriteLine("Invalid username or password");
            return false;
        }

        myConn.Close();
        return true;
    }

    public bool CheckIfAvailableUsername(string username)
    {
        SQLiteConnection myConn = GetConnection();
        string sql = $"SELECT username FROM users WHERE username = '{username}'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        var check = command.ExecuteScalar();

        if (check != null)
        {
            Console.WriteLine("The username is already taken.");
            myConn.Close();
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
        Console.WriteLine("  #  Title                 Category     Condition  Price");
        bool exists = false;
        SQLiteConnection myConn = GetConnection();

        using SQLiteCommand readThis = new SQLiteCommand(sql, myConn);
        using (SQLiteDataReader dataReader = readThis.ExecuteReader())
        {
            while (dataReader.Read())
            {
                int id = Convert.ToInt32(dataReader["listingID"]);
                //string? name = dataReader["username"].ToString();
                string? title = dataReader["title"].ToString();
                //string? description = dataReader["description"].ToString();
                string? category = dataReader["category"].ToString();
                string? itemCondition = dataReader["itemCondition"].ToString();
                //string? availableStatus = dataReader["status"].ToString();
                decimal price = Convert.ToDecimal(dataReader["price"]);

                Console.WriteLine(
                    $"  {id.ToString(),-2} {title,-21} {category,-12} {itemCondition,-10} {price} kr");
                exists = true;
            }
        }
        //TODO:Make choose input readkey, no need for enter 
        myConn.Close();
        return exists;
    }

    public string AllListings()
    {
        return "SELECT * FROM listings " +
               "JOIN users on listings.sellerID = users.userID " +
               "WHERE status = 'Available'";
    }

    public string ActiveUserListings()
    {
        int sellerId = GetActiveUserId();
        return "SELECT * FROM listings " +
               "JOIN users on listings.sellerID = users.userID " +
               $"WHERE sellerID = '{sellerId}'";
    }
    /// <summary>
    /// shows all listings except current user 
    /// </summary>
    public string OthersListings()
    {
        int userId = GetActiveUserId();
        return "SELECT * FROM listings " +
                     "JOIN users on listings.sellerID = users.userID " +
                     $"WHERE listings.sellerID != '{userId}' " +
                     $"AND status = 'Available'";
    }
    public string ShowListingById(int listingId) // TODO: Change to look different 
    {
        return "SELECT * FROM listings " +
               "JOIN users on listings.sellerID = users.userID " +
               $"WHERE listingID = '{listingId}'";
    }

    public bool IsSeller(int listingId)
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
    
    public int GetActiveUserId()
    {
        SQLiteConnection myConn = GetConnection();

        string sql = "SELECT userID FROM users WHERE activeStatus = 1";

        SQLiteCommand command = new SQLiteCommand(sql, myConn);

        int result = Convert.ToInt32(command.ExecuteScalar());
        myConn.Close();
        return result;

    }
    public string GetActiveUser() //TODO: Where is this used?
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
    public int GetSellerId()
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
    public void AddListingToTable(Listings listing)
    {
        int userId = GetActiveUserId();
        SQLiteConnection myConn = GetConnection();

        string sql =
            "INSERT INTO listings(sellerID, title, description, category, itemCondition, price, status, dateOfPurchase) VALUES (" +
            $"'{userId}'," +
            $"'{listing.Title}'," +
            $"'{listing.Description}'," +
            $"'{listing.Category}'," +
            $"'{listing.Condition}'," +
            $"'{listing.Price}'," +
            $"'{listing.CurrentStatus}'," +
            $"'{DateTime.Now:yyyy-MM-dd}')";

        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();

        myConn.Close();
    }
}