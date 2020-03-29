using System ;
using System.Globalization ;
using System.Windows.Data ;
using System.Windows.Input ;

namespace KayMcCormick.Lib.Wpf.Xaml
{
    /// <summary>
    /// 
    /// </summary>
    public class AppCommandConverter : IValueConverter
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
            IAppCommand command = ( IAppCommand ) value ;
            IHandleException exceptionHandleException = ( IHandleException ) parameter ;
            WrappedAppCommand x = new WrappedAppCommand ( command , exceptionHandleException ) ;
            return ( ICommand ) x.Command ;
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


    