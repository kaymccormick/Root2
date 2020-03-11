using System ;
using System.Globalization ;
using System.Windows.Data ;
using Microsoft.CodeAnalysis ;

namespace AnalysisControls
{
    public class LocationConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            if ( value is Location l )
            {
                return l.GetMappedLineSpan ( ).ToString ( ) ;
            }

            return null ;
        }

        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { return null ; }
        #endregion
    }
}