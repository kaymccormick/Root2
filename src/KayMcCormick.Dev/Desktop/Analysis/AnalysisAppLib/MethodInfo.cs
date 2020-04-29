using System.Collections.Generic;

namespace AnalysisAppLib
{
    public sealed class MethodInfo
    {
        private string                 _name ;
        private List < ParameterInfo > _params = new List < ParameterInfo > ( ) ;

        public MethodInfo ( string methodSymbolName , IEnumerable < ParameterInfo > select )
        {
            Name = methodSymbolName ;
            Parameters.AddRange ( select ) ;
        }

        public string Name { get { return _name ; } set { _name = value ; } }

        public List < ParameterInfo > Parameters
        {
            get { return _params ; }
            set { _params = value ; }
        }
    }
}