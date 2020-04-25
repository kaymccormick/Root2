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
using System.Linq ;
using System.Reflection ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Input ;
using AnalysisAppLib ;
using AnalysisControls ;
using Autofac ;
using Autofac.Core ;
using Autofac.Core.Activators.Delegate ;
using Autofac.Core.Lifetime ;
using Autofac.Core.Registration ;
using Autofac.Features.Metadata ;
using AvalonDock.Layout ;
#if EXPLORER
using ExplorerCtrl ;
#endif
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Command ;
using KayMcCormick.Dev.Container ;
using KayMcCormick.Lib.Wpf ;
using KayMcCormick.Lib.Wpf.Command ;
using KayMcCormick.Lib.Wpf.View ;
using KayMcCormick.Lib.Wpf.ViewModel ;
using Microsoft.Extensions.Logging ;
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
        private static readonly Logger Logger =
            LogManager.GetCurrentClassLogger ( ) ;

        private bool _registerControlViewCommandAdapters = true ;

        public override void DoLoad ( [ NotNull ] ContainerBuilder builder )
        {
            builder.RegisterType < MiscInstanceInfoProvider > ( )
                   .AsSelf ( )
                   .As < TypeDescriptionProvider > ( )
                   .WithCallerMetadata ( )
                   .OnActivating (
                                  args => {
                                      foreach ( var type in new[]
                                                            {
                                                                typeof ( IComponentRegistration )
                                                              , typeof ( ComponentRegistration )
                                                            } )
                                      {
                                          TypeDescriptor.AddProvider ( args.Instance , type ) ;
                                      }
                                  }
                                 ) ;

            builder.RegisterType < Myw > ( ).As < ILoggerProvider > ( ) ;
            builder.RegisterSource < MySource > ( ) ;
            Logger.Trace (
                          $"Loading module {typeof ( ProjInterfaceModule ).AssemblyQualifiedName}"
                         ) ;
#if PYTHON
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
#endif

            builder.RegisterType < ResourcesTreeView > ( )
                   .AsSelf ( )
                   .AsImplementedInterfaces ( )
                   .WithCallerMetadata ( ) ;

            builder.RegisterType < EventLogView > ( ).AsSelf ( ).WithCallerMetadata ( ) ;
            builder.RegisterType < EventLogViewModel > ( ).WithCallerMetadata ( ) ;
#if PYTHON
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
#endif
            builder.RegisterModule < AnalysisAppLibModule > ( ) ;
            builder.RegisterType < Window1 > ( ).AsSelf ( ).WithCallerMetadata ( ) ;
#if EXPLORER
if(RegiserExplorerTypes){
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
}
#endif
            builder.RegisterType < AllResourcesTree > ( )
                   .As < UserControl > ( )
                   .AsSelf ( )
                   .As < IViewWithTitle > ( )
                   .As < IControlView > ( )
                   .WithCallerMetadata ( ) ;
            builder.RegisterType < AllResourcesView > ( )
                   .As < UserControl > ( )
                   .AsSelf ( )
                   .As < IViewWithTitle > ( )
                   .As < IControlView > ( )
                   .WithMetadata < IViewMetadata > (
                                                    m => {
                                                        m.For (
                                                               am => am.Title
                                                             , "Resources View"
                                                              ) ;
                                                        m.For (
                                                               am => am.Description
                                                             , "View resources in application"
                                                              ) ;
                                                    }
                                                   )
                   .WithCallerMetadata ( ) ;
            builder.RegisterType < DockWindowViewModel > ( ).AsSelf ( ).WithCallerMetadata ( ) ;
            builder.RegisterType < WorkspaceControl > ( )
                   .As < IViewWithTitle > ( )
                   .As < IControlView > ( )
                   .WithCallerMetadata ( ) ;
            builder.RegisterType < WorkspaceViewModel > ( ).WithCallerMetadata ( ) ;
            builder.RegisterInstance ( Application.Current )
                   .As < IResourceResolver > ( )
                   .WithCallerMetadata ( ) ;

            builder.RegisterType < AllResourcesTreeViewModel > ( )
                   .AsSelf ( )
                   .As < IAddRuntimeResource > ( )
                   .SingleInstance ( )
                   .WithCallerMetadata ( ) ;
            builder.RegisterType < IconsSource > ( )
                   .As < IIconsSource > ( )
                   .WithCallerMetadata ( ) ;

            builder.RegisterType < LogViewerWindow > ( )
                   .AsSelf ( )
                   .As < IViewWithTitle > ( )
                   .WithCallerMetadata ( ) ;
            builder.RegisterType < LogViewerControl > ( )
                   .AsSelf ( )
                   .As < IViewWithTitle > ( )
                   .WithCallerMetadata ( ) ;

            builder.RegisterType < UiElementTypeConverter > ( ).AsSelf ( ) ;
            if ( RegisterControlViewCommandAdapters )
            {
                builder
                   .RegisterAdapter < Meta < Func < LayoutDocumentPane , IControlView > > ,
                        Func < LayoutDocumentPane , IDisplayableAppCommand >
                    > ( ControlViewCommandAdapter )
                   .As < Func < LayoutDocumentPane , IDisplayableAppCommand > > ( )
                   .WithCallerMetadata ( ) ;
            }

#if PYTHON
            builder.RegisterAssemblyTypes (
                                           Assembly.GetCallingAssembly ( )
                                 , typeof ( PythonControl ).Assembly
                                          )
                   .Where (
                           type => {
                               var isAssignableFrom =
                                   typeof ( IDisplayableAppCommand ).IsAssignableFrom ( type )
                                   && type != typeof ( LambdaAppCommand ) ;
                               DebugUtils.WriteLine ( $"{type.FullName} - {isAssignableFrom}" ) ;
                               return isAssignableFrom ;
                           }
                          )
                   .As < IDisplayableAppCommand > ( )
                   .As < IAppCommand > ( )
                   .As < IDisplayable > ( ) ;
#endif

            // builder.Register (
            // ( context , parameters )
            // => new LogViewerControl ( new LogViewerConfig ( 0 ) )
            // )
            // .As < IViewWithTitle > ( )
            // .As < LogViewerControl > ( )
            // .WithCallerMetadata ( ) ;
        }

        public bool RegisterControlViewCommandAdapters
        {
            get { return _registerControlViewCommandAdapters ; }
            set { _registerControlViewCommandAdapters = value ; }
        }

        [ NotNull ]
        public static Func < LayoutDocumentPane , IDisplayableAppCommand >
            ControlViewCommandAdapter (
                [ NotNull ] IComponentContext                                   c
              , IEnumerable < Parameter >                                       p
              , [ NotNull ] Meta < Func < LayoutDocumentPane , IControlView > > metaFunc
            )
        {
            c.Resolve < IResourceResolver > ( ) ;
            metaFunc.Metadata.TryGetValue ( "Title" ,       out var titleo ) ;
            metaFunc.Metadata.TryGetValue ( "ImageSource" , out var imageSource ) ;
            // object res = r.ResolveResource ( imageSource ) ;
            // var im = res as ImageSource ;

            var title = ( string ) titleo ?? "no title" ;

            return pane => ( IDisplayableAppCommand ) new LambdaAppCommand (
                                                                            title
                                                                          , CommandFuncAsync
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
#if PYTHON
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
#endif
#pragma warning disable 1998
        public static async Task < IAppCommandResult > CommandFuncAsync (
#pragma warning restore 1998
            [ NotNull ] LambdaAppCommand command
        )
        {
            //await JoinableTaskFactory.SwitchToMainThreadAsync (  ) ;
            var (viewFunc1 , pane1) =
                ( Tuple < Func < LayoutDocumentPane , IControlView > , LayoutDocumentPane > )
                command.Argument ;

            DebugUtils.WriteLine ( $"Calling viewfunc ({command})" ) ;
            var n = DateTime.Now ;
            var view = viewFunc1 ( pane1 ) ;
            DebugUtils.WriteLine ( ( DateTime.Now - n ).ToString ( ) ) ;
            var doc = new LayoutDocument { Content = view } ;
            pane1.Children.Add ( doc ) ;
            pane1.SelectedContentIndex = pane1.Children.IndexOf ( doc ) ;
            DebugUtils.WriteLine ( "returning success" ) ;
            return AppCommandResult.Success ;
        }

        [ NotNull ]
        // ReSharper disable once UnusedMember.Local
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
#pragma warning disable 1998
                                       , async command => {
#pragma warning restore 1998
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
                                                 DebugUtils.WriteLine ( ex.ToString ( ) ) ;
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

    public sealed class MySource : IRegistrationSource
    {
        public MySource ( )
        {
            _commands = new List < CommandInfo > ( ) ;
            foreach ( var cmd in typeof ( WpfAppCommands )
                                .GetFields ( BindingFlags.Public | BindingFlags.Static )
                                .Select ( fieldInfo => fieldInfo.GetValue ( null ) ) )
            {
                if ( cmd is RoutedUICommand ri )
                {
                    _commands.Add ( new CommandInfo { Command = ri } ) ;
                }
            }
        }

#pragma warning disable 649
        private bool _isAdapterForIndividualComponents ;
#pragma warning restore 649
        private readonly List < CommandInfo > _commands ;
        #region Implementation of IRegistrationSource
        // ReSharper disable once AnnotateNotNullTypeMember
        public IEnumerable < IComponentRegistration > RegistrationsFor (
            Service                                                   service
          , Func < Service , IEnumerable < IComponentRegistration > > registrationAccessor
        )
        {
            DebugUtils.WriteLine ( $"{service}" ) ;
            if ( ! ( service is IServiceWithType swt )
                 || swt.ServiceType != typeof ( RoutedUICommand ) )
            {
                return Enumerable.Empty < IComponentRegistration > ( ) ;
            }

            var reg = new ComponentRegistration (
                                                 Guid.NewGuid ( )
                                               , new DelegateActivator (
                                                                        swt.ServiceType
                                                                      , ( c , p ) => _commands[ 0 ]
                                                                       )
                                               , new CurrentScopeLifetime ( )
                                               , InstanceSharing.None
                                               , InstanceOwnership.OwnedByLifetimeScope
                                               , new[] { service }
                                               , new Dictionary < string , object > ( )
                                                ) ;
            return new IComponentRegistration[] { reg } ;
        }

        public bool IsAdapterForIndividualComponents
        {
            get { return _isAdapterForIndividualComponents ; }
        }
        #endregion
    }

    public sealed class CommandInfo
    {
        private RoutedUICommand _command ;
        public  RoutedUICommand Command { get { return _command ; } set { _command = value ; } }
    }
}