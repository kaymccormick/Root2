using System;
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
using KmDevWpfControls;

namespace ControlsDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            PresentationTraceSources.Refresh();
            InitializeComponent();
            AllowDrop = true;

            VisualTreeViewModel.RootVisual = this;
            
            
            
            

        }

        public VisualTreeViewModel VisualTreeViewModel { get; set; } = new VisualTreeViewModel();
        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
            if (!e.Handled)
            {
                e.Effects = e.AllowedEffects;
                e.Handled = true;

            }
        }

        private void MainWindow_OnDrop(object sender, DragEventArgs e)
        {
            foreach (var format in e.Data.GetFormats())
            {
                Debug.WriteLine($"{format}");
                try
                {
                    Debug.WriteLine($"{e.Data.GetData(format)}");
                }
                catch
                {

                }
            }
        }
    }
}
