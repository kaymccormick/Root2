using KayMcCormick.Dev;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Dev.ServiceReference1;
using NLog;
using System;
using AppInstanceInfoRequest = KayMcCormick.Dev.ServiceReference1.AppInstanceInfoRequest;

namespace ConfigTest
{
    static class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        static void Main()
        {
            AppLoggingConfigHelper.EnsureLoggingConfigured(
                                                            Console.WriteLine
                                                           );
            Utils.PerformLogConfigDump(Console.Out);
            Logger.Info("{test}", new Test1() { Test2 = new Test2() { Hello = "derp" } });
            LogManager.GetCurrentClassLogger().Info("Test log message");
            using (AppInfoServiceClient client = new AppInfoServiceClient())
            {
                var appInstanceInfoResponse =
                    client.GetAppInstanceInfo(new AppInstanceInfoRequest());
                var info = appInstanceInfoResponse.Info;
                Console.WriteLine(info.StartupTime);

                foreach (var infoLoggerInfo in info.LoggerInfos)
                {
                    Console.WriteLine(infoLoggerInfo.TargetName);
                }

                foreach (var ci in info.ComponentInfos)
                {
                    Console.WriteLine(ci.Id);
                    foreach (var i in ci.Instances)
                    {
                        Console.WriteLine(@"	" + i.Desc);
                    }
                }

            }
        }
    }

    internal class Test1
    {
        public Test2 Test2 { get; set; }
    }

    internal class Test2
    {
        public string Hello { get; set; }
    }
}
