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
            //Loaded += OnLoaded;//));
            AddHandler(FormattedTextControl3.RenderCompleteEvent, new RoutedEventHandler(OnLoaded));
        }

        private async void OnLoaded(object sender, RoutedEventArgs args)
        {
            
            Action<string> d = s => DebugUtils.WriteLine(s);
            var c = CodeControl;
            var lines = new string[] {"","/* foo */", "public "};
            var first = true;
            foreach (var line in lines)
            {
                if (!first) await c.DoInput("\r\n").ConfigureAwait(true);
                first = false;
                foreach (var ch in line)
                {
                    DebugUtils.WriteLine("Input is char '" + ch + "'");
                    await c.DoInput(ch.ToString()).ConfigureAwait(true);
                    if (c.InsertionLine != null) d(c.InsertionLine.Length.ToString());
                }
            }

            c.DoInput("c");
        }
    }
}
