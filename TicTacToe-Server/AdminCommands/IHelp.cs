namespace TicTacToe_Server;

interface IHelp
{
    HelpInfo DisplayHelp();
}

struct HelpInfo
{
    public HelpInfo( string commandName, string description )
    {
        this.commandName = commandName;
        this.description = description;
    }

    public string commandName { get; }
    public string description { get; }

    public override string ToString()
    {
        return $"{commandName}:\n\t{description}";
    }
}