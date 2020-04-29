using System.Collections.Generic;

namespace ProjInterface
{
    public interface IAppRibbon
    {
        IEnumerable<IAppRibbonTab> Tabs { get; }
    }
}