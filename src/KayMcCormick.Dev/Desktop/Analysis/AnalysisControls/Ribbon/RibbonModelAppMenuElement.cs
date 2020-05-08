using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonM
{
    public class RibbonModelAppMenuElement
    {
        public string Header { get; set; }
        public string KeyTip { get; set; }
        public object ImageSource { get; set; }

        public RibbonModelAppSplitMenuItem CreateSplitMenuItem(string header)
        {
            var r = new RibbonModelAppSplitMenuItem {Header = header};
            Items.Add(r);
            return r;
        }

        public RibbonModelAppMenuItem CreateAppMenuItem(string Header)
        {
            var r = new RibbonModelAppMenuItem {Header = Header};
            Items.Add(r);
            return r;
        }

        public ObservableCollection<RibbonModelAppMenuElement> Items { get; } =
            new ObservableCollection<RibbonModelAppMenuElement>();
    }
}