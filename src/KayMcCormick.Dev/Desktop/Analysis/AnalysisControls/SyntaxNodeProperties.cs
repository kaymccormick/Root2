using System;
using System.ComponentModel;
using Microsoft.CodeAnalysis.CSharp;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    [ApplyTypes(typeof(CSharpSyntaxNode))]
    public class SyntaxNodeProperties : IPropertiesAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        public SyntaxNodeProperties()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public PropertyDescriptorCollection GetProperties(Type t)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool HandleType(Type type)
        {
            if (typeof(CSharpSyntaxNode).IsAssignableFrom(type)) return true;

            return false;
        }

        public void UpdateProperty(IUpdatableProperty propertyDescriptor)
        {
            if (propertyDescriptor.Name == "Parent") propertyDescriptor.SetIsBrowsable(false);
        }
    }
}