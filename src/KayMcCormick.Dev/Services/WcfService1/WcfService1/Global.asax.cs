using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using KayMcCormick.Dev.Logging ;
using NLog ;
using NLog.Config;
using NLog.Targets;

namespace WcfService1
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // Environment.SetEnvironmentVariable("DISABLE_LOG_TARGETS", (Environment.GetEnvironmentVariable("DISABLE_LOG_TARGETS") ?? "") + ";log");
            Environment.SetEnvironmentVariable (
                                                "LOGGING_WEBSERVICE_ENDPOINT"
                                              , "http://localhost:27809/ReceiveLogs.svc"
                                               ) ;
            AppLoggingConfigHelper.EnsureLoggingConfigured();
            AppLoggingConfigHelper.DoDumpConfig ( msg => Debug.WriteLine ( msg ) ) ;
#if false
            try
            {
                var q = LogManager.Configuration ;
                if ( LogManager.IsLoggingEnabled ( ) )
                {
                    return ;
                }
                var conf = new LoggingConfiguration();
                var t0 = new EventLogTarget("elog")
                {
                    Source = "Application"
                };
                conf.AddTarget ( t0 ) ;
                conf.AddRuleForAllLevels(t0);

                var target = new DebuggerTarget("debug");
                conf.AddTarget(target);
                conf.AddRuleForAllLevels(target);
                var t2 = new NLogViewerTarget ( "viewer" )
                         {
                             IncludeAllProperties = true
                       , IncludeCallSite = true
                       , IncludeSourceInfo = true
                         } ;
                t2.Address = "udp://10.25.0.102:9999" ;
                conf.AddTarget ( t2 ) ;
                conf.AddRuleForAllLevels ( t2 ) ;
                NLog.LogManager.Configuration = conf;
            }
            catch (Exception x)
            {
                Debug.WriteLine(x);

            }
#endif
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}