using System.ComponentModel;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelToggleButton : RibbonModelItem
    {
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        public bool IsChecked

        {
            get;
            set;
        }

        public override ControlKind Kind => ControlKind.RibbonToggleButton;
    }
}