namespace TicTacToe_Server;

using TicTacToe_Shared;
using System.Net;
using System.Net.Sockets;

class Server
{
    enum GameState
    {
        Playing = 0,
        End = 1
    }

    private static bool gameRunning = false;

    private static void Main(string[] args)
    {
        // IPAddress.Any Alle IP Addressen akzeptieren.
        // Gameport, port an dem wir warten
        IPEndPoint ipEndPoint = new ( IPAddress.Any, StaticGameInfo.GAME_PORT );

        // InterNetwork (IPv4)
        // Stream: C# hilft uns mit dem Netzwerk und stellt eine Verbindung für uns her.
        // ProtocolType.Tcp: Benutze das TCP Protokoll. Stellt sicher die Daten kommen definitiv an.
        Socket listener = new ( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );

        // Puffer, in dem wir alle Daten speichern.
        byte[] buffer = new byte[ 2048 ];

        // Mit was der Socket arbeiten soll
        listener.Bind( ipEndPoint );
        // Warte auf immer nur 2 Clients
        listener.Listen( 2 );

        // Listening Loop
        while ( true )
        {
            Console.WriteLine( "Warte auf Spieler..." );

            Socket player1Connection = listener.Accept();
            Console.WriteLine( "Player 1 verbunden!" );
            Console.WriteLine( player1Connection.RemoteEndPoint );

            Socket player2Connection = listener.Accept();
            Console.WriteLine( "Player 2 verbunden!" );
            Console.WriteLine( player2Connection.RemoteEndPoint );

            BoardData boardData = new();

            // Wenn es eine Exception gab 
            // (Ein Fehler im Code, zB. die Spieler haben sich auf einmal vom Server getrennt, oder es gab eine Fehleingabe), 
            // wird der ganze Game Loop unterbrochen und es wird auf neue Spieler gewartet.
            try
            {
                Console.WriteLine( "Getting PlayerData..." );

                // Sende den Spielern die ID, die sie haben
                player1Connection.Send( new byte[]{ 1 } );
                player2Connection.Send( new byte[]{ 2 } );

                // Playerobjekt von den Spielern wird empfangen
                player1Connection.Receive( buffer );
                PlayerData player1 = PlayerData.Deserialize( buffer );
                player2Connection.Receive( buffer );
                PlayerData player2 = PlayerData.Deserialize( buffer );

                Console.WriteLine( "Initializing game." );

                // Playerobjekt von den Spielern wird den anderen Spielern gesendet
                player1Connection.Send( player2.Serialize() );
                player2Connection.Send( player1.Serialize() );

                gameRunning = true;

                while ( gameRunning )
                {
                    GameLoop( 1, player1Connection, player2Connection, boardData, BoardPlace.X );
                    GameLoop( 2, player2Connection, player1Connection, boardData, BoardPlace.O );
                }
            }
            catch( Exception e )
            {
                // In der Konsole loggen, was für ein Fehler es gab
                Console.WriteLine( e );
            }
            
            player1Connection.Disconnect( false );
            player2Connection.Disconnect( false );
            Console.WriteLine( "Verbindung getrennt." );
        }
    }

    private static void GameLoop( byte playerID, Socket currentPlayerSocket, Socket otherPlayer, BoardData board, BoardPlace place )
    {
        byte[] buffer = new byte[ 3 ];

        Console.WriteLine( $"Player {playerID}'s turn" );
        currentPlayerSocket.Send( new byte[]{ (byte)GameState.Playing, playerID } );
        otherPlayer.Send(         new byte[]{ (byte)GameState.Playing, playerID } );

        Console.WriteLine( $"Receiving {playerID}'s move..." );
        currentPlayerSocket.Receive( buffer );
        PlayerAction move = PlayerAction.Deserialize( buffer );
        board.Update( move );
        
        Console.WriteLine( "Syncing boardData" );
        currentPlayerSocket.Send( buffer );
        otherPlayer.Send( buffer );

        if ( CheckBoardWin( board.board, place ) )
        {
            Console.WriteLine( $"Player {playerID} Wins" );
            currentPlayerSocket.Send( new byte[]{ (byte)GameState.End, playerID } );
            otherPlayer.Send(         new byte[]{ (byte)GameState.End, playerID } );
            gameRunning = false;
        }
        else if ( !CheckBoardFree( board.board ) )
        {
            Console.WriteLine( "Draw." );
            currentPlayerSocket.Send( new byte[]{ (byte)GameState.End, 0 } );
            otherPlayer.Send( new byte[]{ (byte)GameState.End, 0 } );
            gameRunning = false;
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

        bool middleLeft  = array[1][0] == place;
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