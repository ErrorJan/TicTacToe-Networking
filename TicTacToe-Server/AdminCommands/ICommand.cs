namespace TicTacToe_Server;

interface ICommand
{
    bool CommandExecuted( string commandName, string[] args, AdminConsole adminConsole );
}