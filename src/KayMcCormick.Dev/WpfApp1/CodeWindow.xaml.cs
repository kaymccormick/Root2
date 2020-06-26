using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using JetBrains.Annotations;
using KayMcCormick.Dev;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for CodeWindow.xaml
    /// </summary>
    public partial class CodeWindow : Window, INotifyPropertyChanged
    {
        private string _sourceText = "";//"public class Test {\r\nint foo;\r\n}\r\n";

        public CodeWindow()
        {
            var f = ((App) Application.Current).LoadFilename;
            if (f != null) SourceText = File.ReadAllText(f);
            InitializeComponent();
            Code.Focus();
            Action<string> d = s => DebugUtils.WriteLine(s);
            Loaded += (sender, args) =>
            {
                var c = Code.CodeControl.CodeControl;
                string[] lines = new string[] {"/* foo */", "public "};
                bool first = true;
                foreach (var line in lines)
                {
                    if(!first)
                        c.DoInput("\r\n");
                    first = false;
                    foreach (var ch in line)
                    {
                        DebugUtils.WriteLine("Input is char '" + ch + "'");
                        c.DoInput(ch.ToString());
                        if (c.InsertionLine != null) d(c.InsertionLine.Length.ToString());
                    }
                    
                }
                c.DoInput("c");
                
            };
        }

        public string SourceText
        {
            get { return _sourceText; }
            set
            {
                if (value == _sourceText) return;
                _sourceText = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
