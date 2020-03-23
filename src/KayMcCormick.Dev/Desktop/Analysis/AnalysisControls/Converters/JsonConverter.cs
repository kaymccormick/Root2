using System ;
using System.Globalization ;
using System.Text.Json ;
using System.Windows.Data ;

namespace AnalysisControls.Converters
{
    public class JsonConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            var x = JsonSerializer.Deserialize<dynamic> ( value as string ?? throw new InvalidOperationException ( ) ) ;
            return x ;
        }

        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { return null ; }
        #endregion
    }
}