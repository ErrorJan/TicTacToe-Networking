namespace TicTacToe_Shared;

// List<byte> message = new();
// message.AddRange( new byte[4] );
// message.Add( (byte)ConnectionDataType.PlayerInfo );

// byte[] lengthBytes = BitConverter.GetBytes( (Int32)( message.Count - 4 ) );
// if ( BitConverter.IsLittleEndian )
//     Array.Reverse( lengthBytes );

// message.InsertRange( 0, lengthBytes );

// _ = player1Connection.SendAsync( message.ToArray() );
// _ = player2Connection.SendAsync( message.ToArray() );

/*
Action<Socket, ConnectionDataType, byte> receivedP1Func = 
(Socket s, ConnectionDataType cdt, byte cdtb) => player1Data = GetSafePlayerData( s, cdt, 1 );
*/