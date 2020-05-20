using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using KmDevWpfControls;
using Xunit;

namespace WpfUiTests
{
    public class Class1
    {
        [WpfFact]
        public void Test1()
        {
            List<object> items = new List<object> {"test"};
            Grid g = new Grid();
            
            CustomTreeView t1 = new CustomTreeView();
            g.Children.Add(t1);
            g.RenderTransform = new ScaleTransform(2, 2);
            t1.ItemsSource = items;
            Window w = new Window {Content = g};
            w.ShowDialog();
        }



        [WpfFact]
        public void TestAssembliesControl()
        {
            var ac = new AssembliesControl1();
            Grid g = new Grid();
            g.Children.Add(ac);
            //g.RenderTransform = new ScaleTransform(2, 2);
            Window w = new Window { Content = g, FontSize = 20.0 };
            w.ShowDialog();
        }

        [WpfFact]
        public void TestAssemblyResourcesTree1()
        {
            var ac = new AssemblyResourceTree1 {Assembly = typeof(KmDevWpfControls.AssembliesControl1).Assembly};
            Grid g = new Grid();
            g.Children.Add(ac);
            //g.RenderTransform = new ScaleTransform(2, 2);
            Window w = new Window { Content = g, FontSize = 20.0 };
            w.ShowDialog();
        }
        [WpfFact]
        public void TestVisualTreeView1()
        {
            var ac = new VisualTreeView1();
            Grid g = new Grid();
            g.Children.Add(ac);
            //g.RenderTransform = new ScaleTransform(2, 2);
            Window w = new Window { Content = g, FontSize = 20.0 };
       
            var m = new VisualTreeViewModel();
            m.CurrentVisual = w;
        ac.SetBinding(VisualTreeView1.RootVisualProperty, new Binding("CurrentVisual") { Source =m});

            w.ShowDialog();

        }
    }
}
