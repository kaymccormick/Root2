using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;
using System.Text ;
using System.Threading.Tasks ;
using Autofac ;
using NLog ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class IocModule : Module
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        #region Overrides of Module
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        // ReSharper disable once MemberCanBeProtected.Global
        public abstract void DoLoad ( ContainerBuilder builder ) ;

        #region Overrides of Module
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load ( ContainerBuilder builder ) { DoLoad ( builder ) ; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        protected static void LogRegistration ( params object[] p )
        {
            var x = p.Prepend ( "Registering" ) ;
            Logger.Trace ( string.Join ( " " , x ) ) ;
        }
    }
}