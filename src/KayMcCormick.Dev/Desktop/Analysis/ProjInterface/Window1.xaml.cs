using System ;
using System.Activities.Expressions ;
using System.Diagnostics ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Input ;
using System.Windows.Interop ;
using System.Windows.Navigation ;
using Autofac ;
using Autofac.Features.Metadata ;
using AvalonDock.Layout ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;
using NLog ;

namespace ProjInterface
{
    [ TitleMetadata ( "Docking window" ) ]
    public partial class Window1 : AppWindow , IView1 , IView < DockWindowViewModel >
    {
        private string              _viewTitle = "Docking window" ;
        private DockWindowViewModel _viewModel ;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public Window1 ( ) { InitializeComponent ( ) ; }

        public Window1 ( ILifetimeScope lifetimeScope , DockWindowViewModel viewModel ) :
            base ( )
        {
            var lf = lifetimeScope.BeginLifetimeScope(
                                                      builder => {
                                                          builder.RegisterType<HandleExceptionImpl>()
                                                                 .As<IHandleException>().InstancePerLifetimeScope();
                                                      }
                                                     );
            SetValue(AttachedProperties.LifetimeScopeProperty, lf);

            ViewModel = viewModel ;
            var wih = new WindowInteropHelper ( this ) ;
            var hWnd = wih.Handle ;
            viewModel.SethWnd ( hWnd ) ;
            InitializeComponent ( ) ;
        }

        #region Overrides of FrameworkElement
        public override async void OnApplyTemplate ( )
        {
            base.OnApplyTemplate ( ) ;
            if ( _viewModel != null )
            {
                _viewModel.ResourcesElement = this ;
                // await _viewModel.LoginSilentAsync ( ).ConfigureAwait ( true ) ;
            }

            AddHandler (
                        TypeControl.TypeActivatedEvent
                      , new TypeControl.TypeActivatedEventHandler ( Target )
                       ) ;
        }

        private void Target ( object sender , TypeActivatedEventArgs e )
        {
            BrowserFrame.NavigationService.Navigate (
                                                     new Page ( )
                                                     {
                                                         Content = new TypeInfoControl ( )
                                                                   {
                                                                       DataContext = e.ActivatedType
                                                                   }
                                                         // Content = new TypeControl ( )
                                                         //           {
                                                         //               RenderedType =
                                                         //                   e.ActivatedType
                                                         //           }
                                                     }
                                                    ) ;

            docpane.SelectedContentIndex = docpane.Children.IndexOf ( FrameDocument ) ;
            Debug.WriteLine ( e.ActivatedType.FullName ) ;
        }
        #endregion
        #region Implementation of IView1
        public string ViewTitle { get { return _viewTitle ; } set { _viewTitle = value ; } }
        #endregion

        #region Implementation of IView<out DockWindowViewModel>
        public DockWindowViewModel ViewModel
        {
            get { return _viewModel ; }
            set { _viewModel = value ; }
        }
        #endregion

        private void CommandBinding_OnExecuted ( object sender , ExecutedRoutedEventArgs e )
        {
            Logger.Warn ( nameof ( CommandBinding_OnExecuted ) ) ;
            if ( e.Parameter is Meta < Lazy < IView1 > > meta )
            {
                try
                {
                    var val = meta.Value.Value ;
                    if ( val is Window w )
                    {
                        w.Show ( ) ;
                    }
                    else if ( val is Control c )
                    {
                        var doc = new LayoutDocument { Content = c } ;
                        doc.Title = val.ViewTitle ;
                        docpane.Children.Add ( doc ) ;
                    }
                }
                catch ( Exception ex )
                {
                    Debug.WriteLine ( ex.ToString ( ) ) ;
                }
            }
        }

        private void CommandBinding_OnExecuted2 ( object sender , ExecutedRoutedEventArgs e )
        {
            System.Windows.Application.Current.Shutdown ( ) ;
        }

        private void BrowserFrame_OnNavigating ( object sender , NavigatingCancelEventArgs e )
        {
            var uri = e.Uri ;
            Type t = null ;
            if ( e.ExtraData is NavState n )
            {
                t = n.RenderedType ;
            } else if ( uri != null
                        && uri.IsAbsoluteUri
                        && uri.Scheme == "obj" )
            {
                var stringToUnescape = uri.AbsolutePath.Substring ( 1 ) ;
                var unescapeDataString = Uri.UnescapeDataString ( stringToUnescape ) ;
                t=  Type.GetType ( unescapeDataString ) ;
            }
            if(t != null) {
                if ( BrowserFrame.Content is TypeInfoControl tic )
                {
                    tic.DataContext              = t ;
                    docpane.SelectedContentIndex = docpane.Children.IndexOf ( FrameDocument ) ;
                    e.Cancel = true ;
                    
                }
            }
        }
    }
}