using System.Data;
using System.Data.SQLite;
using SecondHandMarket.Database;

namespace SecondHandMarket.MainMenu;

public class ListingHandler
{
    public static void CreateListing(Init db, ListingCollection lc)
    {
        Listings listing = lc.CreateListing(db);
        int userId = db.GetUserId();
        db.AddListingToDb(listing, userId);
    }

    public static void EditListingTitle(Init db, int listingId)
    {
        string newTitle = ValidEntryChecker.GetValidString(1, 20);
        SQLiteConnection myConn = db.GetConnection();
        string sql = $"UPDATE listings SET title = '{newTitle}' WHERE listingID = '{listingId}'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();
        myConn.Close();
    }

    public static void EditListingDescription(Init db, int listingId)
    {
        if (!db.IsSeller(listingId))
        {
            Console.WriteLine("You cannot edit this listing.");
        }

        string newDescription = ValidEntryChecker.GetValidString(1, 200);
        SQLiteConnection myConn = db.GetConnection();
        string sql = $"UPDATE listings SET description = '{newDescription}' WHERE listingID = '{listingId}'";
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();
        myConn.Close();
    }

}