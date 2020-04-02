using System ;
using System.Diagnostics ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Controls.Ribbon ;
using System.Windows.Input ;
using System.Windows.Interop ;
using System.Windows.Navigation ;
using AnalysisAppLib.ViewModel ;
using AnalysisControls.ViewModel ;
using Autofac ;
using Autofac.Features.Metadata ;
using AvalonDock.Layout ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Lib.Wpf ;
using NLog ;

namespace ProjInterface
{
    [ TitleMetadata ( "Docking window" ) ]
    public sealed partial class Window1 : RibbonWindow, IViewWithTitle , IView < DockWindowViewModel >
    {
        private static readonly Logger Logger =
            LogManager.GetCurrentClassLogger ( ) ;

        private readonly string              _viewTitle = "Docking window" ;
        private          DockWindowViewModel _viewModel ;

        public Window1 ( ) { InitializeComponent ( ) ; }

        public Window1 ( [ NotNull ] ILifetimeScope lifetimeScope ) : this ( lifetimeScope , null )
        {

        }

        public Window1 (
            [ NotNull ] ILifetimeScope      lifetimeScope
          ,  DockWindowViewModel viewModel
        )
        {
            if ( lifetimeScope == null )
            {
                throw new ArgumentNullException ( nameof ( lifetimeScope ) ) ;
            }

            //SetValue ( AttachedProperties.LifetimeScopeProperty , (ILifetimeScope)lifetimeScope ) ;
            var lf = lifetimeScope.BeginLifetimeScope (
                                                       "Window1 lifetimescope"
                                                     , builder => {
                                                           builder.RegisterInstance (
                                                                                     new
                                                                                         LayoutService (
                                                                                                        leftAnchorablePane
                                                                                                       )
                                                                                    )
                                                                  .SingleInstance ( ) ;
                                                           builder.RegisterInstance (
                                                                                     new
                                                                                         PaneService ( )
                                                                                    )
                                                                  .SingleInstance ( ) ;
                                                           builder
                                                              .RegisterType < HandleExceptionImpl
                                                               > ( )
                                                              .As < IHandleException > ( )
                                                              .InstancePerLifetimeScope ( ) ;
                                                       }
                                                      ) ;
            SetValue ( AttachedProperties.LifetimeScopeProperty , lf ) ;
            // lifetimeScope.ResolveOperationBeginning += ( sender , args ) => {
                // throw new AppComponentException ( "New lifetime scope should be used instead." ) ;
            // } ;

            ViewModel = viewModel ;
            // var wih = new WindowInteropHelper ( this ) ;
            // var hWnd = wih.Handle ;
            // viewModel.SethWnd ( hWnd ) ;
            InitializeComponent ( ) ;
        }

        
        public DockWindowViewModel ViewModel
        {
            get { return _viewModel ; }
            set { _viewModel = value ; }
        }
        
        
        public string ViewTitle { get { return _viewTitle ; } }
        

        private void CommandBinding_OnExecuted (
            object                              sender
          , [ NotNull ] ExecutedRoutedEventArgs e
        )
        {
            if ( e == null )
            {
                throw new ArgumentNullException ( nameof ( e ) ) ;
            }

            Logger.Warn ( nameof ( CommandBinding_OnExecuted ) ) ;
            if ( e.Parameter is Meta < Lazy < IViewWithTitle > > meta )
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
                        var doc = new LayoutDocument { Content = c , Title = val.ViewTitle } ;
                        docpane.Children.Add ( doc ) ;
                    }
                }
                catch ( Exception ex )
                {
                    Debug.WriteLine ( ex.ToString ( ) ) ;
                }
            }
        }

        private void QuitCommandOnExecuted ( object sender , ExecutedRoutedEventArgs e )
        {
            Application.Current.Shutdown ( ) ;
        }

        private void BrowserFrame_OnNavigating (
            object                                sender
          , [ NotNull ] NavigatingCancelEventArgs e
        )
        {
            if ( e == null )
            {
                throw new ArgumentNullException ( nameof ( e ) ) ;
            }

            var uri = e.Uri ;
            Type t = null ;
            if ( e.ExtraData is NavState n )
            {
                t = n.RenderedType ;
            }
            else if ( uri != null
                      && uri.IsAbsoluteUri
                      && uri.Scheme == "obj" )
            {
                var stringToUnescape = uri.AbsolutePath.Substring ( 1 ) ;
                var unescapeDataString = Uri.UnescapeDataString ( stringToUnescape ) ;
                t = Type.GetType ( unescapeDataString ) ;
            }

            if ( t != null )
            {
                if ( BrowserFrame.Content is TypeInfoControl tic )
                {
                    tic.DataContext              = t ;
                    docpane.SelectedContentIndex = docpane.Children.IndexOf ( FrameDocument ) ;
                    e.Cancel                     = true ;
                }
            }
        }

        private void ExecutePythonCode ( object sender , ExecutedRoutedEventArgs e )
        {
            var scope = ( ILifetimeScope ) GetValue ( AttachedProperties.LifetimeScopeProperty ) ;
            var model = scope.Resolve < PythonViewModel > ( ) ;
            model.ExecutePythonScript ( textEditor.Text ) ;
        }

            // AddHandler (
            //             TypeControl.TypeActivatedEvent
            //           , new TypeControl.TypeActivatedEventHandler ( Target )
            //            ) ;

        private void Target ( object sender , [ NotNull ] TypeActivatedEventArgs e )
        {
            if ( e == null )
            {
                throw new ArgumentNullException ( nameof ( e ) ) ;
            }

            BrowserFrame.NavigationService.Navigate (
                                                     new Page
                                                     {
                                                         Content = new TypeInfoControl
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
        
    }
}