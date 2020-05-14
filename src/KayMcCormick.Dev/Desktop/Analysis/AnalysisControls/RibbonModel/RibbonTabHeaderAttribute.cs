using System;
using System.ComponentModel.Composition;

namespace AnalysisControls.RibbonModel
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