namespace TicTacToe_Server_Test;
using TicTacToe_Shared;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class UnitTest1
{
    public PlayerData? player;
    public PlayerData? otherPlayer;

    [Fact]
    public void Test1()
    {
        //Assert.Equal( 4, 5 );

        IPEndPoint ipEndPoint = IPEndPoint.Parse( $"127.0.1.2:{StaticGameInfo.GAME_PORT}" );
        Socket server = new ( ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp );
        byte[] buffer = new byte[ 32 ];
        Console.WriteLine( "Connecting..." );
        server.Connect( ipEndPoint );
        Console.WriteLine( "Connected!" );

        server.Receive( buffer );

        byte playerID = buffer[0];
        player = new( $"Player{playerID}", playerID );
        Console.WriteLine( player );
        server.Send( player.Serialize() );

        Console.WriteLine( "Getting other player..." );
        server.Receive( buffer );
        otherPlayer = PlayerData.Deserialize( buffer );
        Console.WriteLine( otherPlayer );
        int i = 0;

        while ( true )
        {
            byte[] eventBytes = new byte[ 2 ];
            server.Receive( eventBytes );
            // string s = "";
            // foreach ( byte b in buffer )
            //     s += $"{b}, ";
            // Console.WriteLine( s );
            if ( eventBytes[0] == 0 )
            {
                if ( eventBytes[1] == playerID && playerID == 1 )
                {
                    Console.WriteLine( "Sending..." );

                    BoardMove move = new( i++, 0, BoardPlace.X );
                    server.Send( BoardMove.SerializeMove( move ) );
                }
                else if ( eventBytes[1] == playerID && playerID == 2 )
                {
                    Console.WriteLine( "Sending..." );

                    BoardMove move = new( 0, i++, BoardPlace.O );
                    server.Send( BoardMove.SerializeMove( move ) );
                }
                else
                {
                    Console.WriteLine( "Receiving..." );
                }
            }
            else if ( eventBytes[0] == 1 )
            {
                if ( eventBytes[1] == playerID )
                    Console.WriteLine( "I Won!" );
                else if ( eventBytes[1] == 0 )
                    Console.WriteLine( "It's a draw!" );
                else
                    Console.WriteLine( "I Lost..." );

                break;
            }
            eventBytes = new byte[3];
            server.Receive( eventBytes );
            // s = "";
            // foreach ( byte b in buffer )
            //     s += $"{b}, ";
            // Console.WriteLine( s );
            Console.WriteLine( BoardMove.Deserialize( eventBytes ) );
        }

        server.Disconnect( false );
    }
}