using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using JetBrains.Annotations;
using KayMcCormick.Dev;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelContextualTabGroup : DependencyObject, INotifyPropertyChanged
    {
        private Visibility _visibility = Visibility.Collapsed;
        public string Header { get; set; }

        public Visibility Visibility
        {
            get
            {
                DebugUtils.WriteLine("requested visiblity");
                return _visibility;
            }
            set
            {
                if (value == _visibility) return;
                DebugUtils.WriteLine($"Setting visibility to {value}");
                _visibility = value;
                OnPropertyChanged();
            }
        }
        
        [Browsable(false)]
        public PrimaryRibbonModel RibbonModel { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            DebugUtils.WriteLine($"{propertyName}");
        }
    }
}