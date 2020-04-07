using System.Collections.Generic ;

namespace FindLogUsages
{
    internal class TransformNode
    {
        public int RawKind { get ; set ; }

        public string Kind { get ; set ; }

        public IEnumerable < string > Tokens { get ; set ; }

        public string StringRepr { get ; set ; }

        public string Type { get ; set ; }
    }
}