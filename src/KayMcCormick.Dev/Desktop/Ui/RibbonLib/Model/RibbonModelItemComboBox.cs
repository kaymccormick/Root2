﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace RibbonLib.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelItemComboBox : RibbonModelItem, INotifyPropertyChanged
    {
        private object _selectionBoxItem;

        public RibbonModelItemComboBox()
        {
        }

        /// <summary>
        /// 
        /// </summary>

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ObservableCollection<object> Items { get; } = new ObservableCollection<object>();

        [DefaultValue(null)]
        public object SelectionBoxItem
        {
            get { return _selectionBoxItem; }
            set
            {
                if (Equals(value, _selectionBoxItem)) return;
                _selectionBoxItem = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="combo"></param>
        /// <returns></returns>
        public object CreateGallery()
        {
            var g = PrimaryRibbonModel.CreateGallery();
            Items.Add(g);
            return g;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override ControlKind Kind => ControlKind.RibbonComboBox;
    }
}