using System ;
using System.Globalization ;
using System.Windows.Data ;

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
    public sealed class TransformConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
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

#if POCO
                var r = GenTransforms.Transform_CSharp_Node(( CSharpSyntaxNode ) value ) ;
                return JsonSerializer.Serialize ( r ) ;
#else
                throw new NotImplementedException();
#endif

            }
            catch ( Exception ex )
            {
                return ex.Message ;
            }
        }


        /// <summary>
        /// 
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