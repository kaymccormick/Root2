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

namespace FindLogUsages
{
    public class PojoClassDeclarationSyntax
    {
        public PojoClassDeclarationSyntax (
            PocoSyntaxToken identifier
          , List < object >    members
        )
        {
            Identifier = identifier ;
            Members    = members ;
        }

        public PocoSyntaxToken Identifier { get ; }

        public List < object > Members { get ; }
    }
}