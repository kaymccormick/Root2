using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserControl = System.Windows.Controls.UserControl;

namespace AnalysisControls
{
    /// <summary>
    /// Interaction logic for PGrid.xaml
    /// </summary>
    public partial class PGrid : UserControl
    {
        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register(
            "SelectedObject", typeof(object), typeof(PGrid), new PropertyMetadata(default(object), OnSelectedObjectChanged));

        public object SelectedObject
        {
            get { return (object)GetValue(SelectedObjectProperty); }
            set { SetValue(SelectedObjectProperty, value); }
        }

        private static void OnSelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PGrid)d).OnSelectedObjectChanged((object)e.OldValue, (object)e.NewValue);
        }



        protected virtual void OnSelectedObjectChanged(object oldValue, object newValue)
        {
            (Host.Child as PropertyGrid).SelectedObject = newValue;
        }


        public PGrid()
        {
            InitializeComponent();
        }
    }
}
