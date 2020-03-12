using System ;
using System.Globalization ;
using System.Windows.Data ;
using Microsoft.CodeAnalysis ;
using NLog ;

namespace AnalysisControls.Converters
{
    public class TokenConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            if ( value is SyntaxToken x )
            {
                return x.ToFullString() ;
            } else if ( value is SyntaxTrivia t)
            {
                return t.ToString ( ) ;
            }
            LogManager.GetCurrentClassLogger().Debug("{t} {x}", value.GetType().FullName, value);
    
            return null ;
        }

        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { return null ; }
        #endregion
    }
}