using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls.Ribbon;
using AnalysisControls;
using AnalysisControls.RibbonM;
using JetBrains.Annotations;

namespace Client2
{
    /// <summary>
    /// Interaction logic for TestRibbonWindow.xaml
    /// </summary>
    public partial class TestRibbonWindow2 : RibbonWindow , INotifyPropertyChanged
    {
        private RibbonModel _ribbon;

        public TestRibbonWindow2() 
        {
            InitializeComponent();
            RibbonModel m = new RibbonModel();
            RibbonTabProvider1 p = new RibbonTabProvider1();
            var t = p.ProvideModelItem(null);
            m.RibbonItems.Add(t);
            RibbonViewGroupProviderBaseImpl x = new RibbonViewGroupProviderBaseImpl();
            t.Items.Add(x.ProvideModelItem(null));
            Ribbon = m;
        }

        public RibbonModel Ribbon
        {
            get { return _ribbon; }
            set
            {
 _ribbon = value;
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