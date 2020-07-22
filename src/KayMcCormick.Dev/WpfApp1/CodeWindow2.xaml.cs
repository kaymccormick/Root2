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
using System.Windows.Shapes;
using AnalysisControls;
using KayMcCormick.Dev;
using RoslynCodeControls;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for CodeWindow2.xaml
    /// </summary>
    public partial class CodeWindow2 : Window
    {
        public CodeWindow2()
        {
            InitializeComponent();
            CodeControl.Focus();
            Keyboard.Focus(CodeControl);
            
//            AddHandler(RoslynCodeControl.RenderCompleteEvent, new RoutedEventHandler(OnLoaded));
        }


    }
}
