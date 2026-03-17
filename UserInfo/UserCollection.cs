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