using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using AnalysisAppLib;
using AnalysisControls;
using AnalysisControls.RibbonM;
using Autofac;
using Autofac.Core;
using Autofac.Features.Metadata;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Lib.Wpf;
using Microsoft.Graph;
using NLog;

namespace Client2
{
    /// <summary>
    /// Interaction logic for Client2Window1.xaml
    /// </summary>
    [ShortKeyMetadata("Client2Window1")]
    public partial class Client2Window1 : RibbonWindow, IView<ClientModel>, INotifyPropertyChanged
    {
        private ClientModel _viewModel;
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public Client2Window1()
        {
            InitializeComponent();
            throw new InvalidOperationException();
            ViewModel = new ClientModel(new RibbonModel(), new ReplaySubject<IControlView>());
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public Client2Window1(ILifetimeScope scope, ClientModel viewModel, MyCacheTarget2 myCacheTarget)
        {
            SetValue(AttachedProperties.LifetimeScopeProperty, scope);
            InitializeComponent();
            ViewModel = viewModel;

            myCacheTarget?.Cache.SubscribeOn(Scheduler.Default)
                .Buffer(TimeSpan.FromMilliseconds(100))
                .Where(x => x.Any())
                .ObserveOnDispatcher(DispatcherPriority.Background)
                .Subscribe(
                    infos =>
                    {
                        foreach (var logEvent in infos)
                        {
                            var i =
                                JsonSerializer.Deserialize<LogEventInstance>(
                                    logEvent
                                    , new
                                        JsonSerializerOptions()
                                );
                            ViewModel.LogEntries.Add(i);
                        }
                    }
                );
            //Task.Run(Action);
            var logControl = new LogEventInstancesControl();
            logControl.SetBinding(LogEventInstancesControl.EventsSourceProperty,
                new Binding("ViewModel.LogEntries") {Source = this});
        }

        private void Action()
        {
            for (;;)
            {
                LogManager.GetCurrentClassLogger().Info("My event");
                Thread.Sleep(30000);
            }
        }

        public ClientModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                OnPropertyChanged();

                if (_viewModel != null)
                {
                    _viewModel.Main1Model = Main1.ViewModel;
                    Main1.ViewModel.ClientViewModel = _viewModel;
                    DumpRibbon(_ribbon);
                

                foreach (var o in _ribbon.ItemsSource)
                    {
                        Logger.Info($"RibbonItem: {o}");
                        if (o is RibbonModelTab tab)
                        {
                            var i = 0;
                            foreach (var ribbonModelItem in tab.Items) Logger.Info($"[{i}] {ribbonModelItem}");
                        }
                    }


                    if (_AppMenu.ItemsSource != null)
                    {
                        foreach (var appMenuItem in _AppMenu.ItemsSource)
                            Logger.Info(appMenuItem.ToString());
                    }
                    else
                    {
                        foreach (var ribbonModelAppMenuElement in _viewModel.Ribbon.AppMenu.Items)
                            Logger.Info($"{ribbonModelAppMenuElement}");

                        Logger.Info($"No app menu Items source");
                    }

                    //_viewModel.Documents.Add(new DocModel { Title = "Log", Content = logControl });
                }
            }
        }

        private static void DumpRibbon(Ribbon ribbon)
        {
            var Ribbon = ribbon;
            foreach (var ribbonItem in Ribbon.Items)
            {
                if (ribbonItem is RibbonTab tab)
                {
                    foreach (var tabItem in tab.Items)
                    {
                        if (tabItem is RibbonGroup grp)
                        {
                            foreach (var grpItem in grp.Items)
                            {
                                if (grpItem is RibbonComboBox c)
                                {
                                    foreach (var cItem in c.Items)
                                    {
                                        if (cItem is RibbonGallery g)
                                        {
                                            DumpRibbonGallery(g);
                                        }
                                        else
                                        {
                                            throw new InvalidOperationException();
                                        }
                                    }
                                }
                                else
                                {
                                    throw new InvalidOperationException();
                                }
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }
                }
                else if (ribbonItem is RibbonModelTab tab2)
                {
                    foreach (var ribbonModelItem in tab2.Items)
                    {
                        if (ribbonModelItem is RibbonModelGroup g)
                        {
                            foreach (var modelItem in g.Items)
                            {
                                if (modelItem is RibbonModelItemComboBox box)
                                {
                                    foreach (var boxItem in box.Items)
                                    {
                                        if (boxItem is RibbonModelGallery gal)
                                        {
                                            foreach (var galItem in gal.Items)
                                            {
                                                if (galItem is RibbonModelGalleryCategory cat)
                                                {
                                                    foreach (var ribbonModelGalleryItem in cat.Items)
                                                    {
                                                        if (ribbonModelGalleryItem is RibbonModelGalleryItem item1)
                                                        {
                                                            DebugUtils.WriteLine(item1.ToString());
                                                        }
                                                        else
                                                        {
                                                            throw new InvalidOperationException();
                                                        }

                                                    }
                                                }
                                                else if (galItem is RibbonGalleryCategory cat0)
                                                {
                                                    DumpRibbonGalleryCategory(cat0);
                                                }
                                                else
                                                {
                                                    throw new InvalidOperationException();
                                                }

                                            }
                                        }
                                        else if (boxItem is RibbonGallery gal0)
                                        {
                                            DumpRibbonGallery(gal0);
                                        }
                                        else
                                        {
                                            throw new InvalidOperationException();
                                        }

                                    }
                                }
                                else
                                {
                                    //throw new InvalidOperationException();
                                }

                            }
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }

                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            DumpLogicalChildren(Ribbon);
            DumpVisualChildren(Ribbon);
        }

        private static void DumpRibbonGallery(RibbonGallery g)
        {
            foreach (var gItem in g.Items)
            {
                if (gItem is RibbonGalleryCategory cc)
                {
                    DumpRibbonGalleryCategory(cc);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        private static void DumpRibbonGalleryCategory(RibbonGalleryCategory cc)
        {
            foreach (var ccItem in cc.Items)
            {
                if (ccItem is RibbonGalleryItem item)
                {
                    DebugUtils.WriteLine(ccItem.ToString());
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        private static void DumpLogicalChildren(DependencyObject node, int depth = 0)
        {
            DebugUtils.WriteLine(String.Join("", Enumerable.Repeat("  ", depth)) + node.ToString());
            foreach (var child in LogicalTreeHelper.GetChildren(node))
        {
            DumpLogicalChildren((DependencyObject) child, depth + 1);
        }
        }
        private static void DumpVisualChildren(DependencyObject node, int depth = 0)
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(node);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(node, i);
                DebugUtils.WriteLine(String.Join("", Enumerable.Repeat("  ", depth)) + child.ToString());
                DumpVisualChildren(child, depth  + 1);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static RibbonModel RibbonModelBuilder(IComponentContext c, IEnumerable<Parameter> o)
        {
            var r = new RibbonModel {AppMenu = c.Resolve<RibbonModelApplicationMenu>()};
            foreach (var tg in c.Resolve<IEnumerable<RibbonModelContextualTabGroup>>())
            {
                DebugUtils.WriteLine("Adding contextual tab group " + tg);
                r.ContextualTabGroups.Add(tg);
            }

            r.AppMenu.Items.Add(new RibbonModelAppMenuItem {Header = "test"});
            var tabs = c.Resolve<IEnumerable<Meta<Lazy<RibbonModelTab>>>>();
            foreach (var meta in tabs)
            {
                var ribbonModelTab = meta.Value.Value;
                r.RibbonItems.Add(ribbonModelTab);
            }

            var tabProviders = c.Resolve<IEnumerable<IRibbonModelProvider<RibbonModelTab>>>();
            foreach (var ribbonModelProvider in tabProviders)
            {
                var item = ribbonModelProvider.ProvideModelItem(c);
                r.RibbonItems.Add(item);
            }

            return r;
        }
    }
}