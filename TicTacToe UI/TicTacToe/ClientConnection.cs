using TicTacToe_Shared;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

namespace TicTacToe
{
    public class ClientConnection
    {
        private Socket server;
        public PlayerData player { private set; get; }
        public PlayerData opponent { private set; get; }
        public bool currentTurn { private set; get; }
        private Form1 gui;

        public ClientConnection( string playerName, IPEndPoint ipToServer, Form1 gui )
        {
            //IPEndPoint.Parse( $"127.0.1.2:{StaticGameInfo.GAME_PORT}" );
            this.server = new ( ipToServer.AddressFamily, SocketType.Stream, ProtocolType.Tcp );
            this.server.Connect( ipToServer );
            Console.WriteLine( $"Connected to {ipToServer}!" );
            byte[] buffer = new byte[ 1 ];
            server.Receive( buffer );
            player = new( playerName, buffer[0] );
            this.server.Send( player.Serialize() );
            this.gui = gui;

            buffer = new byte[ 512 ];
            server.Receive( buffer );
            opponent = PlayerData.Deserialize( buffer );

            Task.Run( ReceiveAction );
        }

        public void SendAction( int x, int y )
        {
            if ( currentTurn )
            {
                BoardPlace place = player.playerID == 1 ? BoardPlace.X : BoardPlace.O;
                PlayerAction move = new( x, y, place );
                server.Send( PlayerAction.Serialize( move ) );
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
                            gui.PlayerTurnEvent( player );
                        else
                            gui.PlayerTurnEvent( opponent );
                    }
                    else if ( eventBytes[0] == 1 )
                    {
                        bool thisPlayerWon = eventBytes[1] == player.playerID;
                        if ( thisPlayerWon )
                            gui.PlayerWonEvent( player );
                        else if (eventBytes[1] > 0 )                        
                            gui.PlayerWonEvent( opponent );
                        else 
                            gui.PlayerWonEvent( null );

                        break;
                    }
                    
                    eventBytes = new byte[ 3 ];
                    server.Receive( eventBytes );
                    gui.PlayerEvent( PlayerAction.Deserialize(eventBytes) );
                }
            }
            catch (Exception) { }

            server.Disconnect( false );
        }
    }
}