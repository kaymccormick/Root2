using System.Windows ;
using Autofac ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    public class AppWindow : Window
    {
        /// <summary>
        /// </summary>
        public AppWindow ( ) { }

        /// <summary>
        /// </summary>
        /// <param name="lifetimeScope"></param>
        public AppWindow ( ILifetimeScope lifetimeScope )
        {
            SetValue ( AttachedProperties.LifetimeScopeProperty , lifetimeScope ) ;
        }
    }
}