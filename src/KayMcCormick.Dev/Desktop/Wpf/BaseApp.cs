using System.Windows ;
using static KayMcCormick.Dev.Logging.AppLoggingConfigHelper ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public class BaseApp : Application
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Application" /> class.</summary>
        /// <exception cref="System.InvalidOperationException">More than one instance of the <see cref="System.Windows.Application" /> class is created per <see cref="System.AppDomain" />.</exception>
        public BaseApp ( )
        {
            EnsureLoggingConfigured ( ) ;
        }
    }
}
