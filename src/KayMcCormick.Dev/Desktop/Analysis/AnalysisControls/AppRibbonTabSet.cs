using System.Collections.Generic;

namespace AnalysisControls
{
    public class AppRibbonTabSet
    {
        public ISet<IAppRibbonTab> TabSet { get; } = new HashSet<IAppRibbonTab>();

    }
}