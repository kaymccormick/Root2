using System ;
using System.Globalization ;
using System.Windows ;
using System.Windows.Data ;
using System.Windows.Input ;

namespace KayMcCormick.Lib.Wpf
{
    public class AppCommandConverter : IValueConverter
    {
        #region Implementation of IValueConverter
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


    