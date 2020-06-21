using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

    public class RibbonModelTanProviderCollection : ObservableCollection<IRibbonModelProvider<RibbonModelTab>>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class RibbonBuilder1 : FrameworkElement
    {
        public static readonly DependencyProperty RibbonModelTabProvidersProperty = DependencyProperty.Register(
            "RibbonModelTabProviders", typeof(IEnumerable<IRibbonModelProvider<RibbonModelTab>>), typeof(RibbonBuilder1), new PropertyMetadata(default(IEnumerable<IRibbonModelProvider<RibbonModelTab>>)));

        // public static readonly DependencyPropertyKey TabsPropertyKey = DependencyProperty.RegisterReadOnly(
            // "Tabs", typeof(RibbonModelTabCollection), typeof(RibbonBuilder1), new PropertyMetadata(default(RibbonModelTabCollection)));
        // public static readonly DependencyProperty TabsProperty = TabsPropertyKey.DependencyProperty;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonModelTabCollection Tabs { get; set; }
        public IEnumerable<IRibbonModelProvider<RibbonModelTab>> RibbonModelTabProviders
        {
            get { return (IEnumerable<IRibbonModelProvider<RibbonModelTab>>) GetValue(RibbonModelTabProvidersProperty); }
            set { SetValue(RibbonModelTabProvidersProperty, value); }
        }
        private readonly RibbonModelApplicationMenu _appMenu;
        private readonly IEnumerable<IRibbonModelProvider<RibbonModelTab>> _tabProviders;
        private readonly IEnumerable<IRibbonModelProvider<RibbonModelContextualTabGroup>> _grpProviders;
        private readonly Func<RibbonModelTab> RibbonTabFactory;
        private readonly ILifetimeScope _scope;
        private readonly JsonSerializerOptions _options;


        public static readonly DependencyPropertyKey RibbonModelPropertyKey = DependencyProperty.RegisterReadOnly(
            "RibbonModel", typeof(PrimaryRibbonModel), typeof(RibbonBuilder1), new PropertyMetadata(default(PrimaryRibbonModel)));

        public static readonly DependencyProperty RibbonModelProperty = RibbonModelPropertyKey.DependencyProperty;

        public PrimaryRibbonModel RibbonModel
        {
            get { return (PrimaryRibbonModel) GetValue(RibbonModelProperty); }
            set { SetValue(RibbonModelProperty, value); }
        }

        public RibbonBuilder1()
        {
            SetValue(RibbonModelPropertyKey, new PrimaryRibbonModel());
            Tabs = new RibbonModelTabCollection();
//            SetValue(TabsPropertyKey, new RibbonModelTabCollection());
        }
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
            RibbonModelTabProviders = tabProviders;
            _grpProviders = grpProviders;
            this._options = _options;
            RibbonTabFactory = ribbonTabFactory;
            _scope = scope;
        }

        public override void BeginInit()
        {
            base.BeginInit();
        }

        public override void EndInit()
        {
            base.EndInit();
            BuildRibbon();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PrimaryRibbonModel BuildRibbon()
        {
            // var opt = new JsonSerializerOptions();
            // foreach (var optionsConverter in _options.Converters) opt.Converters.Add(optionsConverter);
            //
            // opt.WriteIndented = true;
            // opt.IgnoreNullValues = true;
            var appMenu = _appMenu;
            var r = RibbonModel;
            if (r == null)
            {
                r = new PrimaryRibbonModel();
                SetValue(RibbonModelPropertyKey, r);
            }

            // var r = new PrimaryRibbonModel
            // {
                // AppMenu = appMenu,
                // HelpPaneContent = "help",
                // QuickAccessToolBar =
                // {
                    // CustomizeMenuButton =
                        // new RibbonModelItemMenuButton() {Command = WpfAppCommands.CustomizeQAT}
                // }
            //};
            if (r.QuickAccessToolBar != null)
                r.QuickAccessToolBar.Items.Add(new RibbonModelButton
                {
                    Label = "test1",
                    SmallImageSource = "pack://application:,,,/WpfLib;component/Assets/ASPWebSite_16x.png"
                });
            var dp = new ModelDatePicker();
            var dockPanel = new DockPanel {LastChildFill = false};
            //  dockPanel.Children.Add(new RibbonButton {Label = "Quit"});
            if (appMenu != null) appMenu.FooterPaneContent = dockPanel;
            var ribbonModelProviders = _grpProviders;
            if (ribbonModelProviders != null)
                foreach (var ribbonModelProvider in ribbonModelProviders)
                {
                    var t = ribbonModelProvider.ProvideModelItem();
                    t.RibbonModel = r;
                    r.ContextualTabGroups.Add(t);
                }


            if (r.AppMenu != null)
                r.AppMenu.Items.Add(new RibbonModelAppMenuItem {Header = "Open", Command = ApplicationCommands.Open});

            if (RibbonTabFactory != null)
            {
                var HomeTab = RibbonTabFactory();
                HomeTab.Header = "Home";
                var Group1 = new RibbonModelGroup { Header = "Paste" };
                if (_scope != null)
                    foreach (var cmd in _scope.Resolve<IEnumerable<IDisplayableAppCommand>>())
                    {
                        if (cmd is ICommand cmdz)
                        {
                            var button = new RibbonModelButton() {Label = cmd.DisplayName, Command = cmdz};
                            Group1.Items.Add(button);
                        }
                        else
                        {

                        }
                    }

                var PasteButton = new RibbonModelButton {Label = "Paste", Command = ApplicationCommands.Paste};
            
                Group1.Items.Add(new RibbonModelButton {Label = "Open", Command = ApplicationCommands.Open});
                var Group2 = new RibbonModelGroup { Header = "Context" };
                HomeTab.ItemsCollection.Add(Group2);
                var Group3 = new RibbonModelGroup() {Header = "Random"};
                HomeTab.ItemsCollection.Add(Group3);
                Group3.Items.Add(new RibbonModelControl() {Content = dp});
                Group1.Items.Add(PasteButton);
                HomeTab.ItemsCollection.Add(Group1);
                r.RibbonItems.Add(HomeTab);
            }

            if (Tabs != null)
                foreach (RibbonModelTab ribbonModelTab in Tabs)
                {
                    r.RibbonItems.Add(ribbonModelTab);
                }

            if (RibbonModelTabProviders != null)
                foreach (var ribbonModelProvider in RibbonModelTabProviders)
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

    public class RibbonModelControl : RibbonModelItem
    {
        /// <inheritdoc />
        public override ControlKind Kind { get; }=ControlKind.RibbonContentPresenter;

        public object Content { get; set; }
    }

    // public class DirMenuList : IRibbonMenuButton
    // {

    // }
}