using System;
using System.CodeDom.Compiler ;
using System.Collections ;
using System.Collections.Generic;
using System.Diagnostics ;
using System.IO ;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KayMcCormick.Dev.Logging ;

namespace ConfigTest
{
    class Program
    {
        static void Main(string[] args)
        {
            AppLoggingConfigHelper.EnsureLoggingConfigured(message => Console.WriteLine ( message ));
            var doDumpConfig = AppLoggingConfigHelper.DoDumpConfig( s => { } ) ;
            IndentedTextWriter writer = new IndentedTextWriter(Console.Out);
            DoDump ( writer, doDumpConfig ) ;

        }

        private static void DoDump (
            IndentedTextWriter              dumpConfig
          , IDictionary doDumpConfig
          , int                             depth = 0
        )
        {
            foreach ( var key in doDumpConfig.Keys )
            {
                string @out = key.ToString ( ) ;
                var value = doDumpConfig[ key ] ;
                if ( ! ( value is IDictionary || value is string)
                     && ( value is IEnumerable e ) )
                {
                    Dictionary <int, object> d =new Dictionary < int , object > ();
                    int i = 0;
                    foreach(var yy in e)
                    {
                        d[ i ] = yy ;
                        i ++ ;
                    }

                    value = d ;
                }

                if(value is IDictionary dict)
                {
                    dumpConfig.WriteLine($"{key}:");
                    dumpConfig.Indent += 1;
                    DoDump(dumpConfig, dict, depth + 1);
                    dumpConfig.Indent -= 1 ;
                }
                else
                {
                    @out = @out + " = " + value?.ToString ( ) ;
                    dumpConfig.WriteLine ( @out ) ;
                }
            }
        }
    }
}
