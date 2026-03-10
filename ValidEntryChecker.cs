using System.Security;

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
    /*
    public static SecureString GetValidPassword(SecureString password)
    {
        Console.Write("Password: ");
        while (true)
        {

            if (password.Any(char.IsAsciiLetterOrDigit) && password is { Length: >= 8 and <= 30 })
            {
                return password;
            }
            
            Console.Write("Password can only consist of letters A-Z and must be 8-30 characters long. Try again: ");
        }
    }
    */
    
    public static string GetValidString(int min, int max)
    {
        while (true)
        {
            string str = Console.ReadLine();

            if (str.Any(char.IsAsciiLetterOrDigit) && str.Length >= min && str.Length <= max)
            {
                return str;
            }
            
            Console.Write($"Text can only consist of letters A-Z and must be between {min} and {max} characters. Try again: ");
        }
    }
    public static SecureString GetConsoleSecurePassword( )
    {
        Console.Write("Password: ");
        SecureString pwd = new SecureString( );
        while ( true )
        {
            ConsoleKeyInfo i = Console.ReadKey( true );
            
            if ( i.Key == ConsoleKey.Enter )
            {
                break;
            }
            if (i.Key == ConsoleKey.Backspace)
            {
                //Prevent an exception when you hit backspace with no characters on the array.
                if (pwd.Length>0)
                {
                    pwd.RemoveAt(pwd.Length - 1);
                    Console.Write("\b \b");
                }
            }
            else
            {
                pwd.AppendChar( i.KeyChar );
                Console.Write( "*" );
            }
        }
        return pwd;
    }


}