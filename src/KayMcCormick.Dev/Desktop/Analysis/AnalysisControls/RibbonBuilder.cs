using System.Collections;
using System.Windows.Controls.Ribbon;
using AvalonDock.Layout;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonBuilder
    {
        private readonly IAppRibbon appRibbon;
        private readonly AllCommands _allCommands;
        private Ribbon _ribbon;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appRibbon"></param>
        /// <param name="allCommands"></param>
        public RibbonBuilder(IAppRibbon appRibbon, AllCommands allCommands)
        {
            this.appRibbon = appRibbon;
            _allCommands = allCommands;
        }

        /// <summary>
        /// 
        /// </summary>
        public Ribbon Ribbon
        {
            get
            {
                // ReSharper disable once ConvertIfStatementToNullCoalescingExpression
                if (_ribbon == null)
                {
                    _ribbon = BuildRibbon();
                }

                return _ribbon;
            }
            set { _ribbon = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public LayoutDocumentPane DocPane { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Ribbon BuildRibbon()
        {
            Ribbon  r = new Ribbon ();
            _allCommands.DocPane = DocPane;
            foreach (var appRibbonTab in appRibbon.Tabs)
            {
                var ribbonTab = new RibbonTab {Header = appRibbonTab.Category.Category.ToString()};
                r.Items.Add(ribbonTab);

                var newItem = _allCommands.GetComponent();
                if (newItem is IEnumerable v)
                {
                    foreach (var o in v)
                    {
                        ribbonTab.Items.Add(o);
                    }
                }
                else
                {
                    ribbonTab.Items.Add(newItem);
                }
            }

            return r;
        }
    }
}