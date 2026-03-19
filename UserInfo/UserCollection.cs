using System.Collections.Concurrent;
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
    public User CreateUserInstance(DbInteractor interactor)
    {
        Start:
        string username = ValidEntryChecker.GetValidUsername(interactor);
        if (!interactor.CheckIfAvailableUsername(username))
        {
            goto Start;
        }
        string password = ValidEntryChecker.GetConsoleSecurePassword();
        User user = new User(username, password);
        _usersList.Add(user);
        
        return user;
    }

    public string ShowPurchaseHistory(DbInteractor interactor)
    {
        int userId = interactor.GetActiveUserId();
        return "SELECT * FROM listings " +
               "JOIN users on listings.sellerID = users.userID " +
               $"WHERE buyerID = '{userId}' ";
    }
    public string ShowSellerHistory(DbInteractor interactor)
    {
        int userId = interactor.GetActiveUserId();
        return "SELECT * FROM listings " +
               $"WHERE sellerID = '{userId}' " +
               "AND status = 'Sold'";    
    }

    private int GetSellerId(DbInteractor interactor, int listingId)
    {
        SQLiteConnection myConn = interactor.GetConnection();

        string sql = "SELECT sellerID FROM listings " +
                     $"WHERE listingID = '{listingId}'";
        
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        int sellerId = Convert.ToInt32(command.ExecuteScalar());

        myConn.Close();

        return sellerId;
    }

    public void LeaveReview(DbInteractor interactor, int listingId)
    {
        Console.Write("\nDo you wish to leave a review? " +
                      "\n1. Yes" +
                      "\n2. No" +
                      "\nSelect an option: ");
        if (ValidEntryChecker.GetValidInt(1, 2) == 2) return;
        
        Console.Write("\nRating (1-6): ");
        int score = ValidEntryChecker.GetValidInt(1, 6);
        Console.Write("Comment (or press enter to skip): ");
        string comment = ValidEntryChecker.GetValidString(0, 150);
        
        SQLiteConnection myConn = interactor.GetConnection();
        string sql =  $"INSERT INTO reviews(sellerID, listingID, score, comment, dateOfPurchase) VALUES (" +
                      $"'{GetSellerId(interactor, listingId)}'," +
                      $"'{listingId}'," +
                      $"'{score}'," +
                      $"'{comment}'," +
                      $"'{DateTime.Now:yyyy-MM-dd}')"; 
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteScalar();
        
        myConn.Close();
    }
    public bool ShowReviewHistory(DbInteractor interactor)
    {
        bool exists = false;
        SQLiteConnection myConn = interactor.GetConnection();
        string sql = $"SELECT * FROM reviews " +
                     $"JOIN listings ON reviews.listingID = listings.listingID " +
                     $"WHERE reviews.sellerID = '{interactor.GetActiveUserId()}'";

        using SQLiteCommand readThis = new SQLiteCommand(sql, myConn);
        using (SQLiteDataReader dataReader = readThis.ExecuteReader())
        {
            while (dataReader.Read())
            {
                string? title = dataReader["title"].ToString();
                int score = Convert.ToInt32(dataReader["score"]);
                DateTime date = Convert.ToDateTime(dataReader["dateOfPurchase"]);
                string? comment = dataReader["comment"].ToString();

                Console.WriteLine(
                    $"Title: {title,15} " +
                    $"\nScore: {score.ToString(),15} " +
                    $"\nComment: {comment,13} " +
                    $"\nDate: {date,16:yyyy-MM-dd}" +
                    $"\n");
                exists = true;
            }
        }
        myConn.Close();
        return exists;
    }
    public void RegisterUser(UserCollection uc, DbInteractor interactor)
    {
        User user = uc.CreateUserInstance(interactor);
        interactor.AddUserToTable(user);
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