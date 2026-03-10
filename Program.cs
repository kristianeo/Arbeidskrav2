namespace SecondHandMarket;

class Program
{
    static void Main(string[] args)
    {
        Users user = Users.RegisterUser();
        Console.WriteLine(user.ToString());
        Listings listings = ListingCreator.CreateListing(user);
        user.AddListing(listings);
        Console.WriteLine(listings.ToString());
    }
}