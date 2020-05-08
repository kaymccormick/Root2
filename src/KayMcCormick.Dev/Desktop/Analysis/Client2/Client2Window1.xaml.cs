using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
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
using Autofac;
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
            ViewModel = new ClientModel();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public Client2Window1(ILifetimeScope scope, ClientModel viewModel, MyCacheTarget2 myCacheTarget)
        {
            _viewModel = viewModel;
            SetValue(AttachedProperties.LifetimeScopeProperty, scope);
            InitializeComponent();

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ClientModel
    {
        public LogEventInstanceObservableCollection LogEntries { get; set; } =
            new LogEventInstanceObservableCollection();

        public RibbonModel Ribbon { get; set; } = new RibbonModel();
    }
}