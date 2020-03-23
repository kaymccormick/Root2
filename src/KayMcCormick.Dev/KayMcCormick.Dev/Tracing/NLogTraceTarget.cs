using NLog ;
using NLog.Targets ;

namespace KayMcCormick.Dev.Tracing
{

    /// <summary>
    /// 
    /// </summary>
    [Target("KayTrace")]
    public class NLogTraceTarget : TargetWithLayoutHeaderAndFooter
    {
        #region Overrides of Target
        protected override void Write ( LogEventInfo logEvent )
        {
            var message = RenderLogEvent (Layout, logEvent ) ;
            //Tracing.ExtLib.fnEventWriteSETUP_LOGGING_EVENT_AssumeEnabled ( message ) ;
        }
        #endregion
    }
}
