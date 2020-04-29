using System.Collections.Generic;

namespace ProjInterface
{
    public class AppRibbonTabSet
    {
        public ISet<IAppRibbonTab> TabSet { get; } = new HashSet<IAppRibbonTab>();

    }
}