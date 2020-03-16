using KayMcCormick.Dev ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Dev.ServiceReference1 ;
using NLog ;
using System ;
using System.ServiceModel ;
using ConfigTest.ServiceReference1 ;
using NLog.LogReceiverService ;
using AppInstanceInfoRequest = KayMcCormick.Dev.ServiceReference1.AppInstanceInfoRequest ;

namespace ConfigTest
{
    internal static class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private static void Main ( )
        {
            // ServiceReference1.LogReceiverServerClient client1 = new LogReceiverServerClient(new WSDualHttpBinding(), new EndpointAddress("http://exomail-87976:8737/discovery/scenarios/logreceiver/"));
            // var nLogEvent = new NLogEvent ( )
            //                 {
            //                     Id = 5 , LevelOrdinal = 1 , LoggerOrdinal = 0 , MessageOrdinal = 1
            //                 } ;
            // nLogEvent.Values = "0|1" ;
            //
            // client1.ProcessLogMessages ( new NLogEvents ( ) { Events = new[] { nLogEvent } } ) ;
            //
            // return ;

            var x = new ApplicationInstance (
                                             message => {
                                                 Console.Error.WriteLine ( message ) ;
                                             }
                                            ) ;
            x.Initialize ( ) ;
            x.Startup ( ) ;
            Utils.PerformLogConfigDump ( Console.Out ) ;
            Logger.Info ( "{test}" , new Test1 ( ) { Test2 = new Test2 ( ) { Hello = "derp" } } ) ;
            LogManager.GetCurrentClassLogger ( ).Info ( "Test log message" ) ;
            // using ( var client = new AppInfoServiceClient ( ) )
            // {
            //     var appInstanceInfoResponse =
            //         client.GetAppInstanceInfo ( new AppInstanceInfoRequest ( ) ) ;
            //     var info = appInstanceInfoResponse.Info ;
            //     Console.WriteLine ( info.StartupTime ) ;
            //
            //     foreach ( var infoLoggerInfo in info.LoggerInfos )
            //     {
            //         Console.WriteLine ( infoLoggerInfo.TargetName ) ;
            //     }
            //
            //     foreach ( var ci in info.ComponentInfos )
            //     {
            //         Console.WriteLine ( ci.Id ) ;
            //         foreach ( var i in ci.Instances )
            //         {
            //             Console.WriteLine ( @"	" + i.Desc ) ;
            //         }
            //     }
            // }
        }
    }

    internal class Test1
    {
        public Test2 Test2 { get ; set ; }
    }

    internal class Test2
    {
        public string Hello { get ; set ; }
    }
}