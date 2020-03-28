using System ;
using System.Collections.Generic ;
using System.IO ;
using System.Linq ;
using System.Runtime.Serialization.Formatters.Binary ;
using System.Text ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Documents ;
using System.Windows.Input ;
using System.Windows.Media ;
using System.Windows.Media.Imaging ;
using System.Windows.Navigation ;
using System.Windows.Shapes ;
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
        public static readonly byte[] DataBytes =
            File.ReadAllBytes ( @"C:\data\logs\exception.bin" ) ;

        /// <summary>
        /// 
        /// </summary>
        public static BinaryFormatter BinaryFormatter = new BinaryFormatter ( ) ;

        /// <summary>
        /// 
        /// </summary>
        public static Exception DesignException { get ; } =
            BinaryFormatter.Deserialize ( new MemoryStream ( DataBytes ) ) as Exception ;

        /// <summary>
        /// 
        /// </summary>
        public ExceptionDataInfo DataInfo { get ; } = new ExceptionDataInfo ( )
                                                      {
                                                          Exception =DesignException
                                                        , ParsedExceptions =
                                                              Utils.GenerateParsedException (
                                                                                             DesignException
                                                                                            )
                                                      } ;
        /// <summary>
        /// 
        /// </summary>
        /// 
        public ParsedExceptions Parsed { get ; } = new ParsedExceptions
                                                   {
                                                       ParsedList =
                                                           ( new[]
                                                             {
                                                                 new ParsedStackInfo ( )
                                                                 {
                                                                     StackTraceEntries =
                                                                         Utils.ParseStackTrace (
                                                                                                Environment
                                                                                                   .StackTrace
                                                                                               )
                                                                              .ToList ( )
                                                                 }
                                                             } ).ToList ( )
                                                   } ;


                                                       /// <summary>
        /// 
        /// </summary>
        public ExceptionUserControl ( ) { InitializeComponent ( ) ; }
    }

    public class ExceptionDataInfo
    {
        public Exception Exception { get ; set ; }
        public ParsedExceptions ParsedExceptions { get ; set ; }
    }
}