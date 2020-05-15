using System.Collections.Generic;
using System.Windows;
using AnalysisAppLib;
using Autofac.Features.Metadata;
using KayMcCormick.Dev;

namespace AnalysisControls.RibbonModel.Definition
{
    /// <summary>
    /// 
    /// </summary>
    public class AssembliesRibbonTab : RibbonModelTab
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groups"></param>
        public AssembliesRibbonTab(IEnumerable<Meta<RibbonModelGroup>> groups)
        {
            //Visibility = Visibility.Visible;
            ContextualTabGroupHeader = "Code Analysis";
            Header = "Assemblies";
            foreach (var ribbonModelGroup in groups)
            {
                var props = MetaHelper.GetMetadataProps(ribbonModelGroup.Metadata);
                DebugUtils.WriteLine($"{props.TabHeader} {ContextualTabGroupHeader}");
                if (props.TabHeader != null && props.TabHeader.Equals((string)Header))
                {
                    Items.Add(ribbonModelGroup.Value);
                }
            }
        }
    }
}
