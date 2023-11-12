namespace TicTacToe_Shared;

public static class ArrayUtils
{
    public static T[] TrimArray<T>( T[] array, int startIndex, int length )
    {
        T[] trimmedArray = new T[ length ];

        Array.Copy( array, startIndex, trimmedArray, 0, length );

        return trimmedArray;
    }

    public static T[] TrimArray<T>( T[] array, int startIndex )
    {
        return TrimArray( array, startIndex, array.Length - startIndex );
    }
}