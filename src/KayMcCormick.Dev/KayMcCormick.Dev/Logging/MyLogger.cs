using System ;
using NLog ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class MyLogger : Logger
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'MyLogger'
    {
        /// <summary>Raises the event when the logger is reconfigured.</summary>
        /// <param name="e">Event arguments</param>
        protected override void OnLoggerReconfigured(EventArgs e)
        {
            base.OnLoggerReconfigured(e);
            SetProperty(
                        "Cheese", "Food");
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'MyLogger.MyLogger()'
        public MyLogger()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'MyLogger.MyLogger()'
        {
            SetProperty(
                        "Cheese", "Food");
        }
       

    }
}