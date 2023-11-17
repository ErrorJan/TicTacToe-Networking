namespace TicTacToe_Server;

partial class AdminConsole
{
    public ICommand[] commands { get; private set; } = 
    { 
        new QuitCommand(),
        new CrashCommand(),
        new HelpCommand(),
        new DebugEnabledCommand(),
        new SwitchOutputCommand(),
    };
}