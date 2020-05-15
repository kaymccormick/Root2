using System ;
using System.Windows.Controls ;
using KayMcCormick.Dev.Application ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// Interaction logic for ExceptionUserControl.xaml
    /// </summary>
    public sealed partial class ExceptionUserControl : UserControl
    {
        /// <summary>
        /// 
        /// 
        /// </summary>
        public ExceptionUserControl ( ) { InitializeComponent ( ) ; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class ExceptionDataInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public Exception Exception { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public ParsedExceptions ParsedExceptions { get ; set ; }
    }
}