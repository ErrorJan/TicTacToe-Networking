namespace TicTacToe_Server;
using Spectre.Console;

class AdminConsole
{
    public event Action<Exception>? threadUnexpectadlyDied;

    public AdminConsole()
    {
        if ( spinUpConsoleTries > 0 )
        {
            spinUpConsoleTries--;
            consoleUpdateThread = new Thread( new ThreadStart( InterfaceLogic ) );
            consoleUpdateThread.Start();
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
            Console.WriteLine(wait);
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
    string commandInput;

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
        commandInput += Console.In.ReadLine();

        while( Console.In.Peek() != -1 )
        {
            commandInput += Console.In.Read();
        }

        inputLayout.Update( new Text( commandInput ) );
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
                .Start(ctx => 
                {
                    while ( isThreadSupposedToRun )
                    {
                        Thread.Sleep( 20 );
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