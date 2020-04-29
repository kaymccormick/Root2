using System.Windows.Controls.Ribbon;
using AvalonDock.Layout;

namespace AnalysisControls
{
    public class RibbonBuilder
    {
        private IAppRibbon appRibbon;
        private readonly AllCommands _allCommands;
        private Ribbon _ribbon;

        public RibbonBuilder(IAppRibbon appRibbon, AllCommands allCommands)
        {
            this.appRibbon = appRibbon;
            _allCommands = allCommands;
        }

        public Ribbon Ribbon
        {
            get
            {
                if (_ribbon == null)
                {
                    _ribbon = BuildRibbon();
                }

                return _ribbon;
            }
            set
            {
                _ribbon = value;
            }
        }

        public LayoutDocumentPane DocPane { get; set; }

        public Ribbon BuildRibbon()
        {
            Ribbon r = new Ribbon();
            _allCommands.DocPane = DocPane;
            foreach (var appRibbonTab in appRibbon.Tabs)
            {
                var ribbonTab = new RibbonTab();
                ribbonTab.Header = appRibbonTab.Category.Category.ToString();
                r.Items.Add(ribbonTab);
                
                ribbonTab.Items.Add(_allCommands.GetComponent());
            }

            return r;
        }
    }
}