using System;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using AnalysisControls;
using AnalysisControls.RibbonM;
using JetBrains.Annotations;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Lib.Wpf;

namespace Client2
{
    public class ClientModel : INotifyPropertyChanged, IClientModel
    {
        private readonly ReplaySubject<IControlView> _replay;
        private RibbonModel _ribbon;

        public ClientModel(RibbonModel model, ReplaySubject<IControlView> replay)
        {
            _replay = replay;
            _replay.SubscribeOn(Scheduler.Default)
                .ObserveOnDispatcher(DispatcherPriority.Send)
                .Subscribe(
                    infos =>
                    {
                        var doc = new DocModel();
                        doc.Content = infos;
                        Main1Model.Documents.Add(doc);
                        Main1Model.ActiveContent = doc;
                    }
                );
            Ribbon = model;
        }

        public LogEventInstanceObservableCollection LogEntries { get; set; } =
            new LogEventInstanceObservableCollection();

        public RibbonModel Ribbon
        {
            get { return _ribbon; }
            set
            {
                if (Equals(value, _ribbon)) return;
                _ribbon = value;
                OnPropertyChanged();
            }
        }

        public Main1Model Main1Model { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}