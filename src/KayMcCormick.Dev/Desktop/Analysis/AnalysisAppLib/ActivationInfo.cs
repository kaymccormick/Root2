﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Autofac;
using Autofac.Core;

namespace AnalysisAppLib
{
    /// <summary>
    /// Activation informatio 
    /// </summary>
    public class ActivationInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        public int ManagedThreadId { get; set; } = Thread.CurrentThread.ManagedThreadId;

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
        public IEnumerable<Parameter> Parameters { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public IComponentContext Context { get; set; }
    }
}