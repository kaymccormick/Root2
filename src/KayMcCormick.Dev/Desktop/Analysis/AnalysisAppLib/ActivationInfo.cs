using System;
using System.Collections.Generic;
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
        public int ManagedThreadId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object Instance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IComponentRegistration Component { get; set; }
        public IEnumerable<Parameter> Parameters { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IComponentContext Context { get; set; }
    }
}