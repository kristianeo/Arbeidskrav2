using System.Security;

namespace SecondHandMarket;

public class PremadeListings
{
    public static void Premade()

    {
    Listings _listings = new Listings(new Users("Marit", "passord123"), "Joggesko", "Hei",
        Listings.Categories.SportsAndOutdoors, "Fair", 430);

    Listings _listings2 = new Listings(new Users("Linda", "passord123"), "MacBook", "Ødelagt",
        Listings.Categories.Electronics, "Fair", 43000);

    Listings _listings3 = new Listings(new Users("John", "passord123"), "Sataniske vers", "",
        Listings.Categories.BooksAndMedia, "Good", 40);
    
    Listings.ListingsList.Add(_listings);
    Listings.ListingsList.Add(_listings2);
    Listings.ListingsList.Add(_listings3);
    }

}