using System;
using System.Collections.Generic;
using Autofac;

namespace AnalysisControls.TypeDescriptors
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomTypes
    {
        private readonly List<Type> _kayTypes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kayTypes"></param>
        public CustomTypes(List<Type> kayTypes)
        {
            _kayTypes = kayTypes;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Type> CustomTypeList => _kayTypes;

        /// <summary>
        /// 
        /// </summary>
        public IComponentContext ComponentContext { get; set; }
    }
}