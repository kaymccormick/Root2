using System;
using System.ComponentModel;
using System.Reflection.Emit;
using JetBrains.Annotations;

// ReSharper disable RedundantOverriddenMember

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPropertiesAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        PropertyDescriptorCollection GetProperties(Type t);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool HandleType(Type type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyDescriptor"></param>
        void UpdateProperty(IUpdatableProperty propertyDescriptor);
    }
}