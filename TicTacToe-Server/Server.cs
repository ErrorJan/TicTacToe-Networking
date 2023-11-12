namespace TicTacToe_Server;

using TicTacToe_Shared;

// READ ZIS
// https://intellitect.com/blog/legacy-system-threading/

class Server
{
    public static ConsoleCoutSession mainCoutSession { get; private set; } = new();
    public static bool debugMessagesEnabled = true;
    public static AdminConsole console { get; private set; } = new( mainCoutSession );
    public static ListenForNewClients newClientsListener { get; private set; } = new( StaticGameInfo.GAME_PORT );

    public static void EventRequest( CrossThreadEventType eventType )
    {
        eventLock.WaitOne();
        Server.eventRequestType = eventType;
        eventRequest.Set();
    }

    // Cross Thread Event Stuff
    private static AutoResetEvent eventRequest = new(false);
    private static AutoResetEvent eventLock = new(true);
    private static CrossThreadEventType eventRequestType;

    private static void Main(string[] args)
    {
        Thread.CurrentThread.Name = "Main Thread";
        mainCoutSession.Debug( String.Format( "Console Height: {0}, Console Width: {1}", Console.WindowHeight, Console.WindowWidth ) );
        TestStuff();

        console.SetThreadEnabled( true );

        // CrossThreadEvents handler
        HandleThreadEvents();
    }

    private static void HandleThreadEvents()
    {
        bool handleEvents = true;
        while( handleEvents )
        {
            eventLock.Set();
            eventRequest.WaitOne();
            switch( eventRequestType )
            {
                case CrossThreadEventType.AdminConsoleThreadDied:
                    console = new( mainCoutSession );
                    Console.WriteLine( "Console Crashed... Press any key to restart TUI." );
                    Console.ReadKey( true );
                    console.SetThreadEnabled( true );
                    break;
                case CrossThreadEventType.Quit:
                    // Cleanup
                    console.RequestThreadKill();
                    handleEvents = false;
                    break;
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