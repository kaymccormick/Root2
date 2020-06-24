using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Autofac;
using Autofac.Core;
using JetBrains.Annotations;
using KayMcCormick.Dev;

namespace KmDevLib
{
    /// <summary>
    /// Activation informatio 
    /// </summary>
    public class ActivationInfo : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        public int ManagedThreadId { get; set; } = Thread.CurrentThread.ManagedThreadId;

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, object> Metadata => Component.Metadata;

        /// <summary>
        /// 
        /// </summary>
        public Type InstanceType => Instance?.GetType();
        /// <summary>
        /// 
        /// </summary>
        public object Instance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public IComponentRegistration Component { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Parameter> Parameters { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public IComponentContext Context { get; set; }

        public object InstanceObjectId { get; set; }
        public RegInfo RegInfo { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}