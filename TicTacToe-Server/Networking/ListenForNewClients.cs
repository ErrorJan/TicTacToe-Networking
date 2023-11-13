namespace TicTacToe_Server;

using System.Net;
using System.Net.Sockets;
using System.Text;
using TicTacToe_Shared;

class ListenForNewClients
{
    public ListenForNewClients( int gamePort )
    {
        this.gamePort = gamePort;

        // Runs on the thread pool
        Task.Run( HandleNewConnections );
    }

    public ConsoleCoutSession console { get; private set; } = new();

    private TaskCompletionSource? gameFinished;
    private PlayerData? player1;
    private PlayerData? player2;
    private ClientConnections? game;
    private PlayerData? currentPlayer;
    private int gamePort;

    //https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/sockets/socket-services
    private async Task HandleNewConnections()
    {
        IPEndPoint ipEndPoint = new ( IPAddress.Any, this.gamePort );
        Socket listener = new ( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );

        listener.Bind( ipEndPoint );
        listener.Listen( 2 );

        while ( true )
        {
            var player1Socket = listener.Accept();
            console.Info( "Player 1 connected!" );
            var player2Socket = listener.Accept();
            console.Info( "Player 2 connected!" );
            gameFinished = new();

            game = new( player1Socket, player2Socket );
            game.playerEvent += GameEvent;

            var tuple = await game.RequestPlayerData();
            player1 = tuple.Item1;
            player2 = tuple.Item2;

            currentPlayer = player1;
            await game.GameStart();

            PlayerNext();

            await gameFinished.Task;

            game.Disconnect().Wait();
        }
    }

    private void GameEvent( PlayerData player, BoardMove move )
    {
        if ( player == currentPlayer )
        {
            if ( game?.board.board[move.x][move.y] == BoardPlace.Unassigned )
            {
                game.UpdateBoard( move ).Wait();

                bool playerWon = CheckBoard( game.board.board, move );

                if ( playerWon )
                {
                    game.PlayerWin( player ).Wait();
                    gameFinished?.SetResult();
                }
                else
                    PlayerNext();
            }
            else
            {
                console.Error( "Player moved Illegaly!" );
            }
        }
    }

    private bool CheckBoard( BoardPlace[][] array, BoardMove move )
    {
        // 0: 0 1 2
        // 1: 0 1 2
        // 2: 0 1 2

        bool topLeft     = array[0][0] == move.place;
        bool topRight    = array[0][2] == move.place;
        bool bottomLeft  = array[2][0] == move.place;
        bool bottomRight = array[2][2] == move.place;

        bool middleLeft = array[1][0] == move.place;
        bool middleRight = array[1][2] == move.place;
        
        bool top = array[0][1] == move.place;
        bool middle = array[1][1] == move.place;
        bool bottom = array[2][1] == move.place;
        
        return 
        ( middle && ( topLeft && bottomRight || topRight && bottomLeft || ( top && bottom ) || ( middleRight && middleLeft ) ) ) ||
        ( topLeft && ( ( topRight && top ) || ( bottomLeft && middleLeft ) ) ) ||
        ( bottomRight && ( ( topRight && middleRight ) || ( bottomLeft && bottom ) ) ); 
    }

    private void PlayerNext()
    {
        if ( player1 == null || player2 == null || game == null || currentPlayer == null )
            return;

        game.NextPlayerTurn( currentPlayer ).Wait();

        if ( currentPlayer == player1 )
        {
            currentPlayer = player2;
        }
        else
        {
            currentPlayer = player1;
        }
    }
}