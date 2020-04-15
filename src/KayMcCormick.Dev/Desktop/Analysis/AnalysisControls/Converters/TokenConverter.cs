using System ;
using System.Globalization ;
using System.Windows.Data ;
using Microsoft.CodeAnalysis ;
using NLog ;

namespace AnalysisControls.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenConverter : IValueConverter
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
            switch ( value )
            {
                case SyntaxToken x :  return x.ToFullString ( ) ;
                case SyntaxTrivia t : return t.ToString ( ) ;
            }

            if ( value != null )
            {
                LogManager.GetCurrentClassLogger ( )
                          .Debug ( "{t} {x}" , value.GetType ( ).FullName , value ) ;
            }

            return null ;
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