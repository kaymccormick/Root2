using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace KmDevWpfControls
{
    /// <summary>
    /// Interaction logic for TraceView.xaml
    /// </summary>
    public partial class TraceView : UserControl
    {
        public TraceView()
        {
            InitializeComponent();
        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            PresentationTraceSources.Refresh();
            view.Refresh();
        }

        private void CommandBinding_OnExecuted2(object sender, ExecutedRoutedEventArgs e)
        {
            TraceListener x = null;
            //XmlWriterTraceListener a = new XmlWriterTraceListener();
                        

        }

        public static IEnumerable TraceOptions { get; set; } = Enum.GetValues(typeof(TraceOptions)).Cast<TraceOptions>().Select(o=>new CheckableModelItem<TraceOptions>(o));
    }

    public class CheckableModelItem<T>
    {
        public T Item { get; }

        public CheckableModelItem(T item)
        {
            Item = item;
        }
    }
}
