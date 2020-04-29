using AnalysisAppLib;

namespace ProjInterface
{
    public interface IAppRibbonTab
    {
        CategoryInfo Category { get; set; }
        void AddComponent(IRibbonComponent value);
    }
}