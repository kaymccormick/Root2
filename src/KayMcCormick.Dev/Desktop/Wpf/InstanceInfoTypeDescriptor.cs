using System.ComponentModel ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class InstanceInfoTypeDescriptor : CustomTypeDescriptor
    {
        private bool _enableConverter = false ;
        #region Overrides of CustomTypeDescriptor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override TypeConverter GetConverter ( )
        {
            return EnableConverter ? new WpfInstanceInfoConverter ( ) : base.GetConverter() ;
        }

        private bool EnableConverter { get { return _enableConverter ; } set { _enableConverter = value ; } }
        #endregion
    }
}
