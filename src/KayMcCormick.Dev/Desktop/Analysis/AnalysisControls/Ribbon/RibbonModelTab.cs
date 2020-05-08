using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonM
{
    public class RibbonModelTab
    {
        public string Header { get; set; }
        public ObservableCollection<RibbonModelItem> Items { get; } = new ObservableCollection<RibbonModelItem>();

        public RibbonModelTabItemGroup CreateGroup(string @group)
        {
            var r = new RibbonModelTabItemGroup() {Header = @group};
            Items.Add(r);
            return r;
        }

        public override string ToString()
        {
            return $"RibbonModelTab[{Header}]";
        }
    }
}