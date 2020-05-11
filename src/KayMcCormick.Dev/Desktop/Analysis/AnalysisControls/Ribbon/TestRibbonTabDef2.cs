using System.Collections.Generic;
using System.Linq;
using AnalysisAppLib;
using Autofac.Features.AttributeFilters;
using Autofac.Features.Metadata;

namespace AnalysisControls.RibbonM
{
    [CategoryMetadata(Category.Management)]
    public class TestRibbonTabDef2 : RibbonModelTab
    {

        public TestRibbonTabDef2([MetadataFilter("Category", Category.Management)]IEnumerable<Meta<RibbonModelGroup>> groups, ITypesViewModel typesViewModel)
        {
            Header = Category.Management;
            foreach (var ribbonModelGroup in groups)
            {
                var props = MetaHelper.GetMetadataProps(ribbonModelGroup.Metadata);
                if (props.Category == Category.Infrastructure)
                {
                    Items.Add(ribbonModelGroup.Value);
                }
                else
                {

                }
            }

            var g = CreateGroup("Types");
            var menu = g.CreateRibbonMenuButton("Types");
            foreach (var appTypeInfo in typesViewModel.Root.SubTypeInfos) {
                
                    menu.CreateMenuItem(appTypeInfo.Title);
                
            }
        }
    }
}