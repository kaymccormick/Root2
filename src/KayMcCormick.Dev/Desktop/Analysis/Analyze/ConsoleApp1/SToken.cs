#region header
// Kay McCormick (mccor)
// 
// Analysis
// ConsoleApp1
// SToken.cs
// 
// 2020-04-07-12:41 PM
// 
// ---
#endregion
namespace ConsoleApp1
{
    public sealed class SToken
    {
        public SToken ( ) { }

        public SToken ( string tokenKind , string tokenValue )
        {
            TokenKind  = tokenKind ;
            TokenValue = tokenValue ;
        }

        public string TokenKind { get ; }

        public string TokenValue { get ; }
    }
}