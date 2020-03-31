using System ;
using System.Globalization ;
using System.Text.Json ;
using System.Windows.Data ;
using FindLogUsages ;
using Microsoft.CodeAnalysis ;

namespace AnalysisControls.Converters
{
    /// <summary>
    ///     <para>
    ///         Value converter for CSharp Syntax nodes - performs a JSON
    ///         serialization after transforming the nodes to a more concise form.
    ///     </para>
    ///     <para></para>
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
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
                if ( value == null )
                {
                    return null ;
                }

                var r = Transforms.TransformSyntaxNode ( ( SyntaxNode ) value ) ;
                return JsonSerializer.Serialize ( r ) ;
            }
            catch ( Exception ex )
            {
                return ex.Message ;
            }
        }


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