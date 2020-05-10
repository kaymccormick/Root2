using Autofac;

namespace AnalysisControls
{
    public interface IRibbonModelProvider<T>
    {
        T ProvideModelItem(IComponentContext context);
    }
}