#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// ICodeRenderer.cs
// 
// 2020-03-02-2:30 PM
// 
// ---
#endregion
using Microsoft.CodeAnalysis ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICodeRenderer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawKind"></param>
        /// <param name="text"></param>
        /// <param name="newLine"></param>
        void AddToken ( ushort rawKind , string text , bool newLine ) ;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawKind"></param>
        /// <param name="text"></param>
        /// <param name="newLine"></param>
        void AddTrivia ( int   rawKind , string text , bool newLine ) ;
        /// <summary>
        /// 
        /// </summary>
        void NewLine ( ) ;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void StartNode ( SyntaxNode node ) ;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void EndNode ( SyntaxNode   node ) ;
    }
}