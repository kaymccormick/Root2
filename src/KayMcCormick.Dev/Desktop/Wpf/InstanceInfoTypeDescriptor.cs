using System.ComponentModel ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class InstanceInfoTypeDescriptor : CustomTypeDescriptor
    {
        #region Overrides of CustomTypeDescriptor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ NotNull ]
        public override TypeConverter GetConverter ( )
        {
            return new WpfInstanceInfoConverter ( ) ;
        }
        #endregion
    }
}
