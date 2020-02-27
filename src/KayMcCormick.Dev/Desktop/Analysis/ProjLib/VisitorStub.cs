#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// VisitorStub.cs
// 
// 2020-02-27-12:20 AM
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
    public class VisitorStub : IVisitor
    {
        public Stack < Action < object > > Nodes { get ; set ; }

        public TextSpan TokenSpan { get ; set ; }

        public bool StartOfLine { get ; set ; }

        public int CurLine { get ; set ; }

        public TextSpan currentSpan { get ; set ; }

        public TextBlock CurBlock { get ; set ; }

        public SemanticModel Model { get ; set ; }

        public Dictionary < object , ISpanViewModel > ActiveSpans { get ; set ; }

        public TextSpan SyntaxNodeSpan { get ; set ; }
    }
}