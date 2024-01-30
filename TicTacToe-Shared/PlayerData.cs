using System.Text;

namespace TicTacToe_Shared;

public class PlayerData
{
    public readonly string playerName;
    public readonly byte playerID;
    public const int MAX_NAME_LETTERS = 15;

    public PlayerData( string playerName, byte playerID )
    {
        this.playerName = playerName;
        this.playerID = playerID;
    }

    public static PlayerData Deserialize( byte[] data )
    {
        byte playerID = data[0];

        int stringSize;

        for ( stringSize = 1; data[stringSize] != 0; stringSize++ ) {}

        string playerName = Encoding.UTF8.GetString( data, 1, stringSize );

        PlayerData obj = new( playerName, playerID );

        return obj;
    }

    public static byte[] Serialize( PlayerData player )
    {
        byte[] playerNameBytes = Encoding.UTF8.GetBytes( player.playerName );

        byte[] dataBytes = new byte[ 1 + playerNameBytes.Length ];
        dataBytes[0] = player.playerID;
        Array.Copy( playerNameBytes, 0, dataBytes, 1, playerNameBytes.Length );

        return dataBytes;
    }

    public byte[] Serialize()
    {
        return Serialize( this );
    }
}