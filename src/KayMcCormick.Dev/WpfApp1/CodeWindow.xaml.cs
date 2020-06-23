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
