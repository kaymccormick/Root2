using System.Linq ;
using Autofac ;
using NLog ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// </summary>
    public abstract class IocModule : Module
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        public abstract void DoLoad ( ContainerBuilder builder ) ;

        #region Overrides of Module
        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load ( ContainerBuilder builder ) { DoLoad ( builder ) ; }
        #endregion

        /// <summary>
        /// </summary>
        /// <param name="p"></param>
        protected static void LogRegistration ( params object[] p )
        {
            var x = p.Prepend ( "Registering" ) ;
            Logger.Trace ( string.Join ( " " , x ) ) ;
        }

        #region Overrides of Module
        #endregion
    }
}