using System ;
using System.Diagnostics ;
using System.Globalization ;
using System.Reflection ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Markup ;

namespace KayMcCormick.Lib.Wpf
{
    public class ControlTemplateConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            if ( value == null )
            {
                return null ;
            }

            ControlTemplate x = ( ControlTemplate ) value ;
            var t = x.Template ;
            foreach (var fieldInfo in t.GetType()
                                       .GetFields(
                                                  BindingFlags.Instance | BindingFlags.NonPublic
                                                 ))
            {
                Debug.WriteLine(fieldInfo.Name);
            }

            foreach (var fieldInfo in t.GetType()
                                       .GetProperties(
                                                  BindingFlags.Instance | BindingFlags.NonPublic
                                                 ))
            {
                Debug.WriteLine(fieldInfo.Name);
            }

            try
            {
                var xaml = XamlWriter.Save ( value ) ;
                Debug.WriteLine ( xaml ) ;
                var result = XamlReader.Parse ( xaml ) ;
                Debug.WriteLine ( $"{result.GetType().FullName}");
                return result ;
            }
            catch ( Exception ex )
            {
                return ex.ToString ( ) ;
            }
        }

        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { return null ; }
        #endregion
    }
}