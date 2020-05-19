using System.Collections.Generic;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MethodInfo
    {
        private string                 _name ;
        private List < ParameterInfo > _params = new List < ParameterInfo > ( ) ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodSymbolName"></param>
        /// <param name="select"></param>
        public MethodInfo ( string methodSymbolName , IEnumerable < ParameterInfo > select )
        {
            Name = methodSymbolName ;
            Parameters.AddRange ( select ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get { return _name ; } set { _name = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public List < ParameterInfo > Parameters
        {
            get { return _params ; }
            set { _params = value ; }
        }
    }
}