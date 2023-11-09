namespace TicTacToe_Server;

using TicTacToe_Shared;

class Server
{
    public static ConsoleCoutSession mainCoutSession { get; private set; } = new();
    public static bool debugMessagesEnabled = true;
    public static AdminConsole console { get; private set; } = new( mainCoutSession );
    public static ListenForNewClients newClientsListener { get; private set; } = new( SharedGameInfo.GAME_PORT );

    public static void EventRequest( CrossThreadEventType eventType )
    {
        eventLock.WaitOne();
        Server.eventRequestType = eventType;
        eventRequest.Set();
    }

    // Cross Thread Event Stuff
    private static AutoResetEvent eventRequest = new(false);
    private static AutoResetEvent eventLock = new(false);
    private static CrossThreadEventType eventRequestType;

    private static void Main(string[] args)
    {
        Thread.CurrentThread.Name = "Main Thread";
        mainCoutSession.Debug( String.Format( "Console Height: {0}, Console Width: {1}", Console.WindowHeight, Console.WindowWidth ) );
        TestStuff();

        console.SetThreadEnabled( true );

        // CrossThreadEvents handler
        Task.Run( HandleThreadEvents );
    }

    private static async Task HandleThreadEvents()
    {
        bool handleEvents = true;
        while( handleEvents )
        {
            if ( eventRequest.WaitOne( 1 ) )
            {
                switch( eventRequestType )
                {
                    case CrossThreadEventType.AdminConsoleThreadDied:
                        console = new( mainCoutSession );
                        Console.WriteLine( "Console Crashed... Press any key to restart TUI." );
                        Console.ReadKey( true );
                        await console.SetThreadEnabledAsync( true );
                        break;
                    case CrossThreadEventType.Quit:
                        // Cleanup
                        console.RequestThreadKill();
                        handleEvents = false;
                        break;
                }

                eventLock.Set();
            }
            else
            {
                await Task.Delay( 10 );
            }
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