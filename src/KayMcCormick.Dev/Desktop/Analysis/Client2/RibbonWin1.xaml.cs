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
using KayMcCormick.Dev;
using ReactiveUI;

namespace Client2
{
    /// <summary>
    /// Interaction logic for RibbonWin1.xaml
    /// </summary>
    public partial class RibbonWin1 : Window
    {
        public RibbonWin1()
        {
            InitializeComponent();


        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            DebugUtils.WriteLine($"{e.Property.Name} = {e.NewValue}");
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
            
        }
    }

    public class Ribbon1 : Ribbon
    {
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            DebugUtils.WriteLine($"{e.Property.Name} = {e.NewValue}");
        }
    }

}
