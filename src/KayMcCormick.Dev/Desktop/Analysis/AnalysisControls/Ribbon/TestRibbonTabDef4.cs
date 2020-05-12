﻿using System.Collections.Generic;
using System.Windows;
using AnalysisAppLib;
using Autofac.Features.Metadata;
using KayMcCormick.Dev;

namespace AnalysisControls.RibbonM
{
    public class TestRibbonTabDef4 : RibbonModelTab
    {
        
        public TestRibbonTabDef4(IEnumerable<Meta<RibbonModelGroup>> groups)
        {
            Visibility = Visibility.Visible;
            ContextualTabGroupHeader = "Assemblies";
            Header = "Derp";
            foreach (var ribbonModelGroup in groups)
            {
                // var props = MetaHelper.GetMetadataProps(ribbonModelGroup.Metadata);
                // DebugUtils.WriteLine($"{props.TabHeader} {ContextualTabGroupHeader}");
                // if (props.TabHeader != null && props.TabHeader.Equals((string)Header))
                // {
                    Items.Add(ribbonModelGroup.Value);
                // }
                break;
            }
        }
    }
}