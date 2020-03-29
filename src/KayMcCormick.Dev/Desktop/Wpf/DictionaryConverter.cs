using System ;
using System.Collections ;
using System.Globalization ;
using System.Windows.Data ;

namespace KayMcCormick.Lib.Wpf
{
    public class DictionaryConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            if ( value is IDictionary d )
            {
                if ( parameter != null && d.Contains ( parameter ) )
                {
                    return d[ parameter ] ;
                }
                else
                {
                    return null ;
                }
            }
            return null ;
        }

        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { return null ; }
        #endregion
    }
}