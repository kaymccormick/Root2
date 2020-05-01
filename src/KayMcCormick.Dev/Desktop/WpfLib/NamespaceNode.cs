using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace KayMcCormick.Lib.Wpf
{
    public class NamespaceNode : INotifyPropertyChanged
    {
        public Dictionary<string, NamespaceNode> Ns { get; } = new Dictionary<string, NamespaceNode>();

        private string _prefix;
        private string _ns;
        private Type _entity;
        private int _position;
        private ObservableCollection<NamespaceNode> _children = new ObservableCollection<NamespaceNode>();
        private string _elementName;

        public string Prefix
        {
            get { return _prefix; }
            set
            {
                if (value == _prefix) return;
                _prefix = value;
                OnPropertyChanged();
            }
        }

        protected bool Equals(NamespaceNode other)
        {
            return _prefix == other._prefix && ElementName == other.ElementName && Position == other.Position;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NamespaceNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_prefix != null ? _prefix.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ElementName != null ? ElementName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Position;
                return hashCode;
            }
        }

        public string ElementName
        {
            get { return _elementName; }
            set
            {
                if (value == _elementName) return;
                _elementName = value;
                OnPropertyChanged();
            }
        }

        public string Namespace
        {
            get { return _ns; }
            set
            {
                _ns = value;
                OnPropertyChanged();
                var lastIndexOf = _ns.LastIndexOf('.');
                var l = lastIndexOf == -1 ? 0 : lastIndexOf + 1;
                Prefix = _ns.Substring(0, l);
            }
        }

        public int Position
        {
            get { return _position; }
            set
            {
                if (value == _position) return;
                _position = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<NamespaceNode> Children
        {
            get { return _children; }
            set
            {
                if (Equals(value, _children)) return;
                _children = value;
                OnPropertyChanged();
            }
        }

        public Type Entity
        {
            get { return _entity; }
            set
            {
                if (Equals(value, _entity)) return;
                _entity = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<NamespaceNode> SubItems
        {
            get { return Children.Concat(Ns.Values); }
        }

        public override string ToString()
        {
            return
                $"{nameof(Prefix)}: {Prefix}, {nameof(ElementName)}: {ElementName}, {nameof(Position)}: {Position}, {nameof(Children)}: {Children.Count}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}