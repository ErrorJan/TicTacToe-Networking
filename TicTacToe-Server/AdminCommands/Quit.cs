namespace TicTacToe_Server;

class QuitCommand : ICommand, IHelp
{
    private HelpInfo helpInfo = new HelpInfo( "quit", "Quits the program." );

    public bool CommandExecuted( string commandName, string[] args, AdminConsole adminConsole )
    {
        if ( commandName == helpInfo.commandName || commandName == "stop" )
        {
            adminConsole.currentSession.Info( "Quitting Program..." );
            Server.EventRequest( Server.CrossThreadEventType.Quit );
            return true;
        }

        return false;
    }

    public HelpInfo DisplayHelp()
    {
        return helpInfo;
    }
}