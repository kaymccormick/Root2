using System ;
using System.Collections.Generic ;
using System.IO ;
using System.Linq ;
using System.Text ;
using System.Threading.Tasks ;
using NLog ;
using NLog.Fluent ;

namespace KayMcCormick.Dev
{
    public class Utils
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public static void HandleInnerExceptions (
            Exception e
          , LogLevel  level  = null
          , Logger    logger = null
        )
        {
            TextWriter s = new StringWriter ( ) ;
            try
            {
                void doLog ( Exception exception )
                {
                    new LogBuilder ( logger ?? Logger )
                       .Level ( level ?? LogLevel.Debug )
                       .Exception ( exception )
                       .Message ( exception.Message )
                       .Write ( ) ;
                }

                var msg = $"{e.Message}" ;
                doLog ( e ) ;
                s.WriteLine ( e.Message ) ;
                var inner = e.InnerException ;
                var seen = new HashSet < object > ( ) ;
                while ( inner != null
                        && ! seen.Contains ( inner ) )
                {
                    doLog ( inner ) ;
                    seen.Add ( inner ) ;
                    inner = inner.InnerException ;
                }
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.WriteLine ( "Exception: " + ex ) ;
            }
        }
    }
}