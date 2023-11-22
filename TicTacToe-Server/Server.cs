namespace TicTacToe_Server;

using TicTacToe_Shared;
using System.Net;
using System.Net.Sockets;

class Server
{
    private static void Main(string[] args)
    {
        IPEndPoint ipEndPoint = new ( IPAddress.Any, StaticGameInfo.GAME_PORT );
        Socket listener = new ( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
        byte[] buffer = new byte[ 2048 ];

        listener.Bind( ipEndPoint );
        listener.Listen( 2 );

        while ( true )
        {
            Console.WriteLine( "Awaiting players." );
            Socket player1Connection = listener.Accept();
            Console.WriteLine( "Player 1 connected!" );
            Console.WriteLine( player1Connection.RemoteEndPoint );
            Socket player2Connection = listener.Accept();
            Console.WriteLine( "Player 2 connected!" );
            Console.WriteLine( player2Connection.RemoteEndPoint );
            BoardData boardData = new();
            try
            {
                Console.WriteLine( "Getting PlayerData..." );
                player1Connection.Send( new byte[]{ 1 } );
                player2Connection.Send( new byte[]{ 2 } );
                player1Connection.Receive( buffer );
                PlayerData player1 = PlayerData.Deserialize( buffer );
                Console.WriteLine( $"Player 1: {player1}" );
                player2Connection.Receive( buffer );
                PlayerData player2 = PlayerData.Deserialize( buffer );
                Console.WriteLine( $"Player 2: {player2}" );

                Console.WriteLine( "Initializing game." );
                player1Connection.Send( player2.Serialize() );
                player2Connection.Send( player1.Serialize() );

                while ( true )
                {
                    Console.WriteLine( "Player 1's turn" );
                    Thread.Sleep( 1000 );
                    player1Connection.Send( new byte[]{ 0, 1 } );
                    player2Connection.Send( new byte[]{ 0, 1 } );

                    Console.WriteLine( "Receiving 1's move..." );
                    player1Connection.Receive( buffer );
                    BoardMove move = BoardMove.Deserialize( buffer );
                    boardData.Update( move );
                    Console.WriteLine( move );
                    Console.WriteLine( boardData );
                    Console.WriteLine( "Syncing boardData" );
                    player1Connection.Send( move.Serialize() );
                    player2Connection.Send( move.Serialize() );

                    if ( CheckBoardWin( boardData.board, BoardPlace.X ) )
                    {
                        Console.WriteLine( "Player 1 Wins" );
                        player1Connection.Send( new byte[]{ 1, 1 } );
                        player2Connection.Send( new byte[]{ 1, 1 } );
                        break;
                    }
                    else if ( !CheckBoardFree( boardData.board ) )
                    {
                        Console.WriteLine( "Draw." );
                        player1Connection.Send( new byte[]{ 1, 0 } );
                        player2Connection.Send( new byte[]{ 1, 0 } );
                        break;
                    }

                    Console.WriteLine( "Player 2's turn" );
                    player1Connection.Send( new byte[]{ 0, 2 } );
                    player2Connection.Send( new byte[]{ 0, 2 } );

                    Console.WriteLine( "Receiving 2's move..." );
                    player2Connection.Receive( buffer );
                    move = BoardMove.Deserialize( buffer );
                    boardData.Update( move );
                    Console.WriteLine( move );
                    Console.WriteLine( boardData );
                    Console.WriteLine( "Syncing boardData" );
                    player1Connection.Send( move.Serialize() );
                    player2Connection.Send( move.Serialize() );

                    if ( CheckBoardWin( boardData.board, BoardPlace.O ) )
                    {
                        Console.WriteLine( "Player 2 Wins" );
                        player1Connection.Send( new byte[]{ 1, 2 } );
                        player2Connection.Send( new byte[]{ 1, 2 } );
                        break;
                    }
                    else if ( !CheckBoardFree( boardData.board ) )
                    {
                        Console.WriteLine( "Draw." );
                        player1Connection.Send( new byte[]{ 1, 0 } );
                        player2Connection.Send( new byte[]{ 1, 0 } );
                        break;
                    }
                }
            }
            catch( Exception e )
            {
                Console.WriteLine( e );
            }
            
            Console.WriteLine( "Closing Connection." );
            player1Connection.Disconnect( false );
            player2Connection.Disconnect( false );
        }
    }

    private static bool CheckBoardWin( BoardPlace[][] array, BoardPlace place )
    {
        // 0: 0 1 2
        // 1: 0 1 2
        // 2: 0 1 2

        bool topLeft     = array[0][0] == place;
        bool topRight    = array[0][2] == place;
        bool bottomLeft  = array[2][0] == place;
        bool bottomRight = array[2][2] == place;

        bool middleLeft = array[1][0] == place;
        bool middleRight = array[1][2] == place;
        
        bool top = array[0][1] == place;
        bool middle = array[1][1] == place;
        bool bottom = array[2][1] == place;
        
        return 
        ( middle && ( topLeft && bottomRight || topRight && bottomLeft || ( top && bottom ) || ( middleRight && middleLeft ) ) ) ||
        ( topLeft && ( ( topRight && top ) || ( bottomLeft && middleLeft ) ) ) ||
        ( bottomRight && ( ( topRight && middleRight ) || ( bottomLeft && bottom ) ) ); 
    }

    private static bool CheckBoardFree( BoardPlace[][] array )
    {
        bool free = false;

        foreach ( BoardPlace[] pArr in array )
        {
            foreach ( BoardPlace p in pArr )
            {
                if ( p == BoardPlace.Unassigned )
                {
                    free = true;
                }
            }
        }

        return free;
    }
}