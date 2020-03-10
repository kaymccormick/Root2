using System.Windows ;
using Autofac ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{
    public class AppWindow : Window
    {
        [ UsedImplicitly ] private readonly ILifetimeScope _lifetimeScope ;

        public AppWindow ( ) {
        }

        public AppWindow (ILifetimeScope lifetimeScope )
        {
            _lifetimeScope = lifetimeScope ;
            SetValue ( AttachedProperties.LifetimeScopeProperty , lifetimeScope ) ;
        }
    }
}
