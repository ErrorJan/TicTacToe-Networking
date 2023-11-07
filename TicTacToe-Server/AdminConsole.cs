namespace TicTacToe_Server;
using Spectre.Console;

class AdminConsole
{
    public AdminConsole()
    {
        consoleUpdateThread = new Thread( new ThreadStart( Loop ) );



        consoleUpdateThread.Start();
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

    public async void SetThreadEnabled( bool enabled )
    {
        await SetThreadEnabledAsync( enabled );
    }

    private TaskCompletionSource? tcs;
    private bool isThreadSupposedToRun = true;
    private ManualResetEvent mre = new(false);
    private Thread consoleUpdateThread;

    private void Loop()
    {
        // Synchronous
        var table = new Table().Centered();

        AnsiConsole.Live(table)
            .Start(ctx => 
            {
                while ( isThreadSupposedToRun )
                {
                    tcs?.SetResult();
                    mre.WaitOne();

                    

                    Thread.Sleep( 500 );
                    ctx.Refresh();
                }
            });
    }
}