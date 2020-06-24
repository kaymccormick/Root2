using RibbonLib.Model;

namespace AnalysisControls
{
    abstract class RibbonModelTabProbider1 : IRibbonModelProvider<RibbonModelTab>
    {
        /// <inheritdoc />
        public abstract RibbonModelTab ProvideModelItem();

        /// <inheritdoc />
        public object InstanceObjectId { get; set; }
    }
}