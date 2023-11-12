using System.Net.Sockets;

namespace TicTacToe_Shared;

public enum ConnectionDataType : byte
{
    Unknown,
    PlayerInfo,
    GameStart,
    PlayerTurn,
    GameEvent,
    GameEnd,
    ConnectionClose,
    ConnectionClosedExeption
}

public static class ConnectionData
{
    public static async Task HandleReceive( Socket socket, Action<Socket, ConnectionDataType, byte[]>? raiseEvent )
    {
        bool connectionClosed = false;

        try
        {
            while ( !connectionClosed )
            {
                byte[] dataTypeByte = new byte[1];
                await socket.ReceiveAsync( dataTypeByte, SocketFlags.None );

                ConnectionDataType dataType = GetConnectionDataType( dataTypeByte );
                connectionClosed = dataType == ConnectionDataType.ConnectionClose;

                byte[] nextData = new byte[ socket.Available ];
                if ( nextData.Length > 0 )
                {
                    socket.Receive( nextData );
                }

                byte[] fullData = new byte[ 1 + nextData.Length ];
                fullData[0] = dataTypeByte[0];
                Array.Copy( nextData, 0, fullData, 1, nextData.Length );

                raiseEvent?.Invoke( socket, dataType, fullData );
            }
        }
        catch ( Exception e )
        {
            Console.WriteLine( e );
            raiseEvent?.Invoke( socket, ConnectionDataType.ConnectionClose, new byte[]{ (byte)ConnectionDataType.ConnectionClosedExeption } );
        }
    }

    public static ConnectionDataType GetConnectionDataType( byte[] array )
    {
        return GetConnectionDataType( array[0] );
    }

    public static ConnectionDataType GetConnectionDataType( byte value )
    {
        ConnectionDataType type;

        if( Enum.IsDefined( typeof( ConnectionDataType ), value ) )
            type = (ConnectionDataType)value;
        else
            type = ConnectionDataType.Unknown;

        return type;
    }

    public static async Task SendData( Socket s, ConnectionDataType dataType, byte[] data )
    {
        List<byte> message = new( 1 + data.Length );
        message.Add( (byte)dataType );
        message.AddRange( data );
        await s.SendAsync( message.ToArray(), SocketFlags.None );
    }

    public static async Task SendData( Socket s, ConnectionDataType dataType, byte data )
    {
        await SendData( s, dataType, new byte[]{ data } );
    }

    public static async Task SendData( Socket s, ConnectionDataType dataType )
    {
        await SendData( s, dataType, new byte[]{} );
    }

    public static async Task Broadcast( Socket s1, Socket s2, ConnectionDataType dataType, byte[] data )
    {
        Task t1 = SendData( s1, dataType, data );
        Task t2 = SendData( s2, dataType, data );

        await t1;
        await t2;
    }

    public static async Task Broadcast( Socket s1, Socket s2, ConnectionDataType dataType, byte data )
    {
        await Broadcast( s1, s2, dataType, new byte[]{ data } );
    }

    public static async Task Broadcast( Socket s1, Socket s2, ConnectionDataType dataType )
    {
        await Broadcast( s1, s2, dataType, new byte[]{} );
    }
}