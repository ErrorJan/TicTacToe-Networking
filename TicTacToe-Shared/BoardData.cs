namespace TicTacToe_Shared
{
    public class BoardData
    {
        /// <summary>
        /// BoardPlace[][] ist das Spielbrett, wo alle Spielereingaben verarbeitet wurden.
        /// </summary>
        public BoardPlace[][] board { get; private set; }

        public BoardData()
        {
            // Neues Brett erstellen
            board = new BoardPlace[][]
            {
                new BoardPlace[]{ BoardPlace.Unassigned, BoardPlace.Unassigned, BoardPlace.Unassigned },
                new BoardPlace[]{ BoardPlace.Unassigned, BoardPlace.Unassigned, BoardPlace.Unassigned },
                new BoardPlace[]{ BoardPlace.Unassigned, BoardPlace.Unassigned, BoardPlace.Unassigned }
            };
        }

        /// <summary>
        /// Aktualisiere das Brett mit einer Spielereingabe
        /// </summary>
        /// <param name="move">Die Spielereingabe</param>
        public void Update( PlayerAction move )
        {
            if ( move.y < board.Length || move.x < board[0].Length )
            {
                board[ move.y ][ move.x ] = move.place;
            }
        }
    }
}