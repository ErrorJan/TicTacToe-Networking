using TicTacToe_Shared;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

namespace TicTacToe
{
    public class ClientConnection
    {
        // Erstellt die Objekte der benötigten Klassen
        private Socket server;
        public PlayerData player { private set; get; }
        public PlayerData opponent { private set; get; }
        public bool currentTurn { private set; get; }
        private Form1 gui;

        // Konstruktor der Klasse ClientConnection
        public ClientConnection( string playerName, IPEndPoint ipToServer, Form1 gui )
        {
            //IPEndPoint.Parse( $"127.0.1.2:{StaticGameInfo.GAME_PORT}" );
            this.server = new ( ipToServer.AddressFamily, SocketType.Stream, ProtocolType.Tcp );
            this.server.Connect( ipToServer );
            Console.WriteLine( $"Connected to {ipToServer}!" );
            byte[] buffer = new byte[ 1 ];
            server.Receive( buffer );
            player = new( playerName, buffer[0] );

            // Sendet die Spielerdaten an den Server 
            this.server.Send( player.Serialize() );
            this.gui = gui;

            buffer = new byte[ 512 ];
            server.Receive( buffer );
            opponent = PlayerData.Deserialize( buffer );


            // Task.Run( funcasync ) ruft die Funktion in der Klammer auf. 
            // Diese Funktion muss eine asynchrone Funktion sein.
            // Bsp: public async Task funcasync() { ... }
            // Diese Funktion wird allerdings auf einem anderem Thread gestartet.
            // Das heißt, dass diese Funktion jetzt parallel zu unserem Hauptprogramm läuft.
            Task.Run( ReceiveAction );
        }

        // Methode für die Sendung des Spielzugs an den Server
        public void SendAction( int x, int y )
        {
            if ( currentTurn )
            {
                BoardPlace place;

                // Spieler 1 == X 
                // Spieler 2 == 0
                if (player.playerID == 1)
                    place = BoardPlace.X;
                else
                    place = BoardPlace.O;
                PlayerAction move = new( x, y, place );
                server.Send( PlayerAction.Serialize( move ) );
            }
        }

        private async Task ReceiveAction()
        {
            try
            {
                // Listening loop
                while ( true )
                {
                    byte[] eventBytes = new byte[ 2 ];

                    // await kann nur in einer asynchronen funktion benutzt werden.
                    // Dies sagt der Funktion, dass sie warten soll, bis die Funktion,
                    // auf die await-et werden muss, fertig ist.
                    await server.ReceiveAsync( eventBytes );

                    // Am Platz 0 ist der Zustand des Spieles gespeichert.
                    // Ist es 0 läuft das Spiel immernoch.
                    if ( eventBytes[0] == 0 )
                    {
                        // Am Platz 1 ist der jetzige Spieler gespeichert.
                        currentTurn = eventBytes[1] == player.playerID;
                        if ( currentTurn )
                            gui.PlayerTurnEvent( player );
                        else
                            gui.PlayerTurnEvent( opponent );
                    }
                    // Ist es 1, ist das Spiel zu Ende.
                    else if ( eventBytes[0] == 1 )
                    {
                        bool thisPlayerWon = eventBytes[1] == player.playerID;

                        // Überprüft wer gewonnen hat und ob jemand gewonnen hat
                        if ( thisPlayerWon )
                            gui.PlayerWonEvent( player );
                        else if (eventBytes[1] > 0 )                        
                            gui.PlayerWonEvent( opponent );
                        else 
                            gui.PlayerWonEvent( null );

                        break;
                    }
                    
                    eventBytes = new byte[ 3 ];

                    // Erhaltet den Spielzug des Spielers
                    server.Receive( eventBytes );
                    gui.PlayerEvent( PlayerAction.Deserialize(eventBytes) );
                }
            }
            catch (Exception) { }

            server.Disconnect( false );
        }
    }
}