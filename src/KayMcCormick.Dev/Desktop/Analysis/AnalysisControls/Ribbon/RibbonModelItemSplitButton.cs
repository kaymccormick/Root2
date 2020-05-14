using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonModel
{
    public class RibbonModelItemSplitButton : RibbonModelItem
    {
        public ObservableCollection<RibbonModelItem> Items { get; } = new ObservableCollection<RibbonModelItem>();

        public override ControlKind Kind => ControlKind.RibbonSplitButton;
    }
}