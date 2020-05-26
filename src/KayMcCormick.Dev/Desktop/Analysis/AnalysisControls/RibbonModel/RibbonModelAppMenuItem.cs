using System.Windows.Input;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelAppMenuItem : RibbonModelAppMenuElement
    {
        public ICommand Command { get; set; }
    }
}