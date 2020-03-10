using System.Windows ;
using Autofac ;
using JetBrains.Annotations ;

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
