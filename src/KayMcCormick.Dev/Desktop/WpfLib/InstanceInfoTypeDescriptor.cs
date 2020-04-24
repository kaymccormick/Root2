using System.ComponentModel ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class InstanceInfoTypeDescriptor : CustomTypeDescriptor
    {
        // ReSharper disable once RedundantDefaultMemberInitializer
        private bool _enableConverter = true ;
        #region Overrides of CustomTypeDescriptor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override TypeConverter GetConverter ( )
        {
            return EnableConverter ? new WpfInstanceInfoConverter ( ) : base.GetConverter() ;
        }

        private bool EnableConverter { get { return _enableConverter ; } }
        #endregion
    }
}
