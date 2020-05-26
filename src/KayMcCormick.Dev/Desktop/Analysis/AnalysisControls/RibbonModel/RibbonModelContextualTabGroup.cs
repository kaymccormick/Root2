using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Windows;
using JetBrains.Annotations;
using KayMcCormick.Dev;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelContextualTabGroup : INotifyPropertyChanged
    {
        private Visibility _visibility = Visibility.Collapsed;
        private string _header;

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(Visibility.Collapsed)]
        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                DebugUtils.WriteLine("Visibility setter (value = " + value +   ")");
                if (value == _visibility) return;
                _visibility = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public string Header
        {
            get { return _header; }
            set
            {
                if (value == _header) return;
                _header = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PrimaryRibbonModel RibbonModel { get; set; }

        /// <inheritdoc />
        private event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                DebugUtils.WriteLine($"{nameof(PropertyChanged)}: add: {value}");
                this.PropertyChanged += value;
            }
            remove { this.PropertyChanged -= value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            DebugUtils.WriteLine($"{propertyName}");
        }
    }
}