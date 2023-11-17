using System.Net.Sockets;
using TicTacToe_Shared;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe_Server;

class ClientConnections
{
    public event Action<PlayerData, BoardMove>? playerEvent;
    public BoardData board { get; private set; }

    public ClientConnections( Socket player1, Socket player2 )
    {
        this.player1Connection = player1;
        this.player2Connection = player2;
        board = new();

        _ = ConnectionData.HandleReceive( player1Connection, player1ReceivedEvent );
        _ = ConnectionData.HandleReceive( player2Connection, player2ReceivedEvent );
    }

    public async Task<Tuple<PlayerData, PlayerData>> RequestPlayerData()
    {
        Tuple<PlayerData, PlayerData> players;
        TaskCompletionSource<PlayerData> player1ReqTask = new ();
        TaskCompletionSource<PlayerData> player2ReqTask = new ();

        var receivedP1Func = ( Socket s, ConnectionDataType cdt, byte[] data ) => 
            GetSafePlayerData( cdt, data, 1, player1ReqTask );
        var receivedP2Func = ( Socket s, ConnectionDataType cdt, byte[] data ) => 
            GetSafePlayerData( cdt, data, 2, player2ReqTask );
        player1ReceivedEvent += receivedP1Func;
        player2ReceivedEvent += receivedP2Func;

        _ = ConnectionData.SendData( player1Connection, ConnectionDataType.PlayerInfo, 1 );
        _ = ConnectionData.SendData( player2Connection, ConnectionDataType.PlayerInfo, 2 );

        await player1ReqTask.Task;
        await player2ReqTask.Task;

        player1ReceivedEvent -= receivedP1Func;
        player2ReceivedEvent -= receivedP2Func;

        this.player1Data = player1ReqTask.Task.Result;
        this.player2Data = player2ReqTask.Task.Result;

        players = new ( this.player1Data, this.player2Data );
        return players;
    }
    public Tuple<PlayerData, PlayerData> GetPlayers()
    {
        if ( player1Data == null || player2Data == null )
            throw new Exception( "Players weren't initialized!" );
        
        return new Tuple<PlayerData, PlayerData>( player1Data, player2Data );
    }

    public async Task GameStart()
    {
        if ( player1Data == null || player2Data == null )
            throw new Exception( "Players weren't initialized!" );

        var awaitP1 = ConnectionData.SendData( player1Connection, ConnectionDataType.GameStart, player2Data.Serialize() );
        var awaitP2 = ConnectionData.SendData( player2Connection, ConnectionDataType.GameStart, player1Data.Serialize() );

        await awaitP1;
        await awaitP2;
    }

    public async Task NextPlayerTurn( PlayerData nextPlayersTurn )
    {
        player1ReceivedEvent += PlayerTurnEventHandler;
        player2ReceivedEvent += PlayerTurnEventHandler;

        await ConnectionData.Broadcast( player1Connection, player2Connection, ConnectionDataType.PlayerTurn, nextPlayersTurn.playerID );
    }

    public async Task UpdateBoard( BoardMove move )
    {
        if ( board.Update( move ) )
        {
            player1ReceivedEvent -= PlayerTurnEventHandler;
            player2ReceivedEvent -= PlayerTurnEventHandler;
            await ConnectionData.Broadcast( player1Connection, player2Connection, ConnectionDataType.GameEvent, BoardMove.SerializeMove( move ) );
        }
        else
            throw new Exception( "Board move Illegal!" );
    }

    public async Task PlayerWin( PlayerData won )
    {
        if ( player1Data == null || player2Data == null )
            throw new Exception( "Players weren't initialized!" );

        await ConnectionData.Broadcast( player1Connection, player2Connection, ConnectionDataType.GameEnd, won.playerID );
    }

    public async Task PlayerDraw()
    {
        if ( player1Data == null || player2Data == null )
            throw new Exception( "Players weren't initialized!" );

        await ConnectionData.Broadcast( player1Connection, player2Connection, ConnectionDataType.GameEnd, 0xFF );
    }

    public async Task Disconnect()
    {
        await ConnectionData.Broadcast( player1Connection, player2Connection, ConnectionDataType.ConnectionClose );

        player1Connection.Disconnect( false );
        player2Connection.Disconnect( false );
    }

    ~ClientConnections()
    {
        Disconnect().Wait();
    }

    private Socket player1Connection;
    private Socket player2Connection;
    private event Action<Socket, ConnectionDataType, byte[]>? player1ReceivedEvent;
    private event Action<Socket, ConnectionDataType, byte[]>? player2ReceivedEvent;
    private PlayerData? player1Data;
    private PlayerData? player2Data;

    private void GetSafePlayerData( ConnectionDataType cdt, byte[] data, int playerID, TaskCompletionSource<PlayerData> playerReqTask )
    {
        PlayerData? playerData = null;
        data = ArrayUtils.TrimArray( data, 1 );

        playerData = PlayerData.Deserialize( ref data );

        if ( playerData == null )
            throw new Exception( $"Could not get player {playerID}'s data!" );

        if ( playerData.playerID != playerID )
            throw new Exception( $"Player {playerID} tampered with data!" );

        playerReqTask.SetResult( playerData );
    }

    private void PlayerTurnEventHandler( Socket socket, ConnectionDataType cdt, byte[] data )
    {
        if ( player1Data == null || player2Data == null )
            throw new Exception( "Players weren't initialized!" );

        if ( cdt == ConnectionDataType.PlayerTurn )
        {
            PlayerData player = socket == player1Connection ? player1Data : player2Data;
            BoardMove move = BoardMove.Deserialize( ref data );

            playerEvent?.Invoke( player, move );
        }
    }
}
