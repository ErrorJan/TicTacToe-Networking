namespace TicTacToe_Server_Test;
using TicTacToe_Shared;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class UnitTest1
{
    public event Action<Socket, ConnectionDataType, byte[]>? OnReceived;
    public Task? handleReceiveTask;
    public PlayerData? player;
    public PlayerData? otherPlayer;
    public int i = 0;

    [Fact]
    public void Test1()
    {
        //Assert.Equal( 4, 5 );

        IPEndPoint ipEndPoint = IPEndPoint.Parse( $"127.0.1.2:{StaticGameInfo.GAME_PORT}" );
        Socket server = new ( ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp );
        OnReceived += OnReceivedEvent;
        Console.WriteLine( "Connecting..." );
        server.Connect( ipEndPoint );
        Console.WriteLine( "Connected!" );


        handleReceiveTask = ConnectionData.HandleReceive( server, OnReceived );

        handleReceiveTask.Wait();
        Console.WriteLine( "Connection Closed" );
        OnReceived -= OnReceivedEvent;
    }

    private void OnReceivedEvent( Socket s, ConnectionDataType cdt, byte[] data )
    {
        data = ArrayUtils.TrimArray( data, 1 );
        Console.WriteLine( "Received something." );

        if ( ConnectionDataType.PlayerInfo == cdt )
        {
            byte playerID = data[0];
            player = new( $"Player{playerID}", playerID );
            ConnectionData.SendData( s, cdt, player.Serialize() ).Wait();
            Console.WriteLine( "Sending data." );
        }
        else if ( ConnectionDataType.GameStart == cdt )
        {
            otherPlayer = PlayerData.Deserialize( ref data );
        }
        else if ( ConnectionDataType.PlayerTurn == cdt )
        {
            if ( data[0] == player?.playerID )
            {
                if ( player.playerID == 1 )
                {
                    BoardMove move = new( i++, 0, BoardPlace.X );
                    ConnectionData.SendData( s, ConnectionDataType.PlayerTurn, BoardMove.SerializeMove( move ) ).Wait();
                }
                else if ( player.playerID == 2 )
                {
                    BoardMove move = new( 0, i++, BoardPlace.O );
                    ConnectionData.SendData( s, ConnectionDataType.PlayerTurn, BoardMove.SerializeMove( move ) ).Wait();
                }
            }
        }
        else if ( ConnectionDataType.GameEvent == cdt )
        {
            BoardMove move = BoardMove.Deserialize( ref data );
            Console.WriteLine( move );
        }
        else if ( ConnectionDataType.GameEnd == cdt )
        {
            Console.WriteLine( $"Player ID: {data[0]} won" );
        }
        else if ( ConnectionDataType.ConnectionClose == cdt )
        {
            Console.WriteLine( "ConnectionClose Info" );
        }
    }
}