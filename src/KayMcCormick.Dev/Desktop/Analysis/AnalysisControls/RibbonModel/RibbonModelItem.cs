using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AnalysisControls.Converters;
using JetBrains.Annotations;
using KayMcCormick.Lib.Wpf.Command;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    [TypeConverter(typeof(RibbonModelItemTypeConverter))]
    public abstract class RibbonModelItem : IRibbonModelGroupItem, INotifyPropertyChanged
    {
        private IAppCommand _appCommand;
        private ICommand _command;

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once MemberCanBeProtected.Global
        public abstract ControlKind Kind { get; }
        /// <summary>
        /// 
        /// </summary>
        [TypeConverter(typeof(ObjectStringTypeConverter))]
        public object Label { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [TypeConverter(typeof(AppCommandTypeConverter))]
        // ReSharper disable once UnusedMember.Global
        public IAppCommand AppCommand
        {
            get { return _appCommand; }
            set
            {
                if (Equals(value, _appCommand)) return;
                _appCommand = value;
                Command = _appCommand.Command;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// s
        /// </summary>
        public ICommand Command
        {
            get { return _command; }
            set
            {
                if (Equals(value, _command)) return;
                _command = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object CommandTarget { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object CommandParameter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object LargeImageSource
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public object SmallImageSource { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "RibbonModelItem (Kind=" + Kind + ")";
        }

        /// <summary>
        /// 
        /// </summary>
        public double? MaxWidth { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double? MaxHeight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double? MinWidth { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double? MinHeight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double? Width { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double? Height { get; set; }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public string StringLabel => Label?.ToString();

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
    }
}