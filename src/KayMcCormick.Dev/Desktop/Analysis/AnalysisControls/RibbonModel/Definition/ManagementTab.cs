using System.Collections.Generic;
using AnalysisAppLib;
using Autofac.Features.AttributeFilters;
using Autofac.Features.Metadata;

namespace AnalysisControls.RibbonModel.Definition
{
    /// <summary>
    /// 
    /// </summary>
    [CategoryMetadata(Category.Management)]
    public class ManagementTab : RibbonModelTab
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="typesViewModel"></param>
        public ManagementTab([MetadataFilter("Category", Category.Management)]IEnumerable<Meta<RibbonModelGroup>> groups, ITypesViewModel typesViewModel)
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