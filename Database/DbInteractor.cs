using System.Data.SQLite;

namespace SecondHandMarket.Database;

public class DbInteractor:Init
{
    /// <summary>
    /// Checks the given username and password against the database.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns>True if both parameters are correct</returns>
    public bool CheckUserCredentials(string username, string password)
    {
        SQLiteConnection myConn = GetConnection();
        string sql = $"SELECT username FROM users WHERE username = '{username}'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        var check = command.ExecuteScalar();

        sql = $"SELECT password FROM users " +
              $"WHERE username = '{username}' AND password = '{password}'";
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
    /// <summary>
    /// When creating a new user, checks whether the username exists in the database to avoid duplicates.
    /// </summary>
    /// <param name="username"></param>
    /// <returns>True if the username is available</returns>
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
    /// <summary>
    /// Adds the created instance of a user to the database.
    /// </summary>
    /// <param name="user"></param>
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
    /// <summary>
    /// Used when logging in to set the user as active.
    /// </summary>
    /// <param name="username"></param>
    public void SetUserAsActive(string username)
    {
        SQLiteConnection myConn = GetConnection();
        string sql = $"UPDATE users SET activeStatus = 1 WHERE username = '{username}'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();
        myConn.Close();
    }
    /// <summary>
    /// When logging out, sets all users as inactive as a failsafe.
    /// </summary>
    public void SetUserAsInactive()
    {
        SQLiteConnection myConn = GetConnection();
        string sql = "UPDATE users SET activeStatus = 0";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();
        myConn.Close();
    }
    /// <summary>
    /// Shows all the listings for the active (logged in) user
    /// </summary>
    /// <returns></returns>
    public string ActiveUserListings()
    {
        int sellerId = GetActiveUserId();
        return "SELECT * FROM listings " +
               "JOIN users on listings.sellerID = users.userID " +
               $"WHERE sellerID = '{sellerId}' " +
               "AND status = 'Available'";
    }
    public string SoldUserListings()
    {
        int sellerId = GetActiveUserId();
        return "SELECT * FROM listings " +
               "JOIN users on listings.sellerID = users.userID " +
               $"WHERE sellerID = '{sellerId}' " +
               "AND status = 'Sold'";
    }
    /// <summary>
    /// Shows all listings except for the active (logged in) user 
    /// </summary>
    public string OthersListings()
    {
        int userId = GetActiveUserId();
        return "SELECT * FROM listings " +
                     "JOIN users on listings.sellerID = users.userID " +
                     $"WHERE listings.sellerID != '{userId}' " +
                     $"AND status = 'Available'";
    }
    /// <summary>
    /// Returns true if the active (logged in) user is the seller of chosen listing. 
    /// </summary>
    /// <param name="listingId"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Accesses the userID for the logged-in user
    /// </summary>
    /// <returns></returns>
    public int GetActiveUserId()
    {
        SQLiteConnection myConn = GetConnection();

        string sql = "SELECT userID FROM users WHERE activeStatus = 1";

        SQLiteCommand command = new SQLiteCommand(sql, myConn);

        int result = Convert.ToInt32(command.ExecuteScalar());
        myConn.Close();
        return result;

    }
    /// <summary>
    /// Returns the username of the logged-in user
    /// </summary>
    /// <returns></returns>
    public string GetActiveUsername()
    {
        SQLiteConnection myConn = GetConnection();

        string sql = "SELECT username FROM users WHERE activeStatus = 1";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        var activeUser = command.ExecuteScalar();

        myConn.Close();
        if (activeUser == null)//The program is written so there is always someone logged in
        {
            Console.WriteLine("You are not logged in.");
            return "";
        }
        
        return activeUser.ToString();
        
    }
    /// <summary>
    /// Adds the created instance of Listings to the database
    /// </summary>
    /// <param name="listing"></param>
    public void AddListingToTable(Listings listing)
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
    }
}