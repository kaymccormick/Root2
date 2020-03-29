#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// AppParameterInfo.cs
// 
// 2020-03-24-9:54 PM
// 
// ---
#endregion
using System ;

namespace AnalysisAppLib.Syntax
{
    public class AppParameterInfo
    {
        private int    _index ;
        private bool   _isOptional ;
        private string _name ;
        private Type   _parameterType ;

        public Type ParameterType
        {
            get { return _parameterType ; }
            set { _parameterType = value ; }
        }

        public bool IsOptional { get { return _isOptional ; } set { _isOptional = value ; } }

        public string Name { get { return _name ; } set { _name = value ; } }

        public int Index { get { return _index ; } set { _index = value ; } }
    }
}