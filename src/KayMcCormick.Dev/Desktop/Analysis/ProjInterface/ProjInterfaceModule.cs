#region header
// Kay McCormick (mccor)
// 
// Deployment
// ProjInterface
// ProjInterfaceModule.cs
// 
// 2020-03-08-7:55 PM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.Reflection ;
using System.Windows ;
using System.Windows.Controls ;
using AnalysisAppLib ;
using AnalysisControls ;
using AnalysisControls.Scripting ;
using AnalysisControls.ViewModel ;
using Autofac ;
using Autofac.Core ;
using Autofac.Features.Metadata ;
using AvalonDock.Layout ;
using ExplorerCtrl ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;
using KayMcCormick.Lib.Wpf.ViewModel ;
#if MIGRADOC
using MigraDoc.DocumentObjectModel.Internals ;
#endif
using NLog ;
using ProjLib ;
using Logger = NLog.Logger ;

namespace ProjInterface
{
#if MSBUILDWORKSPACE
    using Microsoft.CodeAnalysis.MSBuild ;
    internal class MSBuildWorkspaceManager : IWorkspaceManager
    {
        public Workspace CreateWorkspace(IDictionary<string, string> props)
        {
           return MSBuildWorkspace.Create(props);
        }
        public Task OpenSolutionAsync(Workspace workspace, string solutionPath) {
            return ((MSBuildWorkspace)workspace).OpenSolutionAsync(solutionPath);
        }
    }
#else
#endif

    public class ProjInterfaceModule : IocModule
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
#region Overrides of Module
        protected override void Load ( ContainerBuilder builder ) { DoLoad ( builder ) ; }
#endregion

        public override void DoLoad ( [ NotNull ] ContainerBuilder builder )
        {
            builder.RegisterType < PythonControl > ( ).AsImplementedInterfaces ( ).AsSelf ( ) ;
            builder.RegisterType < PythonViewModel > ( )
                   .AsSelf ( )
                   .SingleInstance ( ) ; //.AutoActivate();
            builder.RegisterAdapter < Meta < Lazy < object > > , IPythonVariable > (
                                                                                    ( c , p , item )
                                                                                        => {
                                                                                        if ( ! item
                                                                                              .Metadata
                                                                                              .TryGetValue (
                                                                                                            "VariableName"
                                                                                                          , out
                                                                                                            var
                                                                                                                name
                                                                                                           ) )
                                                                                        {
                                                                                            item
                                                                                               .Metadata
                                                                                               .TryGetValue (
                                                                                                             "Identifier"
                                                                                                           , out
                                                                                                             name
                                                                                                            ) ;
                                                                                        }

                                                                                        var r =
                                                                                            new
                                                                                            PythonVariable
                                                                                            {
                                                                                                VariableName
                                                                                                    = (
                                                                                                        string
                                                                                                    ) name
                                                                                              , ValueLambda
                                                                                                    = ( )
                                                                                                        => item
                                                                                                          .Value
                                                                                                          .Value
                                                                                            } ;
                                                                                        return r ;
                                                                                    }
                                                                                   ) ;

            builder.RegisterBuildCallback (
                                           scope => {
                                               var py = scope.Resolve < PythonViewModel > ( ) ;
                                               if ( py is ISupportInitialize init )
                                               {
                                                   init.BeginInit ( ) ;
                                                   init.EndInit ( ) ;
                                               }
                                           }
                                          ) ;

            // builder.Register(
            //                  (c, p) => {
            //                      var listView = Func(c, p);
            //                      return new ContentControlView() { Content = listView };
            //                  })
            //        .WithMetadata("Title", "Syntax Token View")
            //        .As<IControlView>();
            // builder.RegisterType<PythonViewModel>().AsSelf();

            Logger.Trace ( "Load" ) ;
            Logger.Warn (
                         $"Loading module {typeof ( ProjInterfaceModule ).AssemblyQualifiedName}"
                        ) ;
            builder.RegisterModule < AnalysisAppLibModule > ( ) ;
            builder.RegisterModule < ProjLibModule > ( ) ;
            LogRegistration ( typeof ( Window1 ) , "AsSelf" ) ;
            builder.RegisterType < Window1 > ( ).AsSelf ( ) ;
            builder.RegisterAdapter < AppExplorerItem , IExplorerItem > (
                                                                         (
                                                                             context
                                                                           , parameters
                                                                           , item
                                                                         ) => {
                                                                             var r =
                                                                                 new
                                                                                     AdaptedExplorerItem (
                                                                                                          item
                                                                                                         ) ;
                                                                             return r ;
                                                                         }
                                                                        ) ;
            LogRegistration (
                             typeof ( AllResourcesTree )
                           , typeof ( UserControl )
                           , "AsSelf"
                           , typeof ( IViewWithTitle )
                            ) ;
            builder.RegisterType < AllResourcesTree > ( )
                   .As < UserControl > ( )
                   .AsSelf ( )
                   .As < IViewWithTitle > ( )
                   .As < IControlView > ( ) ;

            LogRegistration ( typeof ( AllResourcesTreeViewModel ) , "AsSelf" ) ;
            builder.RegisterType < AllResourcesTreeViewModel > ( ).AsSelf ( ) ;
            LogRegistration ( typeof ( IconsSource ) , typeof ( IIconsSource ) ) ;
            builder.RegisterType < IconsSource > ( ).As < IIconsSource > ( ) ;
            //   builder.RegisterType < ShellExplorerItemProvider > ( ).As < IExplorerItemProvider> ( ) ;

            builder.RegisterType < LogViewerWindow > ( ).AsSelf ( ).As < IViewWithTitle > ( ) ;
            builder.RegisterType < LogViewerControl > ( )
                   .AsSelf ( )
                   .As < IViewWithTitle > ( )
                   .As < IControlView > ( ) ;
            builder
               .RegisterAssemblyTypes (
                                       Assembly.GetCallingAssembly ( )
                                     , typeof ( PythonControl ).Assembly
                                      )
               .Where (
                       type => typeof ( IDisplayableAppCommand ).IsAssignableFrom ( type )
                               && type != typeof ( LambdaAppCommand )
                      )
               .As < IDisplayableAppCommand > ( )
               .As < IAppCommand > ( )
               .As < IDisplayable > ( ) ;
            builder
               .RegisterAdapter < Meta < Func < LayoutDocumentPane , IControlView > > ,
                    Func < LayoutDocumentPane , IDisplayableAppCommand > > (
                                                                            ( c , p , metaFunc )
                                                                                => {
                                                                                metaFunc
                                                                                   .Metadata
                                                                                   .TryGetValue (
                                                                                                 "Title"
                                                                                               , out
                                                                                                 var
                                                                                                     titleo
                                                                                                ) ;
                                                                                var title =
                                                                                    ( string )
                                                                                    titleo
                                                                                    ?? "no title" ;

                                                                                return (
                                                                                    LayoutDocumentPane
                                                                                        pane
                                                                                ) => (
                                                                                    IDisplayableAppCommand
                                                                                ) new
                                                                                    LambdaAppCommand (
                                                                                                      title
                                                                                                    , CommandFunc
                                                                                                    , Tuple
                                                                                                         .Create (
                                                                                                                  metaFunc
                                                                                                                     .Value
                                                                                                                , pane
                                                                                                                 )
                                                                                                     ) ;
                                                                            }
                                                                           )
               .As < Func < LayoutDocumentPane , IDisplayableAppCommand > > ( ) ;

            builder.Register (
                              ( context , parameters )
                                  => new LogViewerControl ( new LogViewerConfig ( 0 ) )
                             )
                   .As < IViewWithTitle > ( )
                   .As < LogViewerControl > ( ) ;
        }

        private static IAppCommandResult CommandFunc ( [ NotNull ] LambdaAppCommand command )
        {
            var (viewFunc1 , pane1) =
                ( Tuple < Func < LayoutDocumentPane , IControlView > , LayoutDocumentPane > )
                command.Argument ;

            var view = viewFunc1 ( pane1 ) ;
            var doc = new LayoutDocument { Content = view } ;
            pane1.Children.Add ( doc ) ;
            pane1.SelectedContentIndex = pane1.Children.IndexOf ( doc ) ;
            return AppCommandResult.Success ;
        }

        [ NotNull ]
        private static LambdaAppCommand LambdaAppCommandAdapter (
            Meta < Lazy < IViewWithTitle > > view
          , object                           obj = null
        )
        {
            view.Metadata.TryGetValue ( "Title" , out var title ) ;
            if ( title == null )
            {
                title = "<none>" ;
            }

            return new LambdaAppCommand (
                                         title.ToString ( )
                                       , ( command ) => {
                                             try
                                             {
                                                 if ( view.Value.Value is Window w )
                                                 {
                                                     w.Show ( ) ;
                                                 }

                                                 // else if (view is Control c)
                                                 // {
                                                 // var doc = new LayoutDocument { Content = c };
                                                 // doc.Title = view.ViewTitle;
                                                 // docpane.Children.Add(doc);
                                                 // }
                                             }
                                             catch ( Exception ex )
                                             {
                                                 Debug.WriteLine ( ex.ToString ( ) ) ;
                                             }

                                             return AppCommandResult.Failed ;
                                         }
                                       , obj
                                        ) ;
        }
    }
}