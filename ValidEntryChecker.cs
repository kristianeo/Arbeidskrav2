namespace SecondHandMarket;

public abstract class ValidEntryChecker
{
    public static int GetValidInt(int min, int max)
    {
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int choice)
                && choice >= min && choice <= max)
            {
                return choice;
            }
            Console.Write("Invalid selection. Try again: ");
        }
    }
    
    public static string GetValidUsername()
    {
        Console.Write("Username: ");
        while (true)
        {
            string username = Console.ReadLine();

            if (username.Any(char.IsAsciiLetter) && username is { Length: >= 2 and <= 30 })
            {
                return username;
            }
            
            Console.Write("Username can only consist of letters A-Z and be 2-30 characters long. Try again: ");
        }
    }
    
    public static string GetValidPassword()
    {
        Console.Write("Password: ");
        while (true)
        {
            string password = Console.ReadLine();

            if (password.Any(char.IsAsciiLetterOrDigit) && password is { Length: >= 8 and <= 30 })
            {
                return password;
            }
            
            Console.Write("Password can only consist of letters A-Z and must be 8-30 characters long. Try again: ");
        }
    }


}