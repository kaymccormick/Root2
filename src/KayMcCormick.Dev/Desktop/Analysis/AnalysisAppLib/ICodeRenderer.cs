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
    public interface ICodeRenderer
    {
        void addToken ( ushort rawKind , string text , bool newLine ) ;
        void addTrivia ( int   rawKind , string text , bool newLine ) ;
        void NewLine ( ) ;
        void StartNode ( SyntaxNode node ) ;
        void EndNode ( SyntaxNode   node ) ;
    }
}