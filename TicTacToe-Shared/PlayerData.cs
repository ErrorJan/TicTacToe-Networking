using System.Text;

namespace TicTacToe_Shared;

public class PlayerData
{
    public readonly string playerName;
    public readonly byte playerID;
    public const int MAX_NAME_LETTERS = 15;

    public PlayerData( string playerName, byte playerID )
    {
        if ( playerName.Length > MAX_NAME_LETTERS )
            throw new Exception( $"Player name {playerName} is too long!" );
        
        if ( playerName.Length == 0 )
        {
            playerName = $"Player{playerID}";
        }

        this.playerName = playerName;
        this.playerID = playerID;
    }

    public static PlayerData Deserialize( byte[] data )
    {
        byte playerID = data[0];

        int stringSize = data.Skip( 1 ).TakeWhile( cb => cb != 0 ).Count();

        if ( stringSize > MAX_NAME_LETTERS )
            throw new Exception( "Wrong name format. Too long!" );

        string playerName = Encoding.UTF8.GetString( data, 1, stringSize );

        PlayerData obj = new( playerName, playerID );

        return obj;
    }

    public static byte[] Serialize( PlayerData player )
    {
        byte[] playerNameBytes = Encoding.UTF8.GetBytes( player.playerName );

        byte[] dataBytes = new byte[ 1 + playerNameBytes.Length + 1 ];
        dataBytes[0] = player.playerID;
        Array.Copy( playerNameBytes, 0, dataBytes, 1, playerNameBytes.Length );
        dataBytes[ dataBytes.Length - 1 ] = 0;

        return dataBytes;
    }

    public byte[] Serialize()
    {
        return Serialize( this );
    }
}