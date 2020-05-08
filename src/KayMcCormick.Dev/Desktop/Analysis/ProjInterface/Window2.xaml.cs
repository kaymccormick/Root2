using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Autofac;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf;

namespace ProjInterface
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    [ShortKeyMetadata("Window2")]
    public partial class Window2 : RibbonWindow
    {
        private readonly ILifetimeScope _scope;

        public Window2(ILifetimeScope scope)
        {
            SetValue(AttachedProperties.LifetimeScopeProperty, scope);
            InitializeComponent();  
        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Main1.RaiseEvent(e);
        }
    }
}
