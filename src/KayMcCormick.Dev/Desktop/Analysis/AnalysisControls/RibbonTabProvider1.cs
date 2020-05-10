using System.Collections.Generic;
using System.Linq;
using AnalysisControls.RibbonM;
using Autofac;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonTabProvider1 : IRibbonModelProvider<RibbonModelTab>
    {
        /// <inheritdoc />
        public RibbonModelTab ProvideModelItem(IComponentContext context)
        {
            var tab = new RibbonModelTab { Header = "Fun tab"};
            tab.CreateGroup("Group 1");
            if (context != null)
            {
                var provs = context.Resolve<IEnumerable<IRibbonModelProvider<RibbonModelGroup>>>();
                if (provs.Any())
                {
                    foreach (var ribbonModelProvider in provs)
                    {
                        var item = ribbonModelProvider.ProvideModelItem(context);
                        tab.Items.Add(item);
                    }
                }
                else
                {
                    DebugUtils.WriteLine("No providers for tab item groups");
                }
            }

            return tab;
        }
    }
}