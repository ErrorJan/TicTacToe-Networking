using TicTacToe_Server;

class SwitchOutputCommand : ICommand, IHelp
{
    private HelpInfo helpInfo = new( "switch", "Switch output sessions." );

    public bool CommandExecuted(string commandName, string[] args, AdminConsole adminConsole)
    {
        if ( commandName == helpInfo.commandName )
        {
            if ( args.Length > 0 )
            {
                if ( args[0] == "Main" )
                    adminConsole.currentSession = Server.mainCoutSession;
                else if ( args[0] == "1" )
                    adminConsole.currentSession = Server.newClientsListener.console;
                else
                    adminConsole.currentSession.Error( "Unknown session!" );
            }
            else
            {
                adminConsole.currentSession.Info( "Sessions available:" );
                adminConsole.currentSession.Info( "        Main" );
                adminConsole.currentSession.Info( "        1" );
            }

            return true;
        }

        return false;
    }

    public HelpInfo DisplayHelp()
    {
        return helpInfo;
    }
}