using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Text ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Documents ;
using System.Windows.Input ;
using System.Windows.Media ;
using System.Windows.Media.Imaging ;
using System.Windows.Shapes ;
using Autofac ;
using Autofac.Features.Metadata ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;
using Microsoft.Build.Logging.StructuredLogger ;
using ProjLib.Interfaces ;
using Xceed.Wpf.AvalonDock.Layout ;

namespace ProjInterface
{
    [ TitleMetadata ( "Window 1" ) ]
    public partial class Window1 : AppWindow , IView1 , IView < DockWindowViewModel >
    {
        private string              _viewTitle = "window 1" ;
        private DockWindowViewModel _viewModel ;

        public Window1 ( ) { InitializeComponent ( ) ; }

        public Window1 ( ILifetimeScope lifetimeScope , DockWindowViewModel viewModel ) :
            base ( lifetimeScope )
        {
            _viewModel = viewModel ;
            InitializeComponent ( ) ;
        }

        #region Implementation of IView1
        public string ViewTitle { get => _viewTitle ; set => _viewTitle = value ; }
        #endregion

        #region Implementation of IView<out DockWindowViewModel>
        public DockWindowViewModel ViewModel { get => _viewModel ; set => _viewModel = value ; }
        #endregion

        private void CommandBinding_OnExecuted ( object sender , ExecutedRoutedEventArgs e )
        {
            if ( e.Parameter is Meta < Lazy < IView1 > > meta )
            {
                var val = meta.Value.Value ;
                if ( val is Window  w ) 
                {
                    w.Show();
                } else if ( val is Control c )
                {
                    LayoutDocument doc = new LayoutDocument();
                    doc.Content = c ;
                    docpane.Children.Add ( doc ) ;
                }
            }

//            MessageBox.Show ( e.Parameter.ToString ( ) ) ;
        }
    }
}