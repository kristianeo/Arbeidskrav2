using System.Runtime.InteropServices.JavaScript;
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
                     "dateOfPurchase DATE," +
                     "FOREIGN KEY(sellerID) REFERENCES users(userID))";

        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();

        myConn.Close();
    }

    public void AddListingToTable(Listings listing, DbInteractor interactor)
    {
        int userId = interactor.GetActiveUserId();
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
            $"{DateTime.Now:yyyy-MM-dd})";

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
    }
    
    public void CreateReviewTable()
    {
        SQLiteConnection myConn = GetConnection();

        string sql = "DROP TABLE IF EXISTS reviews;" +
                     "CREATE TABLE IF NOT EXISTS reviews (" +
                     "reviewID INTEGER PRIMARY KEY," +
                     "userID INTEGER NOT NULL," +
                     "listingID INTEGER NOT NULL," +
                     "score INTEGER NOT NULL," +
                     "comment TEXT," +
                     "dateOfPurchase DATE," +
                     "FOREIGN KEY(userID) REFERENCES users(userID)," +
                     "FOREIGN KEY (listingID) REFERENCES listings(listingID))";

        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();

        myConn.Close();
    }
}