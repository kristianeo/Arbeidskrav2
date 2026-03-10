using System.Security;

namespace SecondHandMarket.MainMenu;

public class Login
{
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