namespace TicTacToe_Server;

class QuitCommand : ICommand
{
    public bool CommandExecuted( string commandName, string[] args, AdminConsole adminConsole )
    {
        if ( commandName == "quit" )
        {
            adminConsole.currentSession.Info( "Quitting Program..." );
            Server.EventRequest( Server.CrossThreadEventType.Quit );
            return true;
        }

        return false;
    }
}