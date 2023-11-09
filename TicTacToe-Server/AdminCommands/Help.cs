using Microsoft.VisualBasic;

namespace TicTacToe_Server;

class HelpCommand : ICommand, IHelp
{
    private HelpInfo helpInfo = new HelpInfo( "help", "Shows help about a command. Usage: help [command]" );
    private IEnumerable<IHelp>? commandsWithHelp = null;

    public bool CommandExecuted( string commandName, string[] args, AdminConsole adminConsole )
    {
        if ( commandsWithHelp == null )
        {
            commandsWithHelp = adminConsole.commands.OfType<IHelp>();
        }

        if ( commandName == helpInfo.commandName )
        {
            if ( args.Length > 0 )
            {
                HelpInfo commandHelp;
                bool foundCommand = false;
                foreach( IHelp help in commandsWithHelp )
                {
                    commandHelp = help.DisplayHelp();
                    if ( commandHelp.commandName == args[0] )
                    {
                        adminConsole.currentSession.Info( commandHelp.ToString() );
                        foundCommand = true;
                        break;
                    }
                }

                if ( !foundCommand )
                {
                    adminConsole.currentSession.Error( $"Unknown Command {args[0]}" );
                }
            }
            else
            {
                adminConsole.currentSession.Info( "Available Commands:" );
                string logMessage = "";
                foreach( IHelp help in commandsWithHelp )
                    logMessage += help.DisplayHelp().commandName + ", ";
                logMessage = logMessage.Remove( logMessage.Length - 2 );
                
                adminConsole.currentSession.Info( logMessage );
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