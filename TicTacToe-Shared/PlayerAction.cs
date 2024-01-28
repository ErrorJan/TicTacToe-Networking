namespace TicTacToe_Shared
{
    public class PlayerAction
    {
        public int x;
        public int y;
        public BoardPlace place;

        public PlayerAction( int x, int y, BoardPlace place )
        {
            this.x = x;
            this.y = y;
            this.place = place;
        }

        /// <summary>
        /// Von Bytes auf Objekt
        /// </summary>
        public static PlayerAction Deserialize( byte[] data )
        {
            int x = data[ 0 ];
            int y = data[ 1 ];
            BoardPlace place = (BoardPlace)data[ 2 ];

            PlayerAction move = new ( x, y, place );

            return move;
        }

        /// <summary>
        /// Von Objekt auf Bytes
        /// </summary>
        public static byte[] Serialize( PlayerAction move )
        {
            byte[] data = new byte[]
            {
                (byte)move.x,
                (byte)move.y,
                (byte)move.place
            };

            return data;
        }

        /// <summary>
        /// Von Bytes auf Objekt
        /// </summary>
        public byte[] Serialize()
        {
            return Serialize( this );
        }
    }
}