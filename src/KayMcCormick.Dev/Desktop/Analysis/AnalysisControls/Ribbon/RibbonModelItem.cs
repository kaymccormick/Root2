using System.Windows.Input;

namespace AnalysisControls.RibbonM
{
    public class RibbonModelItem
    {
        public object Label { get; set; }
        public ICommand Command { get; set; }
        public object CommandTarget { get; set; }
        public object CommandParameter { get; set; }
    }
}