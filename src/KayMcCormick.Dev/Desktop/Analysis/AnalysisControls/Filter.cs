using System;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Filter
    {
        private string _extension ;
        private string _description ;

        /// <summary>
        /// 
        /// </summary>
        public string Extension { get { return _extension ; } set { _extension = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get { return _description ; } set { _description = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public Action < string > Handler { get ; set ; }
    }
}