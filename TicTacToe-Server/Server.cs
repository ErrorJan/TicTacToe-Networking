namespace TicTacToe_Server;

using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

class Server
{
    public static ConsoleCoutSession mainCoutSession { get; private set; } = new();
    public static bool debugMessagesEnabled = true;
    private static AdminConsole console = new( mainCoutSession );

    // Cross Thread Event Stuff
    private static AutoResetEvent eventRequest = new(false);
    private static AutoResetEvent eventLock = new(false);
    private static CrossThreadEventType eventRequestType;

    public static void EventRequest( CrossThreadEventType eventType )
    {
        eventLock.WaitOne();
        Server.eventRequestType = eventType;
        eventRequest.Set();
    }

    private static void Main(string[] args)
    {
        Thread.CurrentThread.Name = "Main Thread";
        mainCoutSession.Debug( String.Format( "Console Height: {0}, Console Width: {1}", Console.WindowHeight, Console.WindowWidth ) );
        TestStuff();

        console.SetThreadEnabled( true );

        // "Spin"
        HandleThreadEvents();
    }

    private static void HandleThreadEvents()
    {
    EventHandlerStart:
        eventLock.Set();
        eventRequest.WaitOne();

        switch( eventRequestType )
        {
            case CrossThreadEventType.AdminConsoleThreadDied:
                console = new( mainCoutSession );
                Task.Run( () => 
                { 
                    Console.WriteLine( "Console Crashed... Press any key to restart TUI." );
                    Console.ReadKey( true );
                    console.SetThreadEnabled( true );
                });
                goto EventHandlerStart;
            case CrossThreadEventType.Quit:
                // Cleanup
                console.RequestThreadKill();
                break;
        }
    }

    public enum CrossThreadEventType
    {
        AdminConsoleThreadDied,
        Quit
    }

    private static void TestStuff()
    {
        
    }
}

//https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/sockets/socket-services
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