/**/ //If things break, just remove the "* /"

using TicTacToe_Shared;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

namespace TicTacToe
{
    public class ClientConnection
    {
        public PlayerData player { private set; get; }
        public PlayerData opponent { private set; get; }
        public bool currentTurn { private set; get; }
        public event Action<BoardMove>? playerEvent;
        public event Action<PlayerData>? playerTurnEvent;
        public event Action<PlayerData?>? playerWonEvent;
        public Task receiveActionTask;

        public ClientConnection( string playerName, IPEndPoint ipToServer )
        {
            //IPEndPoint.Parse( $"127.0.1.2:{StaticGameInfo.GAME_PORT}" );
            this.server = new ( ipToServer.AddressFamily, SocketType.Stream, ProtocolType.Tcp );
            this.server.Connect( ipToServer );
            Console.WriteLine( $"Connected to {ipToServer}!" );
            Console.WriteLine( "Receiving player ID!" );
            byte[] buffer = new byte[ 1 ];
            server.Receive( buffer );
            if ( playerName.Length > PlayerData.MAX_NAME_LETTERS )
                playerName = playerName.Substring(0, PlayerData.MAX_NAME_LETTERS);
            player = new( playerName, buffer[0] );
            Console.WriteLine( $"Full PlayerData: {player}" );
            this.server.Send( player.Serialize() );

            Console.WriteLine( "Receiving other player..." );
            buffer = new byte[ 512 ];
            server.Receive( buffer );
            opponent = PlayerData.Deserialize( buffer );
            Console.WriteLine( $"Opponent: {opponent}" );

            receiveActionTask = Task.Run( ReceiveAction );
        }

        public async Task SendAction( int x, int y )
        {
            if ( currentTurn )
            {
                BoardPlace place = player.playerID == 1 ? BoardPlace.X : BoardPlace.O;
                BoardMove move = new( x, y, place );
                server.Send( BoardMove.SerializeMove( move ) );
            }
        }

        private async Task ReceiveAction()
        {
            try
            {
                while ( true )
                {
                    byte[] eventBytes = new byte[ 2 ];
                    await server.ReceiveAsync( eventBytes );

                    if ( eventBytes[0] == 0 )
                    {
                        currentTurn = eventBytes[1] == player.playerID;
                        if ( currentTurn )
                            playerTurnEvent?.Invoke( player );
                        else
                            playerTurnEvent?.Invoke( opponent );
                    }
                    else if ( eventBytes[0] == 1 )
                    {
                        bool thisPlayerWon = eventBytes[1] == player.playerID;
                        if ( thisPlayerWon )
                            playerWonEvent?.Invoke( player );
                        else if (eventBytes[1] > 0 )                        
                            playerWonEvent?.Invoke( opponent );
                        else 
                            playerWonEvent?.Invoke( null );

                        break;
                    }
                    

                    eventBytes = new byte[ 3 ];
                    server.Receive( eventBytes );
                    var boardData = BoardMove.Deserialize( eventBytes );
                    playerEvent?.Invoke( boardData );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e );
            }

            server.Disconnect( false );
        }

        private Socket server;

        ~ClientConnection()
        {
            server.Close();
            Console.WriteLine( "Closing!" ); // <-- Test if this works, by just setting null reference.
            receiveActionTask.Wait();
        }
    }
}
        
/**/