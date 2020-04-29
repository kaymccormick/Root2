using System.Collections.Generic;

namespace AnalysisControls
{
    public interface IAppRibbon
    {
        IEnumerable<IAppRibbonTab> Tabs { get; }
    }
}