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
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Media ;
using AnalysisAppLib ;
using AnalysisControls ;
using AnalysisControls.Scripting ;
using AnalysisControls.ViewModel ;
using Autofac ;
using Autofac.Core ;
using Autofac.Features.AttributeFilters ;
using Autofac.Features.Metadata ;
using AvalonDock.Layout ;
using ExplorerCtrl ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;
using KayMcCormick.Lib.Wpf.View ;
using KayMcCormick.Lib.Wpf.ViewModel ;
using NLog ;

#if MIGRADOC
using MigraDoc.DocumentObjectModel.Internals ;
#endif

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

    public sealed class ProjInterfaceModule : IocModule
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public ProjInterfaceModule ( ) {
        }

        public override void DoLoad ( [ NotNull ] ContainerBuilder builder )
        {
            Logger.Trace (
                          $"Loading module {typeof ( ProjInterfaceModule ).AssemblyQualifiedName}"
                         ) ;
            //builder.RegisterType < PaneService > ( ) ;
            builder.RegisterType < PythonControl > ( ).AsSelf ().As<IControlView> (  ).WithMetadata(
                                                                              "ImageSource"
                                                                            , new Uri(
                                                                                      "pack://application:,,,/KayMcCormick.Lib.Wpf;component/Assets/python1.jpg"
                                                                                     )
                                                                             )
                   .WithMetadata("Ribbon", true); ;
            builder.RegisterType < PythonViewModel > ( )
                   .AsSelf ( )
                   .SingleInstance ( ) ; //.AutoActivate();
            builder.RegisterAdapter < Meta < Lazy < object > > , IPythonVariable > ( Adapter ) ;
            builder.RegisterType < EventLogView > ( ).AsSelf ( ) ;
            builder.RegisterType < EventLogViewModel > ( ) ;
            builder.RegisterBuildCallback (
                                           scope => {
                                               var py = scope.Resolve < PythonViewModel > ( ) ;
                                               if ( ! ( py is ISupportInitialize init ) )
                                               {
                                                   return ;
                                               }

                                               init.BeginInit ( ) ;
                                               init.EndInit ( ) ;
                                           }
                                          ) ;

            builder.RegisterModule < AnalysisAppLibModule > ( ) ; ;
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
            builder.RegisterType < AllResourcesTree > ( )
                   .As < UserControl > ( )
                   .AsSelf ( )
                   .As < IViewWithTitle > ( ) 
                   .As < IControlView > ( ) ;
            builder.RegisterInstance ( Application.Current ).As < IResourceResolver > ( ) ;

            builder.RegisterType < AllResourcesTreeViewModel > ( ).AsSelf ( ) ;
            builder.RegisterType < IconsSource > ( ).As < IIconsSource > ( ) ;
            //   builder.RegisterType < ShellExplorerItemProvider > ( ).As < IExplorerItemProvider> ( ) ;

            builder.RegisterType < LogViewerWindow > ( ).AsSelf ( ).As < IViewWithTitle > ( ) ;
            builder.RegisterType < LogViewerControl > ( ).AsSelf ( ).As < IViewWithTitle > ( ) ;
                   //.As < IControlView > ( ) ;

                   builder
                      .RegisterAdapter < Meta < Func < LayoutDocumentPane , IControlView > > ,
                           Func < LayoutDocumentPane , IDisplayableAppCommand >
                       > ( ControlViewCommandAdapter )
                      .As < Func < LayoutDocumentPane , IDisplayableAppCommand > > ( ) ;

            builder.RegisterAssemblyTypes (
                                           Assembly.GetCallingAssembly ( )
                                         , typeof ( PythonControl ).Assembly
                                          )
                   .Where (
                           type => {
                               var isAssignableFrom =
                                   typeof ( IDisplayableAppCommand ).IsAssignableFrom ( type )
                                   && type != typeof ( LambdaAppCommand ) ;
                               Debug.WriteLine ( $"{type.FullName} - {isAssignableFrom}" ) ;
                               return isAssignableFrom ;
                           }
                          )
                   .As < IDisplayableAppCommand > ( )
                   .As < IAppCommand > ( )
                   .As < IDisplayable > ( ) ;

            builder.Register (
                              ( context , parameters )
                                  => new LogViewerControl ( new LogViewerConfig ( 0 ) )
                             )
                   .As < IViewWithTitle > ( )
                   .As < LogViewerControl > ( ) ;
        }

        [ NotNull ]
        private Func < LayoutDocumentPane , IDisplayableAppCommand > ControlViewCommandAdapter (
            IComponentContext                                               c
          , IEnumerable < Parameter >                                       p
          , [ NotNull ] Meta < Func < LayoutDocumentPane , IControlView> >  metaFunc
        )
        {
            var r = c.Resolve < IResourceResolver > ( ) ;
            metaFunc.Metadata.TryGetValue ( "Title" ,       out var titleo ) ;
            metaFunc.Metadata.TryGetValue ( "ImageSource" , out var imageSource ) ;
            // object res = r.ResolveResource ( imageSource ) ;
            // var im = res as ImageSource ; 

            var title = ( string ) titleo ?? "no title" ;

            return pane => ( IDisplayableAppCommand ) new LambdaAppCommand (
                                                                            title
                                                                          , CommandFunc
                                                                          , Tuple.Create (
                                                                                          metaFunc
                                                                                             .Value
                                                                                        , pane
                                                                                         )
                                                                           )
                                                      {
                                                          LargeImageSourceKey = imageSource
                                                      } ;
                
        }

        [ NotNull ]
        private IPythonVariable Adapter (
            IComponentContext                    c
          , IEnumerable < Parameter >            p
          , [ NotNull ] Meta < Lazy < object > > item
        )
        {
            if ( ! item.Metadata.TryGetValue ( "VariableName" , out var name ) )
            {
                item.Metadata.TryGetValue ( "Identifier" , out name ) ;
            }

            var r = new PythonVariable
                    {
                        VariableName = ( string ) name , ValueLambda = ( ) => item.Value.Value
                    } ;
            return r ;
        }

        private static async Task < IAppCommandResult > CommandFunc ( [ NotNull ] LambdaAppCommand command )
        {
            var (viewFunc1 , pane1) =
                ( Tuple < Func < LayoutDocumentPane , IControlView > , LayoutDocumentPane > )
                command.Argument ;

            var n = DateTime.Now ;
            var view = viewFunc1 ( pane1 ) ;
            Debug.WriteLine(DateTime.Now - n);
            var doc = new LayoutDocument { Content = view } ;
            pane1.Children.Add ( doc ) ;
            pane1.SelectedContentIndex = pane1.Children.IndexOf ( doc ) ;
            return AppCommandResult.Success ;
        }

        [ NotNull ]
        private static LambdaAppCommand LambdaAppCommandAdapter (
            [ NotNull ] Meta < Lazy < IViewWithTitle > > view
          , object                                       obj = null
        )
        {
            view.Metadata.TryGetValue ( "Title" , out var title ) ;
            if ( title == null )
            {
                title = "<none>" ;
            }

            return new LambdaAppCommand (
                                         title.ToString ( )
                                       , async command => {
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

    internal interface IResourceResolver
    {
        object ResolveResource ( object resourceKey ) ;
    }
}