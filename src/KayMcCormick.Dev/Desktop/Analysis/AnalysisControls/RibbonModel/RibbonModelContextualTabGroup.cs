﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        private Visibility _visibility;

        /// <summary>
        /// 
        /// </summary>
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
        public string Header { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public PrimaryRibbonModel RibbonModel { get; set; }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

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