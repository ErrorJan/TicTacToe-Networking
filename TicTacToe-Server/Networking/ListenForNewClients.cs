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
        }
    }

    private void GameEvent( PlayerData player, BoardMove move )
    {
        if ( player == currentPlayer )
        {
            
        }
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