namespace SecondHandMarket;

class Program
{
    static void Main(string[] args)
    {
        Users user = Users.RegisterUser();
        Console.WriteLine(user.ToString());
    }
}