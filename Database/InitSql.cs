namespace SecondHandMarket.Database;
using System.Data.SQLite;

public class InitSql
{
    public void CreateDatabase()
    {
        SQLiteConnection.CreateFile("SecondHandMarketDB.sqlite");

        SQLiteConnection myConn = new SQLiteConnection("Data Source=SecondHandMarketDB.sqlite;Version=3;");
        myConn.Open();

        string sql = "CREATE TABLE IF NOT EXISTS listings(" +
                     "listingID INT PRIMARY KEY," +
                     "seller VARCHAR(30)," +
                     "title VARCHAR(30)," +
                     "description VARCHAR(200)," +
                     "category VARCHAR(200)," +
                     "itemCondition VARCHAR(20)," +
                     "price INT," +
                     "status varchar(20))";
        
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();

        myConn.Close();
    }

    public void CreateTables()
    {
        /*
        SQLiteConnection myConn = new SQLiteConnection("Data Source=SecondHandMarketDB.sqlite;Version=3;");
        myConn.Open();

        string sql = "INSERT INTO listings(" +
                     "listingID INT PRIMARY KEY," +
                     "seller VARCHAR(30)," +
                     "title VARCHAR(30)," +
                     "description VARCHAR(200)," +
                     "category VARCHAR(200)," +
                     "itemCondition VARCHAR(20)," +
                     "price INT," +
                     "status varchar(20))";
        
        SQLiteCommand command = new SQLiteCommand(sql, myConn);
        command.ExecuteNonQuery();

        myConn.Close();
        */
    }
    
}
