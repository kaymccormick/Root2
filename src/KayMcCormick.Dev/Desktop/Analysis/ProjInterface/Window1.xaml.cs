using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Reactive.Concurrency ;
using System.Reactive.Linq ;
using System.Text.Json ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Controls.Ribbon ;
using System.Windows.Input ;
using System.Windows.Media ;
using System.Windows.Navigation ;
using System.Windows.Threading ;
using AnalysisAppLib ;
using AnalysisAppLib.Project ;
using AnalysisControls ;
using Autofac ;
using Autofac.Features.Metadata ;
using AvalonDock.Layout ;
using FindLogUsages ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Dev.Container ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Lib.Wpf ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.Classification ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.Text ;
using Microsoft.Extensions.Logging ;
using Microsoft.VisualStudio.Threading ;
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
        private readonly UiElementTypeConverter _converter ;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private const string              WindowViewTitle = "Docking window" ;
        private       DockWindowViewModel _viewModel ;
        private       ILifetimeScope      _beginLifetimeScope ;
        private       MyCacheTarget2      _cacheTarget ;

        public Window1 ( )
        {
            DebugUtils.WriteLine ( "Warning, no scope" ) ;
            InitializeComponent ( ) ;
        }

        #region Overrides of FrameworkElement
        protected override void OnInitialized ( EventArgs e )
        {
            base.OnInitialized ( e ) ;
            var z = _beginLifetimeScope.Resolve < IAnalyzeCommand > ( ) ;
            z.AnalyzeCommandAsync (
                                   new ProjectBrowserNode ( )
                                   {
                                       SolutionPath =
                                           @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v2\LogTest\LogTest.sln"
                                   }
                                 , new ActionBlock < RejectedItem > ( item => { } )
                                  ) ;
        }
        #endregion

        public Window1 ( [ NotNull ] ILifetimeScope lifetimeScope ) : this (
                                                                            lifetimeScope
                                                                          , null
                                                                          , null
                                                                           )
        {
        }

        public Window1 (
            [ NotNull ] ILifetimeScope lifetimeScope
          , DockWindowViewModel        viewModel
          , UiElementTypeConverter     converter
        )
        {
            if ( lifetimeScope == null )
            {
                throw new ArgumentNullException ( nameof ( lifetimeScope ) ) ;
            }

            if ( lifetimeScope.IsRegistered ( typeof ( MyCacheTarget2 ) ) )
            {
                _cacheTarget = lifetimeScope.Resolve < MyCacheTarget2 > ( ) ;
            }

            _converter = converter ;
            _cacheTarget?.Cache.SubscribeOn ( Scheduler.Default )
                         .Buffer ( TimeSpan.FromMilliseconds ( 100 ) )
                         .Where ( x => x.Any ( ) )
                         .ObserveOnDispatcher ( DispatcherPriority.Background )
                         .Subscribe (
                                     infos => {
                                         foreach ( var json in infos )
                                         {
                                             var i =
                                                 JsonSerializer.Deserialize < LogEventInstance > (
                                                                                                  json
                                                                                                , new
                                                                                                      JsonSerializerOptions ( )
                                                                                                 ) ;
                                             List1.Items.Add ( new LogEventMisc ( i , json ) ) ;
                                         }
                                     }
                                    ) ;



            //SetValue ( AttachedProperties.LifetimeScopeProperty , (ILifetimeScope)lifetimeScope ) ;
            _beginLifetimeScope = lifetimeScope.BeginLifetimeScope ( ConfigurationAction ) ;
            var lf = _beginLifetimeScope ;
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

        private void ConfigurationAction ( ContainerBuilder builder )
        {
            builder.Register (
                              ( r , p ) => new Myw (
                                                    x => {
                                                        List1.Dispatcher.Invoke (
                                                                                 ( ) => {
                                                                                     List1
                                                                                        .Items.Add (
                                                                                                    x
                                                                                                   ) ;
                                                                                 }
                                                                                ) ;
                                                    }
                                                   )
                             )
                   .As < ILoggerProvider > ( ) .SingleInstance();
            builder.RegisterType < MyL > ( ).AsImplementedInterfaces ( ).SingleInstance() ;
            builder.Register < Action < Tuple < Workspace , Document > > > ( x => Action ) ;

            builder.Register < Action < IEventMisc > > ( x => Action2 ) ;
            builder.Register < Action < Document > > ( Delegate ) ;
            builder.Register < Action < string > > ( x => d => { } ) ;
            builder.Register < Action < ILogInvocation > > (
                                                            x => d => {
                                                                List1.Dispatcher.Invoke (
                                                                                         ( ) => {
                                                                                             List1
                                                                                                .Items
                                                                                                .Add (
                                                                                                      new
                                                                                                          InvocationMisc (
                                                                                                                          null
                                                                                                                        , d
                                                                                                                         )
                                                                                                     ) ;
                                                                                         }
                                                                                        ) ;
                                                            }
                                                           ) ;
            builder.RegisterInstance ( new LayoutService ( leftAnchorablePane ) )
                   .SingleInstance ( )
                   .WithCallerMetadata ( ) ;
            builder.RegisterInstance ( new PaneService ( ) )
                   .SingleInstance ( )
                   .WithCallerMetadata ( ) ;
            builder.RegisterType < HandleExceptionImpl > ( )
                   .As < IHandleException > ( )
                   .InstancePerLifetimeScope ( )
                   .WithCallerMetadata ( ) ;
        }

        private Action < Document > Delegate ( IComponentContext x )
        {
            return d => {
                async Task Callback ( )
                {
                    var st1 = await d.GetTextAsync();
                    var s = new StackPanel { Orientation = Orientation.Vertical } ;
                    foreach ( var textLine in st1.Lines )
                    {
                        var bl = new TextBlock ( )
                                 {
                                     FontFamily = new FontFamily ( "Lucida Console" )
                                   , Text       = textLine.ToString ( )
                                 } ;
                        s.Children.Add ( bl ) ;
                    }

                    var t = new ScrollViewer { Content = s } ;
                    Docpane.Children.Add ( new LayoutDocument { Title = d.Name , Content = t } ) ;
                }

                List1.Dispatcher.Invoke ( Callback ) ;
            } ;
        }

        private void Action2 ( IEventMisc obj )
        {
//            if ( obj.Level == MiscLevel.DEBUG ) return ;
            List1.Dispatcher.Invoke (
                                     ( ) => {
                                         List1.Items.Add ( obj ) ;
                                     }
                                    ) ;
        }

        private void Action ( Tuple < Workspace , Document > d )
        {
            List1.Dispatcher.InvokeAsync (
                                          async ( ) => {
                                              var w = d.Item1 ;
                                              var d2 = d.Item2 ;
                                              var sm = await d2.GetSemanticModelAsync ( ) ;
                                              var st1 = await d2.GetTextAsync ( ) ;

                                              if ( sm     != null
                                                   && st1 != null )
                                              {
                                                  var cc = Classifier.GetClassifiedSpans (
                                                                                          sm
                                                                                        , TextSpan
                                                                                             .FromBounds (
                                                                                                          0
                                                                                                        , st1
                                                                                                             .Length
                                                                                                         )
                                                                                        , w
                                                                                         ) ;
                                                  foreach ( var classifiedSpan in cc )
                                                  {
                                                      //List1.Items.Add ( classifiedSpan ) ;
                                                  }
                                              }
                                          }
                                         ) ;
        }


        public DockWindowViewModel ViewModel
        {
            get { return _viewModel ; }
            set { _viewModel = value ; }
        }


        [ NotNull ] public string ViewTitle { get { return WindowViewTitle ; } }


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
                        case Window w :
                            w.Show ( ) ;
                            break ;
                        case Control c :
                        {
                            var doc = new LayoutDocument { Content = c , Title = val.ViewTitle } ;
                            Docpane.Children.Add ( doc ) ;
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
                                  Extension   = ".cs"
                                , Description = "CSharp source files"
                                 ,
                                  // ReSharper disable once UnusedAnonymousMethodSignature
#pragma warning disable 1998
                                  Handler = async delegate ( string filename ) {
#pragma warning restore 1998
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
                    var analyzeCommand = _beginLifetimeScope.Resolve < IAnalyzeCommand > ( ) ;

                    var node = new ProjectBrowserNode
                               {
                                   Name = "Loaded solution" , SolutionPath = filename
                               } ;
                    DebugUtils.WriteLine ( "await command" ) ;
                    await analyzeCommand.AnalyzeCommandAsync (
                                                              node
                                                            , new ActionBlock < RejectedItem > (
                                                                                                x => Debug
                                                                                                   .WriteLine (
                                                                                                               x.Statement
                                                                                                                .ToString ( )
                                                                                                              )
                                                                                               )
                                                             )
                                        .ConfigureAwait ( false ) ;
                    DebugUtils.WriteLine ( "herecommand" ) ;
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

        #region Overrides of RibbonWindow
        public override void OnApplyTemplate ( )
        {
            base.OnApplyTemplate ( ) ;
            FrameDoc1.NavigationService.Navigating += ( sender , args ) => {
                DebugUtils.WriteLine ( args.Content ) ;
                // if ( args.Content is ResourceNodeInfo node )
                // {
                if ( args.Content is UIElement )
                {
                    return ;
                }

                var ctl = _converter.ControlForValue ( args.Content , 1 ) ;
                FrameDoc1.Content = ctl ;
                args.Cancel       = true ;
                // }
            } ;
        }
        #endregion

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
            Docpane.SelectedContentIndex = Docpane.Children.IndexOf ( FrameDocument ) ;
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

            Docpane.SelectedContentIndex = Docpane.Children.IndexOf ( FrameDocument ) ;
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

        public Action < string > Handler { get ; set ; }
    }
}