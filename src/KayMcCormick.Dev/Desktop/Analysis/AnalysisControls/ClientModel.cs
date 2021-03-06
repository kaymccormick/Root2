﻿using System;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Windows.Controls.Ribbon;
using System.Windows.Threading;
using AnalysisControls.ViewModel;
using JetBrains.Annotations;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Lib.Wpf;
using RibbonLib.Model;

namespace AnalysisControls
{
    public sealed class ClientModel : INotifyPropertyChanged, IClientModel
    {
        private PrimaryRibbonModel _primaryRibbon;
        private object _hoverElement;
        private MyRibbon _myRibbon;

        public ClientModel(PrimaryRibbonModel model)
        {
            PrimaryRibbon = model;
        }

        public LogEventInstanceObservableCollection LogEntries { get; set; } =
            new LogEventInstanceObservableCollection();

        public PrimaryRibbonModel PrimaryRibbon
        {
            get { return _primaryRibbon; }
            set
            {
                if (Object.Equals(value, _primaryRibbon)) return;
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
                if (Object.Equals(value, _hoverElement)) return;
                _hoverElement = value;
                OnPropertyChanged();
            }
        }

        public MyRibbon MyRibbon
        {
            get { return _myRibbon; }
            set { _myRibbon = value; }
        }

        public Ribbon Ribbon { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}