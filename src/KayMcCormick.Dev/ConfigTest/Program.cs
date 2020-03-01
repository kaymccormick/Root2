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

namespace ConfigTest
{
    class Program
    {
        static void Main(string[] args)
        {
            AppLoggingConfigHelper.EnsureLoggingConfigured(message => Console.WriteLine ( message ));
            Utils.PerformLogConfigDump ( Console.Out ) ;
            NLog.LogManager.GetCurrentClassLogger ( ).Info ( "Test log message" ) ;

        }
    }
}
