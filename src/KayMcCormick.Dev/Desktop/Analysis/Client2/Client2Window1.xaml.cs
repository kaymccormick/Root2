using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using AnalysisAppLib;
using AnalysisControls;
using Autofac;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Lib.Wpf;
using NLog;

namespace Client2
{
    /// <summary>
    /// Interaction logic for Client2Window1.xaml
    /// </summary>
    [ShortKeyMetadata("Client2Window1")]
    public partial class Client2Window1 : RibbonWindow, IView<ClientViewModel>
    {
        public Client2Window1(ILifetimeScope scope, ClientViewModel viewModel, MyCacheTarget2 myCacheTarget)
        {
            ViewModel = viewModel;
            SetValue(AttachedProperties.LifetimeScopeProperty, scope);
            InitializeComponent();

            myCacheTarget?.Cache.SubscribeOn(Scheduler.Default)
                .Buffer(TimeSpan.FromMilliseconds(100))
                .Where(x => x.Any())
                .ObserveOnDispatcher(DispatcherPriority.Background)
                .Subscribe(
                    infos => {
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
            LogEventInstancesControl logControl = new LogEventInstancesControl();
            logControl.SetBinding(LogEventInstancesControl.EventsSourceProperty,
                new Binding("ViewModel.LogEntries") {Source = this});

            Main1.ViewModel.Documents.Add(new DocModel { Title = "Log", Content = logControl} );
        }

        private void Action()
        {
            for (;;)
            {
                LogManager.GetCurrentClassLogger().Info("My event");
                Thread.Sleep(30000);
            }
        }

        public ClientViewModel ViewModel { get; }
    }

    public class ClientViewModel
    {
        public LogEventInstanceObservableCollection LogEntries { get; set; } = new LogEventInstanceObservableCollection();
    }
}
