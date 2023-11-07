namespace TicTacToe_Server_Test;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        //Assert.Equal( 4, 5 );

/*
        IPEndPoint ipEndPoint = IPEndPoint.Parse( "127.0.2.2:2030" );
        Socket client = new ( ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp );

        client.Connect( ipEndPoint );
        var buffer = new byte[ 1_024 ];
        var received = client.Receive( buffer, SocketFlags.None );
        var response = Encoding.UTF8.GetString( buffer, 0, received );
        Console.WriteLine( $"Received: {response}" );
        client.LingerState = new LingerOption( false, 0 );
        client.Close();*/

        /*Console.WriteLine( "Start" );

        TaskCompletionSource<int> tcs1 = new TaskCompletionSource<int>();
        Task<int> t1 = tcs1.Task;

        // Start a background task that will complete tcs1.Task
        Task.Factory.StartNew(() =>
        {
            Thread.Sleep(1000);
            tcs1.SetResult(15);
        });

        Stopwatch sw = Stopwatch.StartNew();
        int result = t1.Result;
        sw.Stop();

        tcs1.SetCanceled();

        Console.WriteLine("(ElapsedTime={0}): t1.Result={1} (expected 15) ", sw.ElapsedMilliseconds, result);

        sw = Stopwatch.StartNew();
        result = t1.Result;
        sw.Stop();

        Console.WriteLine("(ElapsedTime={0}): t1.Result={1} (expected 15) ", sw.ElapsedMilliseconds, result);*/
    }
}