using System;
using System.CodeDom.Compiler ;
using System.Collections ;
using System.Collections.Generic;
using System.Diagnostics ;
using System.IO ;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Dev.ServiceReference1 ;
using AppInstanceInfoRequest = KayMcCormick.Dev.ServiceReference1.AppInstanceInfoRequest ;

namespace ConfigTest
{
    class Program
    {
        static void Main ( string[] args )
        {
            AppLoggingConfigHelper.EnsureLoggingConfigured (
                                                            message => Console.WriteLine ( message )
                                                           ) ;
            Utils.PerformLogConfigDump ( Console.Out ) ;
            NLog.LogManager.GetCurrentClassLogger ( ).Info ( "Test log message" ) ;
            using ( AppInfoServiceClient client = new AppInfoServiceClient ( ) )
            {
                var appInstanceInfoResponse =
                    client.GetAppInstanceInfo ( new AppInstanceInfoRequest ( ) ) ;
                var info = appInstanceInfoResponse.Info ;
                Console.WriteLine ( info.StartupTime ) ;

                foreach ( var infoLoggerInfo in info.LoggerInfos )
                {
                    Console.WriteLine ( infoLoggerInfo.TargetName ) ;
                }

                foreach ( var ci in info.ComponentInfos )
                {
                    Console.WriteLine ( ci.Id ) ;
                    foreach(var i in ci.Instances)
                    {
                        Console.WriteLine ( "\t" + i.Desc ) ;
                    }
                }
                
            }
        }
    }
}
