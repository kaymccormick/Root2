using System.ComponentModel;
using AnalysisControls.RibbonModel;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelRadioButton : RibbonModelItem
    {
        /// <summary>
        /// 
        /// 
        /// </summary>
        [DefaultValue(false)]

        public bool IsChecked { get; set; }

        public override ControlKind Kind => ControlKind.RadioButton;
    }
}