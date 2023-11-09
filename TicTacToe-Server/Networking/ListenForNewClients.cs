namespace TicTacToe_Server;

using System.Net;
using System.Net.Sockets;
using System.Text;

class ListenForNewClients
{
    public ListenForNewClients( int gamePort )
    {
        this.gamePort = gamePort;

        Task.Run( HandleNewConnections );
    }

    private int gamePort;

    //https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/sockets/socket-services
    private async Task HandleNewConnections()
    {
        /*
        IPEndPoint ipEndPoint = new ( IPAddress.Any, this.gamePort );
        Socket listener = new ( SocketType.Stream, ProtocolType.Tcp );
        Console.WriteLine( ipEndPoint.AddressFamily );

        listener.Bind( ipEndPoint );
        listener.Listen( 20 );

        while ( true )
        {
            Console.WriteLine( "Waiting..." );
            var handler = listener.Accept();
            Console.WriteLine( "Connected!" );
            var testMsg = "Test";
            var testMsgBytes = Encoding.UTF8.GetBytes( testMsg );
            handler.Send( testMsgBytes, SocketFlags.None );
            Console.WriteLine( $"Send test message back to {handler.RemoteEndPoint} | {handler.LocalEndPoint}" );
            handler.Close();
            Console.WriteLine( "Disconnected!" );
        }*/
    }
}