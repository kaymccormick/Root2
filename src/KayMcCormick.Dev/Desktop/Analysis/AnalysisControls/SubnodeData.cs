using System;
using System.Reflection;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class SubnodeData

    {
        /// <summary>
        /// 
        /// </summary>
        public object Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public object ResourceName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Assembly Assembly { get; set; }

        public object Value { get; set; }
    }
}