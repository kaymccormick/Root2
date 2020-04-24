#region header
// Kay McCormick (mccor)
// 
// Analysis
// WpfLib
// ComponentInfoTypeDescriptor.cs
// 
// 2020-04-23-4:45 AM
// 
// ---
#endregion
using System ;
using System.ComponentModel ;
using System.Globalization ;
using System.Windows ;
using System.Windows.Controls ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ComponentInfoTypeDescriptor : CustomTypeDescriptor
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
            return EnableConverter ? new WpfComponentInfoConverter( ) : base.GetConverter() ;
        }

        private bool EnableConverter { get { return _enableConverter ; } }
        #endregion
    }

    public class WpfComponentInfoConverter : UiElementTypeConverter
    {

        // #region Overrides of TypeConverter
        // public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
        // {
        //     if (destinationType == typeof(UIElement))
        //     {
        //         return true;
        //     }
        //     return base.CanConvertTo ( context , destinationType ) ;
        // }
        //
        // public override object ConvertTo (
        //     ITypeDescriptorContext context
        //   , CultureInfo            culture
        //   , object                 value
        //   , Type                   destinationType
        // )
        // {
        //     if ( destinationType == typeof ( UIElement ) )
        //     {
        //         ContentControl cc = new ContentControl();
        //         cc.Content = value ;
        //         return cc;
        //     }
        //     return base.ConvertTo ( context , culture , value , destinationType ) ;
        // }
        // #endregion
    }
}