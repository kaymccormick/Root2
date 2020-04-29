using System;

namespace AnalysisControls
{
    public sealed class Filter
    {
        private string _extension ;
        private string _description ;

        public string Extension { get { return _extension ; } set { _extension = value ; } }

        public string Description { get { return _description ; } set { _description = value ; } }

        public Action < string > Handler { get ; set ; }
    }
}