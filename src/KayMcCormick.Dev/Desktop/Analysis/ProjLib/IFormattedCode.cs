#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// IFormattedCode.cs
// 
// 2020-03-02-2:49 PM
// 
// ---
#endregion
using System.Threading.Tasks ;
using System.Windows.Documents ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace ProjLib
{
    public interface IFormattedCode
    {
        string SourceCode { get ; set ; }

        SyntaxTree SyntaxTree { get ; set ; }

        SemanticModel Model { get ; set ; }

        CompilationUnitSyntax CompilationUnitSyntax { get ; set ; }

        Task<object> Refresh ( ) ;
        Task<object> VisitAsync ( ) ;
    }
}