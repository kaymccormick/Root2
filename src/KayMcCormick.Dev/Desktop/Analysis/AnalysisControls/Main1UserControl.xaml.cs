using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AnalysisControls;

namespace AnalysisControls
{
    /// <summary>
    /// Interaction logic for Main1UserControl.xaml
    /// </summary>
    public partial class Main1UserControl : UserControl
    {

        public static readonly DependencyProperty Main1Property = DependencyProperty.Register(
            "Main1", typeof(Main1), typeof(Main1UserControl), new PropertyMetadata(default(Main1)));

        public Main1 Main1
        {
            get { return (Main1) GetValue(Main1Property); }
            set { SetValue(Main1Property, value); }
        }
        public Main1UserControl()
        {
            InitializeComponent();
        }
    }
}
