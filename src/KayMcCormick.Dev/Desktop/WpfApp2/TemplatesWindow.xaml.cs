using System;
using System.Collections.ObjectModel ;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml ;
using NLog ;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for TemplatesWindow.xaml
    /// </summary>
    public partial class TemplatesWindow : Window
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        // ReSharper disable once UnusedMember.Global
        public ObservableCollection < TemplateInfo > DesignSource { get ; } = new ObservableCollection < TemplateInfo >();
        public ObservableCollection < TemplateInfo > DataTemplateList { get ; }

        public TemplatesWindow( ObservableCollection < TemplateInfo> dataTemplateList )
        {
            DataTemplateList = dataTemplateList ;
            InitializeComponent();
        }

        private void CommandBinding_OnExecuted ( object sender , ExecutedRoutedEventArgs e )
        {
            var xmlViewer = new XmlViewer();
            var x = new XmlDocument();
            x.LoadXml ( ( string ) e.Parameter ) ;
            xmlViewer.XmlDocument = x ;
            var w = new Window { Content = xmlViewer } ;
            w.ShowDialog ( ) ;
        }

        private void UIElement_OnLayoutUpdated ( object sender , EventArgs e )
        {
            Logger.Debug ( "Layout Updated" ) ;
            try
            {
                var g = ( GridView ) ListView.View ;
                var unit = ( dockPanel.ActualWidth - 30 ) / 4 ;
                g.Columns[ 0 ].SetCurrentValue(GridViewColumn.WidthProperty, unit) ;
                g.Columns[ 1 ].SetCurrentValue(GridViewColumn.WidthProperty, unit) ;
                g.Columns[ 2 ].SetCurrentValue(GridViewColumn.WidthProperty, unit * 2) ;
            }
            catch ( Exception ex )
            {
                Logger.Info ( ex , ex.Message ) ;
            }
        }
    }
}
