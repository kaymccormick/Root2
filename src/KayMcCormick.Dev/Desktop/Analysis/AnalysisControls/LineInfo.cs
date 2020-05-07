using System.Collections.Generic;
using System.Windows;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class LineInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int LineNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Offset { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<RegionInfo> Regions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Size Size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Point Origin { get; set; }
    }
}