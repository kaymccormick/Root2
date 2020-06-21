using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Autofac.Core.Lifetime;
using JetBrains.Annotations;
using KayMcCormick.Dev.Interfaces;
using KmDevWpfControls;

namespace AnalysisControls
{
    public abstract class BaseNode : ITreeViewNode, INotifyPropertyChanged, KmDevWpfControls.IAsyncExpand
    {
        public abstract object NodeItem { get; }
        protected BaseNode()
        {
            _items.Add(new object());
            Items = _items;
        }

        protected bool _isExpanded;
        private bool _isSelected;
        private LifetimeScope _lifetimeScope;
        protected ObservableCollection<object> _items = new ObservableCollection<object>();
        private object _nodeItem;

        /// <inheritdoc />
        public abstract object Header { get; }

        /// <inheritdoc />
        public virtual bool IsExpanded
        {
            get { return _isExpanded; }
        }

        /// <inheritdoc />
        public virtual IEnumerable Items { get; }

        /// <inheritdoc />
        public virtual bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public LifetimeScope LifetimeScope
        {
            get { return _lifetimeScope; }
            set
            {
                if (Equals(value, _lifetimeScope)) return;
                _lifetimeScope = value;
                OnPropertyChanged();
            }
        }

        public IObjectIdProvider IdProvider { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <inheritdoc />
        public virtual void Collapse()
        {
            _isExpanded = false;
            OnPropertyChanged(nameof(IsExpanded));
        }

        /// <inheritdoc />
        public abstract Task ExpandAsync();
    }
}