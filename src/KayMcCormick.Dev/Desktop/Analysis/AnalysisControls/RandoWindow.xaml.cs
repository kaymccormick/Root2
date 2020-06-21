using System.IO;
using System.Reactive.Subjects;
using System.Windows;

namespace AnalysisControls
{
    /// <summary>
    /// Interaction logic for RandoWindow.xaml
    /// </summary>
    public partial class RandoWindow : Window
    {
        private ReplaySubject<object> s = new ReplaySubject<object>();
        public RandoWindow()
        {
            
            InitializeComponent();
            S.Observable = s;
            S.ItemType = typeof(System.IO.FileInfo);
            s.OnNext(new FileInfo(@"C:\temp\z"));
        }
    }
}
