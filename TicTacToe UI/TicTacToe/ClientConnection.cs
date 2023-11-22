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
        public event Action<byte>? playerWonEvent;

        public ClientConnection( string playerName, IPEndPoint ipToServer )
        {
            //IPEndPoint.Parse( $"127.0.1.2:{StaticGameInfo.GAME_PORT}" );
            this.server = new ( ipToServer.AddressFamily, SocketType.Stream, ProtocolType.Tcp );
            Console.WriteLine( $"Connected to {ipToServer}!" );
            Console.WriteLine( "Receiving player ID!" );
            byte[] buffer = new byte[ 1 ];
            server.Receive( buffer );
            player = new( playerName, playerID );
            Console.WriteLine( $"Full PlayerData: {player}" );
            this.server.Send( player.Serialize() );

            Console.WriteLine( "Receiving other player..." );
            buffer = new byte[ 512 ];
            server.Receive( buffer );
            opponent = PlayerData.Deserialize( buffer );
            Console.WriteLine( $"Opponent: {opponent}" );
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
                        currentTurn = eventBytes[1] == playerID;
                        if ( currentTurn )
                            playerTurnEvent?.Invoke( player );
                        else
                            playerTurnEvent?.Invoke( opponent );
                    }
                    else if ( eventBytes[0] == 1 )
                    {
                        playerWonEvent?.Invoke( eventBytes[1] );
                        break;
                    }

                    eventBytes = new byte[ 3 ];
                    server.Receive( eventBytes );
                    BoardMove.Deserialize( eventBytes );
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
        }
    }
}
        
/**/