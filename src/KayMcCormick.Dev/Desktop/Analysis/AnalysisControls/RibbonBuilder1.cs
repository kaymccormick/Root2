using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;
using System.Xml;
using AnalysisControls.RibbonModel;
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
        /// <param name="appMenu"></param>
        /// <param name="ribbonModelContextualTabGroups"></param>
        /// <param name="tabs"></param>
        /// <param name="tabProviders"></param>
        /// <returns></returns>
        public static PrimaryRibbonModel RibbonModelBuilder(RibbonModelApplicationMenu appMenu, IEnumerable<RibbonModelContextualTabGroup> ribbonModelContextualTabGroups, IEnumerable<RibbonModelTab> tabs, IEnumerable<IRibbonModelProvider<RibbonModelTab>> tabProviders)
        {
            var r = new PrimaryRibbonModel {AppMenu = appMenu};
            r.QuickAccessToolbar.Items.Add(new RibbonModelItemButton {Label = "test1",
            SmallImageSource = "pack://application:,,,/WpfLib;component/Assets/ASPWebSite_16x.png"
            });

            foreach (var tg in ribbonModelContextualTabGroups)
            {
                DebugUtils.WriteLine("Adding contextual tab group " + tg);
                tg.RibbonModel = r;
                r.ContextualTabGroups.Add(tg);
            }

            r.AppMenu.Items.Add(new RibbonModelAppMenuItem {Header = "test"});
            foreach (var meta in tabs)
            {
                var ribbonModelTab = meta;
                ribbonModelTab.BeginInit();
                ribbonModelTab.EndInit();                
                r.RibbonItems.Add(ribbonModelTab);
                if (ribbonModelTab.ContextualTabGroupHeader != null)
                {
                    ribbonModelTab.Visibility = Visibility.Collapsed;
                }

                ribbonModelTab.RibbonModel = r;
            }

            foreach (var ribbonModelProvider in tabProviders)
            {
                var item = ribbonModelProvider.ProvideModelItem();
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