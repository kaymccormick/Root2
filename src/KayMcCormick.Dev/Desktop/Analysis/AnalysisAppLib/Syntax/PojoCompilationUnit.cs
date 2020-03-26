using System.Collections.Generic ;



namespace AnalysisAppLib.Syntax
{
    public class PojoCompilationUnit
    {
        public PojoCompilationUnit ( ) {
        }

        public List < object > Usings { get ; }

        public List < object > ExternAliases { get ; }

        public List < object > AttributeLists { get ; }

        public List < object > Members { get ; }

        public PojoCompilationUnit (
            List < object > Usings
          , List < object > ExternAliases
          , List < object > AttributeLists
          , List < object > members
        )
        {
            this.Usings         = Usings ;
            this.ExternAliases  = ExternAliases ;
            this.AttributeLists = AttributeLists ;
            Members             = members ;
        }
    }
}