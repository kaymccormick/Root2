using System.ComponentModel;
using System.Windows.Media;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelTwoLineText : RibbonModelItem
    {
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public Geometry PathData
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public Brush PathFill
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public Brush PathStroke { get; set; } = Brushes.Black;

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        public bool HasTwoLines
        {
            get;
            set;
        }

        public override ControlKind Kind => ControlKind.RibbonTwoLine;
    }
}