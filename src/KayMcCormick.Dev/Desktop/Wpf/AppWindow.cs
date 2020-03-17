using System.Windows ;
using Autofac ;

namespace KayMcCormick.Lib.Wpf
{
    public class AppWindow : Window
    {
        protected AppWindow ( ) {
        }

        protected AppWindow (ILifetimeScope lifetimeScope )
        {
            SetValue ( AttachedProperties.LifetimeScopeProperty , lifetimeScope ) ;
        }
    }
}
