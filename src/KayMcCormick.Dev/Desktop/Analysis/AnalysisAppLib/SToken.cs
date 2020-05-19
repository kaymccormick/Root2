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
namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SToken
    {
        /// <summary>
        /// 
        /// </summary>
        public SToken ( ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenKind"></param>
        /// <param name="tokenValue"></param>
        public SToken ( string tokenKind , string tokenValue )
        {
            TokenKind  = tokenKind ;
            TokenValue = tokenValue ;
        }

        /// <summary>
        /// 
        /// </summary>
        public string TokenKind { get ; }

        /// <summary>
        /// 
        /// </summary>
        public string TokenValue { get ; }
    }
}