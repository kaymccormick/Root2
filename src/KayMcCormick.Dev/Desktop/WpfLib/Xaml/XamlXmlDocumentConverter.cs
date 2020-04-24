using System ;
using System.Globalization ;
using System.Windows.Data ;
using System.Xaml ;
using System.Xml ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf.Xaml
{
    /// <summary>
    /// A converter which generates XAML for the given value and produces an XML Document for use.
    /// </summary>
    // ReSharper disable once UnusedType.Global
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
        [ NotNull ]
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