using System ;
using System.Globalization ;
using System.Windows.Data ;
using System.Xaml ;
using System.Xml ;

namespace KayMcCormick.Lib.Wpf.Xaml
{
    /// <summary>
    /// </summary>
    public class XamlXmlDocumentConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            var d = new XmlDocument ( ) ;
            var t = XamlServices.Save ( value ) ;
            d.LoadXml ( t ) ;
            return d ;
        }


        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            return null ;
        }
        #endregion
    }
}