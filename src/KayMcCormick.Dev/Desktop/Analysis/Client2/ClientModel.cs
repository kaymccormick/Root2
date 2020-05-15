using System;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using AnalysisControls;
using AnalysisControls.RibbonModel;
using JetBrains.Annotations;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Lib.Wpf;

namespace Client2
{
    public sealed class ClientModel : INotifyPropertyChanged, IClientModel
    {
        private PrimaryRibbonModel _primaryRibbon;
        private object _hoverElement;
        private MyRibbon _ribbon;

        public ClientModel(PrimaryRibbonModel model, ReplaySubject<IControlView> replay)
        {
            replay.SubscribeOn(Scheduler.Default)
                .ObserveOnDispatcher(DispatcherPriority.Send)
                .Subscribe(
                    infos =>
                    {
                        var doc = new DocModel {Content = infos};
                        Main1Model.Documents.Add(doc);
                        Main1Model.ActiveContent = doc;
                    }
                );
            PrimaryRibbon = model;
        }

        public LogEventInstanceObservableCollection LogEntries { get; set; } =
            new LogEventInstanceObservableCollection();

        public PrimaryRibbonModel PrimaryRibbon
        {
            get { return _primaryRibbon; }
            set
            {
                if (Equals(value, _primaryRibbon)) return;
                _primaryRibbon = value;
                OnPropertyChanged();
            }
        }

        public Main1Model Main1Model { get; set; }

        public object HoverElement
        {
            get { return _hoverElement; }
            set
            {
                if (Equals(value, _hoverElement)) return;
                _hoverElement = value;
                OnPropertyChanged();
            }
        }

        public MyRibbon Ribbon
        {
            get { return _ribbon; }
            set { _ribbon = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}