using System.Collections.Generic;
using System.Windows.Media;
using AnalysisAppLib;
using Autofac.Features.AttributeFilters;
using Autofac.Features.Metadata;
using KayMcCormick.Lib.Wpf;

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

            var g2 = CreateGroup("Dropone");
            g2.Items.Add(new RibbonModelDropZoneImpl() { Fill = Brushes.Red, MaxWidth = 80, MaxHeight = 80, MinWidth = 40, MinHeight = 40 });
            g2.Items.Add(new RibbonModelDropZone()
            {
                Fill = Brushes.Blue, MaxWidth = 80, MaxHeight = 80, MinWidth = 40, MinHeight = 40 ,
                Command = WpfAppCommands.ConvertToJson
            });
        }
    }
}