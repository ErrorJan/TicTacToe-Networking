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

    public static BoardMove Deserialize( byte[] data )
    {
        int x = data[ 0 ];
        int y = data[ 1 ];
        BoardPlace place = (BoardPlace)data[ 2 ];

        if ( !Enum.IsDefined( typeof( BoardPlace ), data[ 2 ] ) )
            throw new Exception( "Invalid board data!" );

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

    public override string ToString()
    {
        return $"X: {x}, Y: {y}, Placement: {place}";
    }

    public byte[] Serialize()
    {
        return SerializeMove( this );
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

    public override string ToString()
    {
        return $"{(byte)board[0][0]} {(byte)board[0][1]} {(byte)board[0][2]}\n{(byte)board[1][0]} {(byte)board[1][1]} {(byte)board[1][2]}\n{(byte)board[2][0]} {(byte)board[2][1]} {(byte)board[2][2]}";
    }

    public BoardPlace[][] board { get; private set; }
}