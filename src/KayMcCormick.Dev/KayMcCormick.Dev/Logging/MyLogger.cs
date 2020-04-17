using System ;
using NLog ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedType.Global
    public class MyLogger : Logger

    {
        /// <summary>
        ///     Random test constructor.
        /// </summary>
        public MyLogger ( ) { SetProperty ( "Cheese" , "Food" ) ; }

        /// <summary>Raises the event when the logger is reconfigured.</summary>
        /// <param name="e">Event arguments</param>
        protected override void OnLoggerReconfigured ( EventArgs e )
        {
            base.OnLoggerReconfigured ( e ) ;
            SetProperty ( "Cheese" , "Food" ) ;
        }
    }
}