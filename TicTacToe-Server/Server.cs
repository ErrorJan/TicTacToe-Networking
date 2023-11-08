namespace TicTacToe_Server;

using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

//https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/sockets/socket-services
class Server
{
    public static ConsoleOutSession mainCoutSession { get; private set; } = new();
    private static AdminConsole console = new();

    private static void HandleAdminConsoleThreadDying( Exception exception )
    {
        Console.WriteLine( exception.ToString() );
        console = new();
        console.threadUnexpectadlyDied += HandleAdminConsoleThreadDying;
        #pragma warning disable CS4014
        console.SetThreadEnabledAsync( true );
        #pragma warning restore CS4014
    }

    private static void Main(string[] args)
    {
        Thread.CurrentThread.Name = "Main Thread";
        mainCoutSession.Debug( String.Format( "Console Height: {0}, Console Width: {1}", Console.WindowHeight, Console.WindowWidth ) );
        TestStuff();
        /*IPEndPoint ipEndPoint = new ( IPAddress.Any, 2030 );
        Socket listener = new ( ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp );
        Console.WriteLine( ipEndPoint.AddressFamily );

        listener.Bind( ipEndPoint );
        listener.Listen( 20 );

        while ( true )
        {
            Console.WriteLine( "Waiting..." );
            var handler = listener.Accept();
            Console.WriteLine( "Connected!" );
            var testMsg = "Test";
            var testMsgBytes = Encoding.UTF8.GetBytes( testMsg );
            handler.Send( testMsgBytes, SocketFlags.None );
            Console.WriteLine( $"Send test message back to {handler.RemoteEndPoint} | {handler.LocalEndPoint}" );
            handler.Close();
            Console.WriteLine( "Disconnected!" );
        }*/

        console.threadUnexpectadlyDied += HandleAdminConsoleThreadDying;
        console.SetThreadEnabled( true );
    }

    private static void TestStuff()
    {
        mainCoutSession.Info( "This is an Info message" );
        mainCoutSession.Debug( "This is a Debug message" );
        mainCoutSession.Error( "This is an Error message" );
        mainCoutSession.Warning( "This is a Warning message" );
    }
}
