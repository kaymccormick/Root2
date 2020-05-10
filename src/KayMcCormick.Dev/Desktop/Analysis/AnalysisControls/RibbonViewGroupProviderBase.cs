using AnalysisControls.RibbonM;
using Autofac;

namespace AnalysisControls
{
    public abstract class RibbonViewGroupProviderBase : IRibbonModelProvider<RibbonModelGroup>
    {
        public abstract RibbonModelGroup ProvideModelItem(IComponentContext context);

    }
}