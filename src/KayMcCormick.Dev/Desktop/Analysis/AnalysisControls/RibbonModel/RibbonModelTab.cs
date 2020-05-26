using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Markup;
using AnalysisControls.ViewModel;
using JetBrains.Annotations;
using KayMcCormick.Dev;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRibbonModelTab
    {
        /// <summary>
        /// 
        /// </summary>
        Visibility Visibility { get; set; }

        /// <summary>
        /// 
        /// </summary>
        object ContextualTabGroupHeader { get; set; }

        /// <summary>
        /// 
        /// </summary>
        object Header { get; set; }

        
        /// <summary>
        /// 
        /// </summary>
// ReSharper disable once UnusedAutoPropertyAccessor.Global
        PrimaryRibbonModel RibbonModel { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
[ContentProperty("Items")]
    public class RibbonModelTab : INotifyPropertyChanged, IRibbonModelTab, IHaveItems
    {
        /// <summary>
        /// 
        /// </summary>
        public RibbonModelTab()
        {
            Items = ItemsCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event OnSelectedChangedHandler OnSelectedChanged;
        public event ContextualTabGroupActivatedHandler ContextualTabGroupActivated;
        private Visibility _visibility = Visibility.Visible;
        private object _contextualTabGroupHeader;
        private object _header;
        private readonly ObservableCollection<IRibbonModelGroup> _items = new ObservableCollection<IRibbonModelGroup>();
        private IEnumerable _items2;
        private bool _isSelected;

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(Visibility.Visible)]
        public virtual Visibility Visibility
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
        public virtual object ContextualTabGroupHeader
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
        public virtual object Header
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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [JsonIgnore][Browsable(false)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public PrimaryRibbonModel RibbonModel { get; set; }

        public virtual ICollection<IRibbonModelGroup> ItemsCollection => _items;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public RibbonModelGroup CreateGroup(string @group)
        {
            var r = new RibbonModelGroup() {Header = @group};
            ItemsCollection.Add(r);
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
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <inheritdoc />
        public virtual IEnumerable Items
        {
            get { return _items2; }
            set
            {
                if (Equals(value, _items2)) return;
                _items2 = value;
                OnPropertyChanged();
            }
        }

        public virtual void OnContextualTabGroupActivated(object sender, ContextualTabGroupActivatedHandlerArgs e)
        {
            ContextualTabGroupActivated?.Invoke(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        public event ActiveContentChangedHandler ActiveContentChanged;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnActiveContentChanged(object sender, ActiveContentChangedEventArgs e)
        {
            ActiveContentChanged?.Invoke(sender, e);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void OnSelectedChangedHandler(object sender, OnSelectedChangedEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class OnSelectedChangedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isSelected"></param>
        public OnSelectedChangedEventArgs(bool isSelected)
        {
            IsSelected = isSelected;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSelected { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void ActiveContentChangedHandler(object sender,ActiveContentChangedEventArgs args);

    /// <summary>
    /// 
    /// </summary>
    public class ActiveContentChangedEventArgs : EventArgs
    {
        /// <inheritdoc />
        public ActiveContentChangedEventArgs(object activeContent)
        {
            ActiveContent = activeContent;
        }

        /// <summary>
        /// 
        /// </summary>
        public object ActiveContent { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void ContextualTabGroupActivatedHandler(object sender, ContextualTabGroupActivatedHandlerArgs args);

    /// <summary>
    /// 
    /// </summary>
    public class ContextualTabGroupActivatedHandlerArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public object Document{ get; set; }

        /// <inheritdoc />
        public ContextualTabGroupActivatedHandlerArgs(object document)
        {
            Document = document;
        }
    }
}