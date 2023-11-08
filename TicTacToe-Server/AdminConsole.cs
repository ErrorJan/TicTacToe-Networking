namespace TicTacToe_Server;
using Spectre.Console;

partial class AdminConsole
{
    public ConsoleCoutSession currentSession;

    public AdminConsole( ConsoleCoutSession startingSession )
    {
        currentSession = startingSession;
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

        int wait = 500;
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

    private static int spinUpConsoleTries = 3;
    private TaskCompletionSource? tcs;
    private bool isThreadSupposedToRun = true;
    private ManualResetEvent mre = new(false);
    private Thread consoleUpdateThread;
    private Layout outputLayout = new( Text.Empty );
    private Layout inputLayout = new( Text.Empty );
    private string commandInput = "";
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
                    string[] commandFormatted = commandInput.Split( ' ' );
                    string[] args = new string[ commandFormatted.Length - 1 ];
                    Array.Copy( commandFormatted, 1, args, 0, args.Length );

                    bool commandExists = false;
                    foreach ( ICommand command in commands )
                    {
                        if ( command.CommandExecuted( commandFormatted[0], args, this ) )
                            commandExists = true;
                    }

                    if ( !commandExists )
                    {
                        currentSession.Error( $"Unknown Command \"{commandFormatted[0]}\"" );
                    }

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
                        tcs?.SetResult();
                        tcs = null;
                        mre.WaitOne();
                        ctx.Refresh();
 
                        InterfaceUpdate( layout );
                    }
                    InterfaceEnd();
                });
        }
        catch ( Exception e )
        {
            Console.WriteLine( e.ToString() );
            Server.EventRequest( Server.CrossThreadEventType.AdminConsoleThreadDied );
        }
    }
}