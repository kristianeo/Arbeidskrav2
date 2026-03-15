using System.Data.SQLite;

namespace SecondHandMarket.Database;

public class ListingsDb:Init
{
    /*
    public static void InactivateListing(Init db)
    {
        SQLiteConnection myConn = db.GetConnection();
        string sql = $"UPDATE listings SET status = 0 WHERE listingID = " +
                     $"JOIN users ON listings.sellerId = users.userId";

        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();

        myConn.Close();
    }
    
    public static int SelectListing(Init db)//TODO: Rather show a list of listings and chose from those
    {
        SQLiteConnection myConn = db.GetConnection();
        foreach (Listings listings in )
        sql = $"SELECT listingID FROM listings "+
                     $"JOIN users ON users.userId = listings.sellerId " +
                     $"WHERE title = '{}' and users.activeStatus = 1";
    }
    */
}