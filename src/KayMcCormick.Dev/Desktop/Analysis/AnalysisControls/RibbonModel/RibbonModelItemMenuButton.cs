using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public sealed class RibbonModelItemMenuButton : RibbonModelItem
    {
        private RibbonModelMenuCollection  _items = new RibbonModelMenuCollection();
        private IEnumerable _items1;
        private bool _isDropDownOpen;
        private bool _isChecked;

        /// <inheritdoc />
        public RibbonModelItemMenuButton()
        {
            _items1 = _items;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable Items
        {
            get { return _items1; }
            set
            {
                if (Equals(value, _items1)) return;
                _items1 = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public RibbonModelMenuItem CreateMenuItem(string label)
        {
            var r = new RibbonModelMenuItem() { Header = label};
            _items.Add(r);
            return r;
        }

        public override ControlKind Kind => ControlKind.RibbonMenuButton;
        /// <summary>
        /// 
        /// </summary>
        public ICollection<object> ItemsCollection => _items;

        /// <summary>
        /// 
        /// </summary>
        public bool IsDropDownOpen
        {
            get { return _isDropDownOpen; }
            set
            {
                if (value == _isDropDownOpen) return;
                RibbonDebugUtils.WriteLine($"ISDropDownOpen = {value}");
                _isDropDownOpen = value;
                OnPropertyChanged();
            }
        }

        public object ItemContainerTemplateKey { get; set; }
    }

    public class RibbonModelMenuCollection : ObservableCollection<object>, IRibbonMenuCollection
    {
    }
}