using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonM
{
    public class RibbonModelTab
    {
        public string Header { get; set; }
        public ObservableCollection<RibbonModelItem> Items { get; } = new ObservableCollection<RibbonModelItem>();

        public RibbonModelGroup CreateGroup(string @group)
        {
            var r = new RibbonModelGroup() {Header = @group};
            Items.Add(r);
            return r;
        }

        public override string ToString()
        {
            return $"RibbonModelTab[{Header}]";
        }
    }
}