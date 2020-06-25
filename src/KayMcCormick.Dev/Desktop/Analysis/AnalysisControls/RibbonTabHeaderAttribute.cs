using System;
using System.Composition;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    [MetadataAttribute]
    public class RibbonTabHeaderAttribute : Attribute
    {
        private readonly string _tabHeader;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabHeader"></param>
        public RibbonTabHeaderAttribute(string tabHeader)
        {
            _tabHeader = tabHeader;
        }

        /// <summary>
        /// 
        /// </summary>
        public string TabHeader
        {
            get { return _tabHeader; }
        }
    }
}