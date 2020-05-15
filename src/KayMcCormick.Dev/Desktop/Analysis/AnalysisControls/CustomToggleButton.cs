using System.Windows.Controls.Primitives;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomToggleButton : ToggleButton
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void OnToggle()
        {
            DebugUtils.WriteLine("OnToggle");
        }
    }
}