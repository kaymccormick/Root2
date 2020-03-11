using System.Collections.Generic ;

namespace AnalysisFramework
{
    class TransformNode
    {
        private string type ;

        public int                 RawKind    { get ;                 set ; }
        public string              Kind       { get ;                 set ; }
        public IEnumerable<string> Tokens     { get ;                 set ; }
        public string              StringRepr { get ;                 set ; }
        public string              Type       { get { return type ; } set { type = value ; } }
    }
}