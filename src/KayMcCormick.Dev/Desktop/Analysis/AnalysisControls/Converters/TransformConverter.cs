using System ;
using System.Globalization ;
using System.Text.Json ;
using System.Windows.Data ;
using AnalysisAppLib ;
using Microsoft.CodeAnalysis ;

namespace AnalysisControls.Converters
{
    public class TransformConverter : IValueConverter   
    {
        #region Implementation of IValueConverter
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
         
            try
            {
                if ( value != null ) {
                    var r = Transforms.TransformSyntaxNode(( SyntaxNode ) value);
                    return JsonSerializer.Serialize ( r ) ;
                }
return null;
            }
            catch ( Exception ex )
            {
                return ex.Message ;
            }
        }


        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { return null ; }
        #endregion
    }
}
