using System.Data.SQLite;
using System.Security;
using SecondHandMarket.Database;

namespace SecondHandMarket;

public class UserCollection
{
    private List<User> _usersList = new List<User>();

    /// <summary>
    /// 
    /// </summary>
    /// <returns>New instance of User</returns>
    public User RegisterUser()
    {
        string username = ValidEntryChecker.GetValidUsername();
        string password = ValidEntryChecker.GetConsoleSecurePassword();
        User user = new User(username, password);
        _usersList.Add(user);
        
        return user;
    }

    public string ShowPurchaseHistory(Init db)
    {
        int userId = db.GetActiveUserId();
        return "SELECT * FROM listings " +
               "JOIN users on listings.sellerID = users.userID " +
               $"WHERE buyer = '{userId}' ";
    }
    public string ShowSellerHistory(Init db)
    {
        int userId = db.GetActiveUserId();
        return "SELECT * FROM listings " +
               $"WHERE sellerID = '{userId}' " +
               "AND status = 'Sold'";    
    }

    public int GetSellerId(Init db, int listingId)
    {
        SQLiteConnection myConn = db.GetConnection();

        string sql = "SELECT sellerID FROM listings " +
                     $"WHERE listingID = '{listingId}'";
        
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        int sellerId = Convert.ToInt32(command.ExecuteScalar());

        myConn.Close();

        return sellerId;
    }

    public void LeaveReview(Init db, int listingsId)
    {
        Console.WriteLine("1. Very poor" +
                          "\n2. Poor" +
                          "\n3. Fair" +
                          "\n4. Good" +
                          "\n5. Very good" +
                          "\n6. Excellent");
        Console.Write("Please select review score: ");
        int score = ValidEntryChecker.GetValidInt(1, 6);
        
        SQLiteConnection myConn = db.GetConnection();
        string sql =  $"UPDATE users SET reviewScore = '{score}'," +
                      $"reviewsNumber = reviewsNumber + 1 " +
                      $"WHERE userID = '{GetSellerId(db, listingsId)}'"; 
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteScalar();

        myConn.Close();
    }


    /// <summary>
    /// Deprecated
    /// </summary>
    /// <returns></returns>
    public string GetActiveUser()
    {
        IEnumerable<string> seller =
            from users in _usersList
            where users.IsActive()
            select users.Username;

        return seller.ToString();
    }


}