using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using NLog.Config;
using NLog.Targets;

namespace WcfService1
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {
                var conf = new LoggingConfiguration();
                var t0 = new EventLogTarget ( "elog" ) ;
                t0.Source = "Application" ;
                conf.AddTarget ( t0 ) ;
                conf.AddRuleForAllLevels(t0);

                var target = new DebuggerTarget("debug");
                conf.AddTarget(target);
                conf.AddRuleForAllLevels(target);
                var t2 = new NLogViewerTarget ( "viewer" )
                         {
                             IncludeAllProperties = true
                           , IncludeCallSite      = true
                           , IncludeSourceInfo    = true
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