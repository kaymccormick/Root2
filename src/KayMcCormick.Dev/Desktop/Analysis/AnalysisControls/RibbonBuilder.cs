using System.Windows.Controls.Ribbon;

namespace AnalysisControls
{
    public class RibbonBuilder
    {
        private IAppRibbon appRibbon;
        private Ribbon _ribbon;

        public RibbonBuilder(IAppRibbon appRibbon)
        {
            this.appRibbon = appRibbon;
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

        public Ribbon BuildRibbon()
        {
            Ribbon r = new Ribbon();
            foreach (var appRibbonTab in appRibbon.Tabs)
            {
                var ribbonTab = new RibbonTab();
                ribbonTab.Header = appRibbonTab.Category.ToString();
                r.Items.Add(ribbonTab);
            }

            return r;
        }
    }
}