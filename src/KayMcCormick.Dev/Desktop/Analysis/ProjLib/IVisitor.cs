#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// IVisitor.cs
// 
// 2020-02-27-12:21 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.Windows.Controls ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.Text ;

namespace ProjLib
{
    public interface IVisitor   
    {
        Stack < Action < object > > Nodes { get ; }

        TextSpan TokenSpan { get ; set ; }

        bool StartOfLine { get ; set ; }

        int CurLine { get ; set ; }

        TextSpan currentSpan { get ; set ; }

        TextBlock CurBlock { get ; set ; }

        SemanticModel Model { get ; set ; }

        Dictionary < object , ISpanViewModel > ActiveSpans { get ; }

        TextSpan SyntaxNodeSpan { get ; set ; }
    }
}