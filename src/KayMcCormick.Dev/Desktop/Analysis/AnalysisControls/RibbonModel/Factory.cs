using Autofac;

namespace AnalysisControls.RibbonModel
{
    public class Factory
    {

        public static RibbonModelTab CreateInstance(IComponentContext context)
        {
            return new RibbonModelTab();
        }
    }
}