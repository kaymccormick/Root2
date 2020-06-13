using System.Collections.Generic;
using JetBrains.Annotations;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CallerInfo
    {
        private List < LocationInfo > _locations = new List < LocationInfo > ( ) ;

        /// <summary>
        /// 
        /// </summary>
        public string CalledSymbol { get ; }

        /// <summary>
        /// 
        /// </summary>
        public string CallingSymbol { get ; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDirect { get ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="calledSymbol"></param>
        /// <param name="callingSymbol"></param>
        /// <param name="isDirect"></param>
        /// <param name="select"></param>
        public CallerInfo (
            string                                   calledSymbol
            , string                                   callingSymbol
            , bool                                     isDirect
            , [ JetBrains.Annotations.NotNull ] IEnumerable < LocationInfo > select
        )
        {
            CalledSymbol  = calledSymbol ;
            CallingSymbol = callingSymbol ;
            IsDirect      = isDirect ;
            Locations.AddRange ( select ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public List < LocationInfo > Locations
        {
            get { return _locations ; }
            set { _locations = value ; }
        }
    }
}