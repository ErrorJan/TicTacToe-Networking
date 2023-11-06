namespace TicTacToe_Server_Test;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        //Assert.Equal( 4, 5 );


        IPEndPoint ipEndPoint = IPEndPoint.Parse( "127.0.2.2:2030" );
        Socket client = new ( ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp );

        client.Connect( ipEndPoint );
        var buffer = new byte[ 1_024 ];
        var received = client.Receive( buffer, SocketFlags.None );
        var response = Encoding.UTF8.GetString( buffer, 0, received );
        Console.WriteLine( $"Received: {response}" );
        client.LingerState = new LingerOption( false, 0 );

        while ( client.Connected )
        {
            Console.WriteLine( "Not dead" );
            Thread.Sleep(1000);
            
            string s = Console.ReadLine();
            if (s == "y") 
                client.Close();
        }
        Console.WriteLine( "Dead" );
    }
}