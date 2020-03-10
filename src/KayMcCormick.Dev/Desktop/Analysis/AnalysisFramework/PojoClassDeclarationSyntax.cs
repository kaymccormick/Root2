#region header
// Kay McCormick (mccor)
// 
// AnalyzeConsole
// AnalysisFramework
// PojoClassDeclarationSyntax.cs
// 
// 2020-03-09-9:56 PM
// 
// ---
#endregion
using System.Collections.Generic ;

namespace AnalysisFramework
{
    public class PojoClassDeclarationSyntax
    {
        public PocoSyntaxToken Identifier { get ; }

        public List < object > Members { get ; }

        public PojoClassDeclarationSyntax (
            in PocoSyntaxToken identifier
          , List < object >    members
        )
        {
            Identifier = identifier ;
            Members    = members ;
        }
    }
}