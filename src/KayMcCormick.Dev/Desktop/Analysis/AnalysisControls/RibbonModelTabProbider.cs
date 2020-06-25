using System;
using RibbonLib.Model;

namespace AnalysisControls
{
    abstract class RibbonModelTabProvider1 : IRibbonModelProvider<RibbonModelTab>
    {
        private Func<RibbonModelTab> factory;

        protected RibbonModelTabProvider1(Func<RibbonModelTab> factory)
        {
            this.Factory = factory;
        }

        /// <inheritdoc />
        public abstract RibbonModelTab ProvideModelItem();

        /// <inheritdoc />
        public object InstanceObjectId { get; set; }

        public Func<RibbonModelTab> Factory
        {
            get { return factory; }
            set { factory = value; }
        }
    }
}