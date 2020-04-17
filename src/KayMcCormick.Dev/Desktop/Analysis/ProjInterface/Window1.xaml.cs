using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Threading.Tasks.Dataflow ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Controls.Ribbon ;
using System.Windows.Input ;
using System.Windows.Navigation ;
using AnalysisAppLib ;
using AnalysisAppLib.Project ;
using Autofac ;
using Autofac.Features.Metadata ;
using AvalonDock.Layout ;
using FindLogUsages ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Dev.Container ;
using KayMcCormick.Lib.Wpf ;

using Microsoft.Win32 ;
using NLog ;
using Application = System.Windows.Application ;

namespace ProjInterface
{
    [ TitleMetadata ( "Docking window" ) ]
    public sealed partial class Window1 : RibbonWindow
      , IViewWithTitle
      , IView < DockWindowViewModel >
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private const string _viewTitle = "Docking window" ;
        private          DockWindowViewModel _viewModel ;

        public Window1 ( )
        {
            DebugUtils.WriteLine ( "Warning, no scope" ) ;
            InitializeComponent ( ) ;
        }

        public Window1 ( [ NotNull ] ILifetimeScope lifetimeScope ) : this ( lifetimeScope , null )
        {
        }

        public Window1 ( [ NotNull ] ILifetimeScope lifetimeScope , DockWindowViewModel viewModel )
        {
            if ( lifetimeScope == null )
            {
                throw new ArgumentNullException ( nameof ( lifetimeScope ) ) ;
            }

            //SetValue ( AttachedProperties.LifetimeScopeProperty , (ILifetimeScope)lifetimeScope ) ;
            var lf = lifetimeScope.BeginLifetimeScope (
                                                       builder => {
                                                           builder.RegisterInstance (
                                                                                     new
                                                                                         LayoutService (
                                                                                                        leftAnchorablePane
                                                                                                       )
                                                                                    )
                                                                  .SingleInstance ( ).WithCallerMetadata() ;
                                                           builder.RegisterInstance (
                                                                                     new
                                                                                         PaneService ( )
                                                                                    )
                                                                  .SingleInstance ( ).WithCallerMetadata() ;
                                                           builder
                                                              .RegisterType < HandleExceptionImpl
                                                               > ( )
                                                              .As < IHandleException > ( )
                                                              .InstancePerLifetimeScope ( ).WithCallerMetadata() ;
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


        [ NotNull ] public string ViewTitle { get { return _viewTitle ; } }


        private async void CommandBinding_OnExecuted (
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
                    switch ( val )
                    {
                        case Window w : w.Show ( ) ;
                            break ;
                        case Control c :
                        {
                            var doc = new LayoutDocument { Content = c , Title = val.ViewTitle } ;
                            docpane.Children.Add ( doc ) ;
                            break ;
                        }
                    }
                }
                catch ( Exception ex )
                {
                    DebugUtils.WriteLine ( ex.ToString ( ) ) ;
                }
            }

            var filters = new List < Filter >
                          {
                              new Filter { Extension = ".sln" , Description = "Solution Files" }
                            , new Filter { Extension = ".xml" , Description = "XML files" }
                            , new Filter
                              {
                                  Extension = ".cs" , Description = "CSharp source files",
                                  // ReSharper disable once UnusedAnonymousMethodSignature
#pragma warning disable 1998
                                  Handler = async delegate ( string filename ) {
#pragma warning restore 1998
                                      return ;
                                  }
                              }
                          } ;
            var dlg = new OpenFileDialog
                      {
                          DefaultExt = ".cs"
                        , Filter = string.Join (
                                                "|"
                                              , filters.Select (
                                                                f => $"{f.Description} (*{f.Extension})|*{f.Extension}"
                                                               )
                                               )
                      } ;

            var result = dlg.ShowDialog ( ) ;

            // Process open file dialog box results
            if ( result != true )
            {
                return ;
            }

            {
                var scope =
                    ( ILifetimeScope ) GetValue ( AttachedProperties.LifetimeScopeProperty ) ;

                // Open document
                var filename = dlg.FileName ;
                if ( Path.GetExtension ( filename ).ToLowerInvariant ( ) == ".sln" )
                {
                    var analyzeCommand = scope.Resolve < IAnalyzeCommand > ( ) ;
                    var node = new ProjectBrowserNode
                               {
                                   Name = "Loaded solution" , SolutionPath = filename
                               } ;
                    await analyzeCommand.AnalyzeCommandAsync (
                                                              node
                                                            , new ActionBlock < RejectedItem > (
                                                                                                x => Debug
                                                                                                   .WriteLine (
                                                                                                               x.Statement.ToString()
                                                                                                              )
                                                                                               )
                                                             ) ;
                    return ;
                }

                // ReSharper disable once UnusedVariable
                var view = scope.ResolveKeyed < IControlView > (
                                                                ApplicationEntityIds.File
                                                              , new NamedParameter (
                                                                                    "filename"
                                                                                  , filename
                                                                                   )
                                                               ) ;
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

            if ( t == null )
            {
                return ;
            }

            if ( ! ( BrowserFrame.Content is TypeInfoControl tic ) )
            {
                return ;
            }

            tic.DataContext              = t ;
            docpane.SelectedContentIndex = docpane.Children.IndexOf ( FrameDocument ) ;
            e.Cancel                     = true ;
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable twice UnusedParameter.Local
        private void ExecutePythonCode ( object sender , ExecutedRoutedEventArgs e )
        {
#if PYTHON
            var scope = ( ILifetimeScope ) GetValue ( AttachedProperties.LifetimeScopeProperty ) ;
            var model = scope.Resolve < PythonViewModel > ( ) ;
            model.ExecutePythonScript ( textEditor.Text ) ;
#endif
        }

        // AddHandler (
        //             TypeControl.TypeActivatedEvent
        //           , new TypeControl.TypeActivatedEventHandler ( Target )
        //            ) ;

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
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
            DebugUtils.WriteLine ( e.ActivatedType.FullName ) ;
        }

#region Implementation of IResourceResolver
        public object ResolveResource ( [ NotNull ] object resourceKey )
        {
            return TryFindResource ( resourceKey ) ;
        }
#endregion
    }

    public sealed class Filter
    {
        private string _extension ;
        private string _description ;

        public string Extension { get { return _extension ; } set { _extension = value ; } }

        public string Description { get { return _description ; } set { _description = value ; } }

        public Action<string> Handler { get ; set ; }
    }
}