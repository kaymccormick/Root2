using System ;
using System.Globalization ;
using System.Text.Json ;
using System.Windows.Data ;
using AnalysisFramework ;
using Microsoft.CodeAnalysis ;

namespace AnalysisControls
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
                var r = Transforms.TransformSyntaxNode(value as SyntaxNode);
                return JsonSerializer.Serialize ( r ) ;
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