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
        public bool IsChecked { get; set; }

        public override ControlKind Kind => ControlKind.RadioButton;
    }
}