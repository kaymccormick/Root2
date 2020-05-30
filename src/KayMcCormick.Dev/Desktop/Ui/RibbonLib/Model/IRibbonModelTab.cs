using System.Windows;

namespace RibbonLib.Model
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
}