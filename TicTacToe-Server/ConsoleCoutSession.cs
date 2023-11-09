namespace TicTacToe_Server;
using Spectre.Console;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

class ConsoleCoutSession
{
    public const int MAX_LINE_BUFFER = 100;
    private Text[] outputLines = new Text[ MAX_LINE_BUFFER ];
    public int messagesLogged { get { return numOfTextsLogged; } }
    private int numOfTextsLogged = 0;

    public IEnumerable<Text> GetConsoleOutput( int size, int offset = 0 )
    {
        for ( int i = 1; i <= size && i <= MAX_LINE_BUFFER; i++ )
        {
            int i2 = size+offset - i;
            if ( i2 > MAX_LINE_BUFFER )
                continue;
            if ( outputLines[ i2 ] == null )
                continue;
            yield return outputLines[ i2 ];
        }
    }

    private void LogMessage( string messageType, string message, Color color )
    {
        // Needs reimplementing.... Don't feel like it rn...
        // Maybe even own implementation of TUI
        Array.Copy( outputLines, 0, outputLines, 1, MAX_LINE_BUFFER - 1 );
        Text insertText = new ( String.Format( "{0} [{1}]: {2}", DateTime.Now.ToString("HH:mm:ss"), messageType, message ).Replace( "\t", "    " ).Replace( '\n', ' ' ), new Style( color ) );
        outputLines[0] = insertText;
        if ( numOfTextsLogged <= MAX_LINE_BUFFER )
            numOfTextsLogged++;
    }

    public void Info( string logMsg )
    {
        LogMessage( "Info", logMsg, Color.White );
    }

    public void Error( string errMsg )
    {
        LogMessage( "Error", errMsg, Color.Red1 );
    }

    public void Warning( string warnMsg )
    {
        LogMessage( "Warning", warnMsg, Color.Yellow1 );
    }

    public void Debug( string dbgMsg )
    {
        if ( Server.debugMessagesEnabled )
            LogMessage( "Debug", dbgMsg, Color.Tan );
    }
}