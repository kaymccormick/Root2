using System.Collections.ObjectModel;

namespace RibbonLib.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelMenuItem: RibbonModelItem
    {
        private bool _isEnabled = true;
        private object _header;
        private bool _isHorizontallyResizable;
        private bool _isVerticallyResizable;
        private bool _isChecked;
        private bool _isCheckable;
        private object _toolTipDescription;
        private string _keyTip;
        private object _toolTipFooterImage;
        private object _toolTipFooterDescription;
        private object _toolTipImage;
        private object _toolTipFooterTitle;
        private object _toolTipTitle;
        private object _image;

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<object> Items { get; } = new ObservableCollection<object>();

        /// <summary>
        /// 
        /// </summary>
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
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value == _isEnabled) return;
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value == _isChecked) return;
                _isChecked = value;
                OnPropertyChanged();
            }
        }

        public string KeyTip
        {
            get { return _keyTip; }
            set
            {
                if (value == _keyTip) return;
                _keyTip = value;
                OnPropertyChanged();
            }
        }

        public bool IsCheckable
        {
            get { return _isCheckable; }
            set
            {
                if (value == _isCheckable) return;
                _isCheckable = value;
                OnPropertyChanged();
            }
        }

        public object ToolTipFooterImage
        {
            get { return _toolTipFooterImage; }
            set
            {
                if (Equals(value, _toolTipFooterImage)) return;
                _toolTipFooterImage = value;
                OnPropertyChanged();
            }
        }

        public object ToolTipFooterDescription
        {
            get { return _toolTipFooterDescription; }
            set
            {
                if (Equals(value, _toolTipFooterDescription)) return;
                _toolTipFooterDescription = value;
                OnPropertyChanged();
            }
        }

        public object ToolTipFooterTitle
        {
            get { return _toolTipFooterTitle; }
            set
            {
                if (Equals(value, _toolTipFooterTitle)) return;
                _toolTipFooterTitle = value;
                OnPropertyChanged();
            }
        }

        public object ToolTipImage
        {
            get { return _toolTipImage; }
            set
            {
                if (Equals(value, _toolTipImage)) return;
                _toolTipImage = value;
                OnPropertyChanged();
            }
        }

        public object ToolTipTitle
        {
            get { return _toolTipTitle; }
            set
            {
                if (Equals(value, _toolTipTitle)) return;
                _toolTipTitle = value;
                OnPropertyChanged();
            }
        }

        public object Image
        {
            get { return _image; }
            set
            {
                if (Equals(value, _image)) return;
                _image = value;
                OnPropertyChanged();
            }
        }


        public object ToolTipDescription
        {
            get { return _toolTipDescription; }
            set
            {
                if (Equals(value, _toolTipDescription)) return;
                _toolTipDescription = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsHorizontallyResizable
        {
            get { return _isHorizontallyResizable; }
            set
            {
                if (value == _isHorizontallyResizable) return;
                _isHorizontallyResizable = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsVerticallyResizable
        {
            get { return _isVerticallyResizable; }
            set
            {
                if (value == _isVerticallyResizable) return;
                _isVerticallyResizable = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public RibbonModelMenuItem CreateMenuItem(string label)
        {
            var r = new RibbonModelMenuItem() { Header = label };
            Items.Add(r);
            return r;
        }

        /// <inheritdoc />
        public override ControlKind Kind => ControlKind.RibbonMenuItem;
    }
}