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
        public AppWindow (ILifetimeScope lifetimeScope )
        {
            SetValue ( AttachedProperties.LifetimeScopeProperty , lifetimeScope ) ;
        }
    }
}
