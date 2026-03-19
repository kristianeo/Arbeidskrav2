using System.Globalization;
using System.Security;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using SecondHandMarket.Database;

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
    
    public static string GetValidUsername(DbInteractor interactor)
    {
        while (true)
        {
            Console.Write("Username: ");
            string username = GetValidString(4, 20);
            
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

            if (str.Length == 0 && min == 0)
            {
                return "";
            }
            
            Console.Write($"Text can only consist of letters A-Z and must be between {min} and {max} characters. Try again: ");
        }
    }

/// <summary>
/// Encrypts password for storage in database. Returns the same encryption when the same password is entered again,
/// to make it easier to compare to the database.
/// </summary>
/// <param name="pwd"></param>
/// <returns>hashed password</returns>
    private static string GetHashedPwd(string pwd)
    {
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: pwd,
            salt: [8, 2, 3, 2, 5, 4, 7, 6, 4, 2, 8, 13, 68, 43, 56, 54, 32, 5, 76, 32, 54, 98, 65, 32, 54, 65],
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        return hashed;
    }
    public static string GetConsoleSecurePassword( )
    {
        Console.Write("Password: ");
        string pwd = "";
        while ( true )
        {
            ConsoleKeyInfo i = Console.ReadKey( true );
            
            if ( i.Key == ConsoleKey.Enter )
            {
                break;
            }
            if (i.Key == ConsoleKey.Backspace)
            {
                if (pwd.Length>0)
                {
                    pwd = pwd.Remove(pwd.Length-1);
                    Console.Write("\b \b");
                }
            }
            else
            {
                pwd += i.KeyChar;
                Console.Write( "*" );
            }
        }

        Console.WriteLine();
        return GetHashedPwd(pwd);
    }


}