using System ;
using System.Globalization ;
using System.Windows.Data ;
using JetBrains.Annotations ;
using KayMcCormick.Lib.Wpf.Command ;

namespace KayMcCormick.Lib.Wpf.Xaml
{
    /// <summary>
    /// </summary>
    public sealed class AppCommandConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        [ NotNull ]
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            var command = ( IAppCommand ) value ;
            var exceptionHandleException = ( IHandleException ) parameter ;
            var x = new WrappedAppCommand ( command , exceptionHandleException ) ;
            return x.Command ;
        }

        /// <summary>
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