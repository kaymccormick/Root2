using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalysisControls.ViewModel;
using Autofac;
using Autofac.Core;
using JetBrains.Annotations;
using KayMcCormick.Dev.Application;

namespace AnalysisControls
{
    public class ControlsAppInstance : ApplicationInstance
    {
        private Main1Model _main1Model;

        public ControlsAppInstance([NotNull] ApplicationInstanceConfiguration applicationInstanceConfiguration) : base(applicationInstanceConfiguration)
        {
            foreach (var module in new IModule[] { new Client2Module1(), new AnalysisControlsModule() })
            {
                AddModule(module);
            }
        }

        public Main1Model Main1Model => _main1Model;
        public override void Startup()
        {
            _main1Model = GetLifetimeScope()?.Resolve<Main1Model>();
            base.Startup();
        }
    }
}
