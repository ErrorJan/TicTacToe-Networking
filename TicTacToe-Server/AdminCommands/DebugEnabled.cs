namespace TicTacToe_Server;

class DebugEnabledCommand : ICommand, IHelp
{
    private HelpInfo helpInfo = new HelpInfo( "debugEnabled", "Enable/Disable debug output. Usage: DebugEnabled [true/false]" );

    public bool CommandExecuted(string commandName, string[] args, AdminConsole adminConsole)
    {
        if ( commandName == helpInfo.commandName )
        {
            if ( args.Length > 0 )
            {
                switch ( args[0] )
                {
                    case "true":
                        Server.debugMessagesEnabled = true;
                        adminConsole.currentSession.Debug( "Logging debug messages." );
                        break;
                    case "false":
                        adminConsole.currentSession.Debug( "Not logging debug messages." );
                        Server.debugMessagesEnabled = false;
                        break;
                    default:
                        adminConsole.currentSession.Error( "Not true/false." );
                        break;
                }
            }
            else
            {
                adminConsole.currentSession.Error( "Invalid Command use. true/false needed" );
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