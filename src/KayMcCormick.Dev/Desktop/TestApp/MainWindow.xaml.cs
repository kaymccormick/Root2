using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autofac ;
using KayMcCormick.Lib.Wpf ;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AppWindow
    {
        public MainWindow ( ILifetimeScope lifetimeScope ) : base ( lifetimeScope )
        {
            InitializeComponent();
        }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
