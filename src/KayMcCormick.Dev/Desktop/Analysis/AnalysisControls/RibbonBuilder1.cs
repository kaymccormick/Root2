using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml;
using AnalysisControls.RibbonModel;
using Autofac.Features.Metadata;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonBuilder1
    {
        private readonly RibbonModelApplicationMenu _appMenu;
        private readonly IEnumerable<RibbonModelContextualTabGroup> _ribbonModelContextualTabGroups;
        private readonly IEnumerable<RibbonModelTab> _tabs;
        private readonly IEnumerable<IRibbonModelProvider<RibbonModelTab>> _tabProviders;
        private readonly IEnumerable<IRibbonModelProvider<RibbonModelContextualTabGroup>> _grpProviders;
        private Func<RibbonModelTab> RibbonTabFactory;
        private JsonSerializerOptions _options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appMenu"></param>
        /// <param name="ribbonModelContextualTabGroups"></param>
        /// <param name="tabs"></param>
        /// <param name="tabProviders"></param>
        /// <param name="_options"></param>
        /// <returns></returns>
        public RibbonBuilder1(RibbonModelApplicationMenu appMenu,
            IEnumerable<RibbonModelContextualTabGroup> ribbonModelContextualTabGroups, IEnumerable<RibbonModelTab> tabs,
            IEnumerable<IRibbonModelProvider<RibbonModelTab>> tabProviders,
            IEnumerable<IRibbonModelProvider<RibbonModelContextualTabGroup>> grpProviders,
            JsonSerializerOptions _options, Func<RibbonModelTab> ribbonTabFactory)
        {
            _appMenu = appMenu;
            _ribbonModelContextualTabGroups = ribbonModelContextualTabGroups;
            _tabs = tabs;
            _tabProviders = tabProviders;
            _grpProviders = grpProviders;
            this._options = _options;
            RibbonTabFactory = ribbonTabFactory;
        }

        public PrimaryRibbonModel BuildRibbon(){
        JsonSerializerOptions opt= new JsonSerializerOptions();
            foreach (var optionsConverter in _options.Converters)
            {
                opt.Converters.Add(optionsConverter);
            }

            opt.WriteIndented = true;
            opt.IgnoreNullValues = true;
            RibbonModelApplicationMenu appMenu=_appMenu;
            var r = new PrimaryRibbonModel {AppMenu = appMenu, HelpPaneContent = "help"};
            r.QuickAccessToolBar.CustomizeMenuButton = new RibbonModelItemMenuButton() { Command = WpfAppCommands.CustomizeQAT };
            r.QuickAccessToolBar.Items.Add(new RibbonModelItemButton {Label = "test1",
            SmallImageSource = "pack://application:,,,/WpfLib;component/Assets/ASPWebSite_16x.png"
            });
            var dockPanel = new DockPanel {LastChildFill = false};
          //  dockPanel.Children.Add(new RibbonButton {Label = "Quit"});
            appMenu.FooterPaneContent = dockPanel;
            var ribbonModelProviders = _grpProviders;
            foreach (var ribbonModelProvider in ribbonModelProviders)
            {
	    var t = ribbonModelProvider.ProvideModelItem();
	    t.RibbonModel = r;
                r.ContextualTabGroups.Add(t);
		
            }

            r.AppMenu.Items.Add(new RibbonModelAppMenuItem {Header = "Open", Command = ApplicationCommands.Open});

            var HomeTab = RibbonTabFactory();
        HomeTab.Header = "Home";
        var PasteButton = new RibbonModelItemButton { Label = "Paste", Command = ApplicationCommands.Paste };
	    var Group1 = new RibbonModelGroup { Header = "Paste" };
        Group1.Items.Add(new RibbonModelItemButton() {Label = "Open", Command = ApplicationCommands.Open});

        
	    Group1.Items.Add(PasteButton);
	    HomeTab.ItemsCollection.Add(Group1);
	    r.RibbonItems.Add(HomeTab);
	    
            foreach (var ribbonModelProvider in _tabProviders)
            {
                var item = ribbonModelProvider.ProvideModelItem();
                item.RibbonModel = r;
                if (item.ContextualTabGroupHeader != null)
                {
                    item.Visibility = Visibility.Collapsed;
                }
                r.RibbonItems.Add(item);
            }

            // var file = @"C:\temp\ribbon.xaml";
            // using (var w = XmlWriter.Create(file, new XmlWriterSettings() {Indent = true}))
            // {
                // ResourceDictionary d = new ResourceDictionary();
                // d["Ribbon0"] = r;
                // XamlWriter.Save(d, w);
            // }

            // var json = JsonSerializer.Serialize(r, opt);
            // File.WriteAllText(@"C:\temp\ribbon.json", json);

            return r;
        }
    }

    // public class DirMenuList : IRibbonMenuButton
    // {

    // }
}