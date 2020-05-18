using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Markup;
using JetBrains.Annotations;
using KayMcCormick.Dev;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
[ContentProperty("Items")]
    public class RibbonModelTab : INotifyPropertyChanged, ISupportInitialize
    {
        private Visibility _visibility = Visibility.Visible;
        private object _contextualTabGroupHeader;
        private object _header;

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(Visibility.Visible)]
        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                if (value == _visibility) return;
                DebugUtils.WriteLine($"Setting visibility to {value}");
                _visibility = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public object ContextualTabGroupHeader
        {
            get { return _contextualTabGroupHeader; }
            set
            {
                if (Equals(value, _contextualTabGroupHeader)) return;
                _contextualTabGroupHeader = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 

        /// </summary>
        [DefaultValue(null)]
        public object Header
        {
            get { return _header; }
            set
            {
                if (Equals(value, _header)) return;
                _header = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ObservableCollection<RibbonModelGroup> Items { get; } = new ObservableCollection<RibbonModelGroup>();

        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [JsonIgnore][Browsable(false)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public PrimaryRibbonModel RibbonModel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public RibbonModelGroup CreateGroup(string @group)
        {
            var r = new RibbonModelGroup() {Header = @group};
            Items.Add(r);
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"RibbonModelTab[{Header}]";
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        // ReSharper disable once VirtualMemberNeverOverridden.Global
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <inheritdoc />
        public virtual void BeginInit()
        {
            
        }

        /// <inheritdoc />
        public virtual void EndInit()
        {
            
        }
    }

}