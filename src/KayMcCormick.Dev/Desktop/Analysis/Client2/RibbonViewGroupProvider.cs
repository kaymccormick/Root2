using System;
using System.Reactive.Subjects;
using AnalysisControls;
using AnalysisControls.RibbonM;
using Autofac;
using KayMcCormick.Lib.Wpf;

namespace Client2
{
    public class RibbonViewGroupProvider : RibbonViewGroupProviderBase
    {
        private readonly ReplaySubject<IControlView> _subj;

        public RibbonViewGroupProvider(ReplaySubject<IControlView> subj)
        {
            _subj = subj;
        }

        private void OnFaultDelegate(AggregateException obj)
        {
        }

        public override RibbonModelGroup ProvideModelItem(IComponentContext context)
        {
            var g = new RibbonModelGroup(){Header="oop"};
            return g;
        }
    }
}