namespace TicTacToe_Shared;

public enum BoardPlace : byte
{
    Unassigned,
    O,
    X
}

public class BoardMove
{
    public BoardMove( int x, int y, BoardPlace place )
    {
        this.x = x;
        this.y = y;
        this.place = place;
    }

    public static BoardMove Deserialize( ref byte[] data )
    {
        int x = data[ 0 ];
        int y = data[ 1 ];
        BoardPlace place = (BoardPlace)data[ 2 ];

        if ( !Enum.IsDefined( typeof( BoardPlace ), data[ 2 ] ) )
            throw new Exception( "Invalid board data!" );

        data = ArrayUtils.TrimArray( data, 3 );
        BoardMove move = new ( x, y, place );

        return move;
    }

    public static byte[] SerializeMove( BoardMove move )
    {
        byte[] data = new byte[]
        {
            (byte)move.x,
            (byte)move.y,
            (byte)move.place
        };

        return data;
    }

    public int x;
    public int y;
    public BoardPlace place;
}

public class BoardData
{
    public BoardData()
    {
        board = new BoardPlace[][]
        {
            new BoardPlace[]{ BoardPlace.Unassigned, BoardPlace.Unassigned, BoardPlace.Unassigned },
            new BoardPlace[]{ BoardPlace.Unassigned, BoardPlace.Unassigned, BoardPlace.Unassigned },
            new BoardPlace[]{ BoardPlace.Unassigned, BoardPlace.Unassigned, BoardPlace.Unassigned }
        };
    }

    public BoardData( BoardPlace[][] boardSync )
    {
        this.board = boardSync;
    }

    public bool Update( BoardMove move )
    {
        if ( move.y > board.Length || move.x > board[0].Length )
            return false;
        
        board[ move.y ][ move.x ] = move.place;
        return true;
    }

    public BoardPlace[][] board { get; private set; }
}