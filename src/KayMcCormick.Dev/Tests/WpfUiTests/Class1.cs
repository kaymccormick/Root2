using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
            var items = new List<object> {"test"};
            var g = new Grid();

            var t1 = new CustomTreeView();
            g.Children.Add(t1);
            g.RenderTransform = new ScaleTransform(2, 2);
            t1.ItemsSource = items;
            var w = new Window {Content = g};
            w.Show();
        }


        [WpfFact]
        public void TestAssembliesControl()
        {
            var ac = new AssembliesControl1();
            var g = new Grid();
            g.Children.Add(ac);
            //g.RenderTransform = new ScaleTransform(2, 2);
            var w = new Window {Content = g, FontSize = 20.0};
            w.Show();
        }

        [WpfFact]
        public void TestAssemblyResourcesTree1()
        {
            var ac = new AssemblyResourceTree1 {Assembly = typeof(AssembliesControl1).Assembly};
            var g = new Grid();
            g.Children.Add(ac);
            //g.RenderTransform = new ScaleTransform(2, 2);
            var w = new Window {Content = g, FontSize = 20.0};
            w.Show();
        }

        [WpfFact]
        public void TestVisualTreeView1()
        {
            var ac = new VisualTreeView1();
            var g = new Grid();
            g.Children.Add(ac);
            //g.RenderTransform = new ScaleTransform(2, 2);
            var w = new Window {Content = g, FontSize = 20.0};

            var m = new VisualTreeViewModel {RootVisual = w};
            ac.SetBinding(VisualTreeView1.RootVisualProperty, new Binding("CurrentVisual") {Source = m});

            w.Show();
        }

        [WpfFact]
        public void TestTypeDetailsControl1()
        {
            var td = new TypeDetailsControl() {Type = typeof(TypeDetailsControl)};
            var w = new Window {Content = td};
            w.Show();
        }
        [WpfFact]
        public void TestTraceView()
        {
            var td = new TraceView();
            var w = new Window {Content = td};
            w.Show();
        }

        [Fact]
        public void T1()
        {
           var p = TypeDescriptor.GetProperties(typeof(XmlWriterTraceListener));

        }

    }
}