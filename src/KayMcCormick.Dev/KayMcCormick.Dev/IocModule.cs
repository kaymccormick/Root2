using Autofac ;
using NLog ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// </summary>
    public abstract class IocModule : Module
    {
        // ReSharper disable once UnusedMember.Local
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

        #region Overrides of Module
        #endregion
    }
}