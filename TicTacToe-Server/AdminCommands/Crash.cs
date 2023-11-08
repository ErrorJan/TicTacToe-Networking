namespace TicTacToe_Server;

class CrashCommand : ICommand
{
    public bool CommandExecuted( string commandName, string[] args, AdminConsole adminConsole )
    {
        if ( commandName == "crash_adminconsole" )
        {
            throw new Exception( "Crashing the AdminConsole" );
        }

        return false;
    }
}