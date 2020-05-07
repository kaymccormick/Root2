using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog ;

namespace LogTest
{
    class Program
    {
        private static readonly  Logger Logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args) {
            Action<string> xx = Logger.Info;
            xx("hi");
            Logger.Debug ( $"Hello {1}" ) ;
            try {
                string xxx = null;
                var q = xxx.ToString();
            } catch(Exception ex) {
                Logger.Info(ex, ex.Message);
            }
            var x = Logger;
            // doprocess
            x.Info("hello {test} {ab}", 123, 45);
        }

    }
}
