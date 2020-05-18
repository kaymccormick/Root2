using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;
using System.Xml;
using AnalysisControls.RibbonModel;
using Autofac;
using Autofac.Core;
using Autofac.Features.Metadata;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public static class RibbonBuilder1
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public static PrimaryRibbonModel RibbonModelBuilder(IComponentContext c, IEnumerable<Parameter> o)
        {
            var r = new PrimaryRibbonModel {AppMenu = c.Resolve<RibbonModelApplicationMenu>()};
            foreach (var tg in c.Resolve<IEnumerable<RibbonModelContextualTabGroup>>())
            {
                DebugUtils.WriteLine("Adding contextual tab group " + tg);
                tg.RibbonModel = r;
                r.ContextualTabGroups.Add(tg);
            }

            r.AppMenu.Items.Add(new RibbonModelAppMenuItem {Header = "test"});
            var tabs = c.Resolve<IEnumerable<Meta<Lazy<RibbonModelTab>>>>();
            foreach (var meta in tabs)
            {
                var ribbonModelTab = meta.Value.Value;
                ribbonModelTab.BeginInit();
                ribbonModelTab.EndInit();                
                r.RibbonItems.Add(ribbonModelTab);
                if (ribbonModelTab.ContextualTabGroupHeader != null)
                {
                    ribbonModelTab.Visibility = Visibility.Collapsed;
                }

                ribbonModelTab.RibbonModel = r;
            }

            var tabProviders = c.Resolve<IEnumerable<IRibbonModelProvider<RibbonModelTab>>>();
            foreach (var ribbonModelProvider in tabProviders)
            {
                var item = ribbonModelProvider.ProvideModelItem(c);
                item.RibbonModel = r;
                if (item.ContextualTabGroupHeader != null)
                {
                    item.Visibility = Visibility.Collapsed;
                }
                r.RibbonItems.Add(item);
            }

            var file = @"C:\temp\ribbon.xaml";
            using (var w = XmlWriter.Create(file, new XmlWriterSettings() {Indent = true}))
            {
                ResourceDictionary d = new ResourceDictionary();
                d["Ribbon0"] = r;
                XamlWriter.Save(d, w);
            }
            
            return r;
        }
    }
}