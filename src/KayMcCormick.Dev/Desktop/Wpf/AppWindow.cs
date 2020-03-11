using System.Windows ;
using Autofac ;

namespace KayMcCormick.Lib.Wpf
{
    public class AppWindow : Window
    {
        public AppWindow ( ) {
        }

        public AppWindow (ILifetimeScope lifetimeScope )
        {
            SetValue ( AttachedProperties.LifetimeScopeProperty , lifetimeScope ) ;
        }
    }
}
