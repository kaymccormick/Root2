using System ;
using System.IO ;
using System.Linq ;
using System.Runtime.Serialization.Formatters.Binary ;
using System.Windows.Controls ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// Interaction logic for ExceptionUserControl.xaml
    /// </summary>
    public partial class ExceptionUserControl : UserControl
    {
        
        /// <summary>
        /// 
        /// </summary>
        public static BinaryFormatter BinaryFormatter = new BinaryFormatter ( ) ;


        /// <summary>
        /// 
        /// </summary>
        public ExceptionUserControl ( ) { InitializeComponent ( ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public ParsedExceptions Parsed { get ; } = new ParsedExceptions
                                                   {
                                                       ParsedList =
                                                           new[]
                                                           {
                                                               new ParsedStackInfo
                                                               {
                                                                   StackTraceEntries =
                                                                       Utils.ParseStackTrace (
                                                                                              Environment
                                                                                                 .StackTrace
                                                                                             )
                                                                            .ToList ( )
                                                               }
                                                           }.ToList ( )
                                                   } ;
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExceptionDataInfo
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