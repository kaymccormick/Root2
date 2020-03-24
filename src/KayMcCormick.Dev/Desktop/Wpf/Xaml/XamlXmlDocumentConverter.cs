using System ;
using System.Globalization ;
using System.Windows.Data ;
using System.Xaml ;
using System.Xml ;

namespace KayMcCormick.Lib.Wpf.Xaml
{
    public class XamlXmlDocumentConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        public object Convert(
            object value
          , Type targetType
          , object parameter
          , CultureInfo culture
        )
        {
            XmlDocument d = new XmlDocument();
            try
            {
                var t = XamlServices.Save ( value ) ;
                d.LoadXml ( t ) ;
                return d ;
            } catch(Exception )
            {
                throw ;
            }
        }


        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { return null ; }
        #endregion
    }
}