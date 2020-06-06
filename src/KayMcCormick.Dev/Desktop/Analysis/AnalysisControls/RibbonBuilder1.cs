using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Autofac;
using KayMcCormick.Lib.Wpf;
using KayMcCormick.Lib.Wpf.Command;
using RibbonLib.Model;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonBuilder1
    {
        private readonly RibbonModelApplicationMenu _appMenu;
        private readonly IEnumerable<IRibbonModelProvider<RibbonModelTab>> _tabProviders;
        private readonly IEnumerable<IRibbonModelProvider<RibbonModelContextualTabGroup>> _grpProviders;
        private readonly Func<RibbonModelTab> RibbonTabFactory;
        private readonly ILifetimeScope _scope;
        private readonly JsonSerializerOptions _options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appMenu"></param>
        /// <param name="tabProviders"></param>
        /// <param name="grpProviders"></param>
        /// <param name="_options"></param>
        /// <param name="ribbonTabFactory"></param>
        /// <returns></returns>
        public RibbonBuilder1(RibbonModelApplicationMenu appMenu,
            IEnumerable<IRibbonModelProvider<RibbonModelTab>> tabProviders,
            IEnumerable<IRibbonModelProvider<RibbonModelContextualTabGroup>> grpProviders,
            JsonSerializerOptions _options, Func<RibbonModelTab> ribbonTabFactory, 
            ILifetimeScope scope)
        {
            _appMenu = appMenu;
            _tabProviders = tabProviders;
            _grpProviders = grpProviders;
            this._options = _options;
            RibbonTabFactory = ribbonTabFactory;
            _scope = scope;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PrimaryRibbonModel BuildRibbon()
        {
            var opt = new JsonSerializerOptions();
            foreach (var optionsConverter in _options.Converters) opt.Converters.Add(optionsConverter);

            opt.WriteIndented = true;
            opt.IgnoreNullValues = true;
            var appMenu = _appMenu;
            var r = new PrimaryRibbonModel
            {
                AppMenu = appMenu,
                HelpPaneContent = "help",
                QuickAccessToolBar =
                {
                    CustomizeMenuButton =
                        new RibbonModelItemMenuButton() {Command = WpfAppCommands.CustomizeQAT}
                }
            };
            r.QuickAccessToolBar.Items.Add(new RibbonModelButton
            {
                Label = "test1",
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
            var Group1 = new RibbonModelGroup { Header = "Paste" };
            foreach (var cmd in _scope.Resolve<IEnumerable<IDisplayableAppCommand>>())
            {
                if (cmd is ICommand cmdz)
                {
                    var button = new RibbonModelButton() { Label = cmd.DisplayName, Command = cmdz };
                    Group1.Items.Add(button);

                }
            }
            var PasteButton = new RibbonModelButton {Label = "Paste", Command = ApplicationCommands.Paste};
            
            Group1.Items.Add(new RibbonModelButton {Label = "Open", Command = ApplicationCommands.Open});
            var Group2 = new RibbonModelGroup { Header = "Context" };
            HomeTab.ItemsCollection.Add(Group2);

            Group1.Items.Add(PasteButton);
            HomeTab.ItemsCollection.Add(Group1);
            r.RibbonItems.Add(HomeTab);

            foreach (var ribbonModelProvider in _tabProviders)
            {
                var item = ribbonModelProvider.ProvideModelItem();
                item.RibbonModel = r;
                if (item.ContextualTabGroupHeader != null) item.Visibility = Visibility.Collapsed;
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