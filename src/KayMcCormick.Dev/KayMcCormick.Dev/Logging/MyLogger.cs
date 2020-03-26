using System ;
using NLog ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class MyLogger : Logger

    {
        /// <summary>Raises the event when the logger is reconfigured.</summary>
        /// <param name="e">Event arguments</param>
        protected override void OnLoggerReconfigured(EventArgs e)
        {
            base.OnLoggerReconfigured(e);
            SetProperty(
                        "Cheese", "Food");
        }


        /// <summary>
        /// Random test constructor.
        /// </summary>
        public MyLogger()

        {
            SetProperty(
                        "Cheese", "Food");
        }
       

    }
}