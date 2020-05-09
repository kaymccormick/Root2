using AnalysisControls.RibbonM;
using Autofac;

namespace Client2
{
    public abstract class RibbonViewGroupProviderBase : IRibbonModelProvider<RibbonModelGroup>
    {
        public abstract RibbonModelGroup ProvideModelItem(IComponentContext context);

    }
}