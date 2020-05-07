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

namespace Client2
{
    /// <summary>
    /// Interaction logic for Client2Window1.xaml
    /// </summary>
    [ShortKeyMetadata("Client2Window1")]
    public partial class Client2Window1 : RibbonWindow
    {
        public Client2Window1(ILifetimeScope scope)
        {
            SetValue(AttachedProperties.LifetimeScopeProperty, scope);
            InitializeComponent();
        }
    }
}
