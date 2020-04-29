using System.Collections.Generic;
using JetBrains.Annotations;

namespace AnalysisAppLib
{
    public sealed class CallerInfo
    {
        private List < LocationInfo > _locations = new List < LocationInfo > ( ) ;

        public string CalledSymbol { get ; }

        public string CallingSymbol { get ; }

        public bool IsDirect { get ; }

        public CallerInfo (
            string                                   calledSymbol
            , string                                   callingSymbol
            , bool                                     isDirect
            , [ NotNull ] IEnumerable < LocationInfo > select
        )
        {
            CalledSymbol  = calledSymbol ;
            CallingSymbol = callingSymbol ;
            IsDirect      = isDirect ;
            Locations.AddRange ( select ) ;
        }

        public List < LocationInfo > Locations
        {
            get { return _locations ; }
            set { _locations = value ; }
        }
    }
}