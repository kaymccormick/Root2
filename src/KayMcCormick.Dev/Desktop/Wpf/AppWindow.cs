using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows ;
using Autofac ;

namespace KayMcCormick.Lib.Wpf
{
    public class AppWindow : Window
    {
        private readonly ILifetimeScope _lifetimeScope ;

        public AppWindow ( ) {
        }

        public AppWindow (ILifetimeScope lifetimeScope )
        {
            _lifetimeScope = lifetimeScope ;
            SetValue ( AttachedProperties.LifetimeScopeProperty , lifetimeScope ) ;
        }
    }
}
