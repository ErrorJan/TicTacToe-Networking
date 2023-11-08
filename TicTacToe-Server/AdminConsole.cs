namespace TicTacToe_Server;
using Spectre.Console;

class AdminConsole
{
    public event Action<Exception>? threadUnexpectadlyDied;

    public AdminConsole()
    {
        currentSession = Server.mainCoutSession;
        if ( spinUpConsoleTries > 0 )
        {
            spinUpConsoleTries--;
            consoleUpdateThread = new Thread( new ThreadStart( InterfaceLogic ) );
            consoleUpdateThread.Start();
            consoleUpdateThread.Name = "Admin Console";
        }
        else
        {
            Console.WriteLine( "ERROR: Console spun up more than 3 times, giving up..." );
            consoleUpdateThread = new Thread( new ThreadStart( () => {} ) ); // making the compiler happy
        }
    }

    public async Task SetThreadEnabledAsync( bool enabled )
    {
        tcs = new();
        if ( enabled )
            mre.Set();
        else
            mre.Reset();

        await tcs.Task;
    }

    public async Task RequestThreadKillAsync()
    {
        isThreadSupposedToRun = false;

        int wait = 100;
        while ( consoleUpdateThread.IsAlive && wait > 0 )
        {
            await Task.Delay( 10 );
            wait--;
        }

        if ( wait <= 0 )
        {
            throw new Exception( "Thread could not be killed!" );
        }
    }

    public void SetThreadEnabled( bool enabled )
    {
        SetThreadEnabledAsync( enabled ).Wait();
    }
    public void RequestThreadKill()
    {
        RequestThreadKillAsync().Wait();
    }

    private TaskCompletionSource? tcs;
    private bool isThreadSupposedToRun = true;
    private ManualResetEvent mre = new(false);
    private Thread consoleUpdateThread;
    private static int spinUpConsoleTries = 3;
    private Layout outputLayout = new( Text.Empty );
    private Layout inputLayout = new( Text.Empty );
    private string commandInput = "";
    private ConsoleOutSession currentSession;
    private int sessionOffset = 0;

    private void InterfaceStart( Layout layout )
    {
        layout.SplitRows
        (
            outputLayout,
            new Layout( new Rule( "Console Commmands" ).LeftJustified() ).Size( 1 ),
            inputLayout.Size( 1 )
        );
    }

    private void InterfaceUpdate( Layout layout )
    {
        while( Console.KeyAvailable )
        {
            var readKey = Console.ReadKey( true );
            switch ( readKey.Key )
            {
                case ConsoleKey.Enter:
                    currentSession.Debug( "Command typed: " + commandInput  );
                    commandInput = "";
                    break;
                case ConsoleKey.Backspace:
                    if ( commandInput.Length > 0)
                        commandInput = commandInput.Remove( commandInput.Length-1, 1 );
                    break;
                case ConsoleKey.PageUp:
                    if ( sessionOffset < currentSession.messagesLogged - (Console.WindowHeight-2) )
                        sessionOffset++;
                    break;
                case ConsoleKey.PageDown:
                    if ( sessionOffset > 0 )
                        sessionOffset--;
                    break;
                default:
                    if ( ( (int)readKey.Modifiers & ~(int)ConsoleModifiers.Shift ) == 0 )
                    {
                        if ( commandInput.Length < 50 )
                        {
                            commandInput += readKey.KeyChar;
                        }
                    }

                    if ( (readKey.Modifiers & ConsoleModifiers.Control) != 0 && readKey.Key == ConsoleKey.W )
                    {
                        int index = commandInput.LastIndexOf( ' ' );
                        if ( index == -1 )
                            index = 0;
                        index = commandInput.Length - index;
                        commandInput = commandInput.Remove( commandInput.Length - index, index);
                    }

                    break;
            }
        }

        outputLayout.Update( new Rows( currentSession.GetConsoleOutput( Console.WindowHeight-2, sessionOffset ) ) );
        inputLayout.Update( new Text( "> " + commandInput ) );
    }

    private void InterfaceEnd()
    {

    }

    private void InterfaceLogic()
    {
        try
        {
            var layout = new Layout("Root");
            InterfaceStart( layout );
            AnsiConsole.Live( layout )
                .AutoClear( false )
                .Start(ctx => 
                {
                    while ( isThreadSupposedToRun )
                    {
                        Thread.Sleep( 50 );
                        ctx.Refresh();
                        tcs?.SetResult();
                        tcs = null;
                        // mre.WaitOne();
 
                        InterfaceUpdate( layout );
                    }
                    InterfaceEnd();
                });
        }
        catch ( Exception e )
        {
            threadUnexpectadlyDied?.Invoke( e );
        }
    }
}