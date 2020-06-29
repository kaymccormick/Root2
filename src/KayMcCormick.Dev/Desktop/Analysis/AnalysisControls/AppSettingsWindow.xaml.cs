using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using JetBrains.Annotations;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// Interaction logic for AppSettingsWindow.xaml
    /// </summary>
    public partial class AppSettingsWindow : Window, IView<AppSettingsViewModel>, INotifyPropertyChanged
    {
        private AppSettingsViewModel _viewModel;

        public AppSettingsWindow()
        {
            InitializeComponent();
        }

        /// <inheritdoc />
        public AppSettingsViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                if (Equals(value, _viewModel)) return;
                _viewModel = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AppSettingsViewModel : IViewModel, INotifyPropertyChanged
    {
        private bool _ribbon = true;

        /// <inheritdoc />
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

        /// <inheritdoc />
        public object InstanceObjectId { get; set; }

        public Visibility RibbonVisibility => Ribbon ? Visibility.Visible : Visibility.Collapsed;

        public bool Ribbon
        {
            get { return _ribbon; }
            set
            {
                if (value == _ribbon) return;
                _ribbon = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RibbonVisibility));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
