using System.ComponentModel;

namespace RibbonLib.Model
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