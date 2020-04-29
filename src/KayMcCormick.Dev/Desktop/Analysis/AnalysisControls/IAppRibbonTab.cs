namespace AnalysisControls
{
    public interface IAppRibbonTab
    {
        CategoryInfo Category { get; set; }
        void AddComponent(IRibbonComponent value);
    }
}