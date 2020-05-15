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
using System.Collections;
using System.Collections.Concurrent ;
using System.Collections.Generic ;
using System.Collections.ObjectModel;
using System.ComponentModel ;
using System.Linq ;
using System.Reactive.Subjects ;
using System.Reflection ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Controls.Ribbon;
using System.Windows.Input ;
using AnalysisAppLib ;
using AnalysisAppLib.Syntax ;
using AnalysisControls ;
using AnalysisControls.Scripting;
using AnalysisControls.ViewModel;
using Autofac ;
using Autofac.Core ;
using Autofac.Core.Activators.Delegate ;
using Autofac.Core.Activators.ProvidedInstance ;
using Autofac.Core.Activators.Reflection ;
using Autofac.Core.Lifetime ;
using Autofac.Core.Registration ;
using Autofac.Features.Metadata ;
using Autofac.Features.ResolveAnything ;
using Autofac.Features.Variance ;
using AvalonDock.Layout ;
#if EXPLORER
using ExplorerCtrl ;
#endif
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Command ;
using KayMcCormick.Dev.Container ;
using KayMcCormick.Dev.Metadata ;
using KayMcCormick.Lib.Wpf ;
using KayMcCormick.Lib.Wpf.Command ;

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
        #region Overrides of Module
        protected override void AttachToRegistrationSource (
            IComponentRegistryBuilder componentRegistry
          , IRegistrationSource       registrationSource
        )
        {
            DebugUtils.WriteLine ( $"{componentRegistry}:{registrationSource}" ) ;
        }

        private ConcurrentDictionary < Guid , MyInfo > _regs =
            new ConcurrentDictionary < Guid , MyInfo > ( ) ;

        protected override void AttachToComponentRegistration (
            IComponentRegistryBuilder componentRegistry
          , IComponentRegistration    registration
        )
        {
            object guidFrom = null ;
            if ( registration.Metadata.TryGetValue ( "SeenTimes" , out var times ) )
            {
                registration.Metadata[ "SeenTimes" ] = ( int ) times + 1 ;
            }
            else
            {
                registration.Metadata[ "SeenTimes" ] = 0 ;
            }

            if ( registration.Metadata.TryGetValue ( "RandomGuid" , out var guid ) )
            {
                guidFrom = registration.Metadata[ "GuidFrom" ] ;
            }
            else
            {
                registration.Metadata[ "RandomGuid" ] = Guid.NewGuid ( ) ;
                guidFrom                              = GetType ( ).FullName ;
                registration.Metadata[ "GuidFrom" ]   = guidFrom ;
            }

            registration.Preparing += ( sender , args ) => {
            } ;
            registration.Activating += ( sender , args ) => {
            } ;
            registration.Activated += ( sender , args ) => {
            } ;
            var registrationActivator = registration.Activator ;
            var limitType = registrationActivator.LimitType ;
            DebugUtils.WriteLine ( $"LimitType = {limitType}" ) ;
            if ( _activators.TryGetValue ( limitType , out var info2 ) )
            {
            }
            else
            {
                info2 = new MyInfo2 ( ) { LimitType = limitType } ;
                _activators.TryAdd ( limitType , info2 ) ;
            }

            info2.Registrations.Add ( registration ) ;
            if ( ! info2.Lifetimes.TryGetValue ( registration.Lifetime , out var info3 ) )
            {
                info3 = new MyInfo3 { LimitType = limitType , Lifetime = registration.Lifetime } ;
                info2.Lifetimes.TryAdd ( registration.Lifetime , info3 ) ;
            }

            info3.Registrations.Add ( registration ) ;
            info3.Ids.Add ( registration.Id ) ;
            DebugUtils.WriteLine ( $"{registration.Id} {limitType.FullName}" ) ;
            if ( _regs.TryGetValue ( registration.Id , out var info ) )
            {
            }
            else
            {
                info = new MyInfo ( ) { Id = registration.Id } ;
                _regs.TryAdd ( registration.Id , info ) ;
            }

            info.Registrations.Add ( registration ) ;
            if ( info.Registrations.Count > 1 )
            {
                DebugUtils.WriteLine (
                                      string.Join (
                                                   ", "
                                                 , info.Registrations.Select (
                                                                              x => x
                                                                                  .Lifetime
                                                                                  .ToString ( )
                                                                             )
                                                  )
                                     ) ;
            }
        }

        private void OnComponentRegistryOnRegistered (
            object                       sender
          , ComponentRegisteredEventArgs args
        )
        {
            DebugUtils.WriteLine (
                                  $"{sender} Logging reg {args.ComponentRegistration} ({args.ComponentRegistration.Lifetime})"
                                 ) ;
            regs.OnNext ( args.ComponentRegistration ) ;
        }
        #endregion


        private Subject < IComponentRegistration >
            regs = new Subject < IComponentRegistration > ( ) ;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private bool _registerControlViewCommandAdapters = true ;

        private ConcurrentDictionary < Type , MyInfo2 > _activators =
            new ConcurrentDictionary < Type , MyInfo2 > ( ) ;

        public override void DoLoad ( [ NotNull ] ContainerBuilder builder )
        {
            builder.RegisterType<RibbonBuilder>();
            builder.ComponentRegistryBuilder.Registered += ComponentRegistryBuilderOnRegistered ;
            builder.RegisterInstance ( _activators ) ;
            builder.Register < AppTypeInfoObservableCollection > (
                                                                  ( c , p )
                                                                      => new AppTypeInfoObservableCollection(
                                                                                                    c.Resolve
                                                                                                    < IEnumerable
                                                                                                        < AppTypeInfo
                                                                                                        > > ( )
                                                                                                   )
                                                                 ) ;
            builder.Register<IEnumerable<GroupInfo>>((c, p) =>
            {
                var t = p.TypedAs<TabInfo>();
                var views = c.Resolve<IEnumerable<Meta<Lazy<IControlView>>>>();
                var groupInfo2 = new GroupInfo2() {Group="Views"};
                foreach (var view in views)
                {
                    foreach (var keyValuePair in view.Metadata)
                    {
                        DebugUtils.WriteLine($"{keyValuePair.Key} = {keyValuePair.Value}");
                    }

                    view.Metadata.TryGetValue("Title", out var title);
                    groupInfo2.Items.Add(new RibbonItemInfo() {Content = view, Title=(string) title});
                }
                
                return new[] {groupInfo2};
            });
            builder.RegisterInstance ( regs )
                   .AsSelf ( )
                   .As < IObservable < IComponentRegistration > > ( ) ;
        

            builder.RegisterType < Myw > ( ).As < ILoggerProvider > ( ) ;
            //builder.RegisterSource < MySource > ( ) ;
            Logger.Trace (
                          $"Loading module {typeof ( ProjInterfaceModule ).AssemblyQualifiedName}"
                         ) ;
#if PYTHON
            builder.RegisterType < PythonControl > ( ).AsSelf ().As<IControlView> (  ).WithMetadata(
                                                                              "ImageSource"
                                                                , new Uri(
                                                                                      "pack://application:,,,/WpfLib;component/Assets/python1.jpg"
                                                                                     )
                                                                             )
                   .WithMetadata("PrimaryRibbon", true); ;
            builder.RegisterType < PythonViewModel > ( )
                   .AsSelf ( )
                   .SingleInstance ( ) ; //.AutoActivate();
                        builder.RegisterAdapter < Meta < Lazy < object > > , IPythonVariable > ( Adapter ) ;
#endif

            builder.RegisterType < ResourcesTreeView > ( )
                   .AsSelf ( )
                   .AsImplementedInterfaces ( )
                   .WithCallerMetadata ( ) ;

            // builder.RegisterType < EventLogView > ( ).AsSelf ( ).WithCallerMetadata ( ) ;
            // builder.RegisterType < EventLogViewModel > ( ).WithCallerMetadata ( ) ;
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

            builder.RegisterType<Window1>().AsSelf().As<Window>().WithCallerMetadata().OnActivating(OnWindowActivating);
            builder.RegisterType<Window2>().AsSelf().As<Window>().WithCallerMetadata().OnActivating(OnWindowActivating);
            //builder.RegisterType<Window2>().AsSelf().As<Window>().WithCallerMetadata().OnActivating(OnWindowActivating);
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

            // builder.RegisterAdapter<Meta<Lazy<IViewModel>>, IDisplayableAppCommand>((c, p, o) =>
            // {
                // var meta = o.Metadata;
                
            // })
            


#if PYTHON
#endif

            // builder.Register (
            // ( context , parameters )
            // => new LogViewerControl ( new LogViewerConfig ( 0 ) )
            // )
            // .As < IViewWithTitle > ( )
            // .As < LogViewerControl > ( )
            // .WithCallerMetadata ( ) ;
        }

        private void OnWindowActivating<T>(IActivatingEventArgs<T> obj) where T : Window
        {
            //obj.Instance.SetValue(AttachedProperties.LifetimeScopeProperty, obj.Context.Resolve<ILifetimeScope>());
        }

        private void ComponentRegistryBuilderOnRegistered (
            object                       sender
          , ComponentRegisteredEventArgs e
        )
        {
            DebugUtils.WriteLine (
                                  $"{e.ComponentRegistration.Id}:{e.ComponentRegistryBuilder}:{e.ComponentRegistration}"
                                 ) ;
        }

        public bool RegisterControlViewCommandAdapters
        {
            get { return _registerControlViewCommandAdapters ; }
            set { _registerControlViewCommandAdapters = value ; }
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

    internal class MyInfo
    {
        public Guid Id { get; internal set; }
        public List<IComponentRegistration> Registrations { get; internal set; } = new List<IComponentRegistration>();
    }

    internal interface IResourceResolver
    {
        object ResolveResource ( object resourceKey ) ;
    }

    public sealed class MySource : IRegistrationSource
    {
        public MySource ( )
        {
            _commands = new List < CommandInfo2 > ( ) ;
            foreach ( var cmd in typeof ( WpfAppCommands )
                                .GetFields ( BindingFlags.Public | BindingFlags.Static )
                                .Select ( fieldInfo => fieldInfo.GetValue ( null ) ) )
            {
                if ( cmd is RoutedUICommand ri )
                {
                    _commands.Add ( new CommandInfo2 { Command = ri } ) ;
                }
            }
        }

#pragma warning disable 649
        private bool _isAdapterForIndividualComponents ;
#pragma warning restore 649
        private readonly List < CommandInfo2 > _commands ;
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

    public sealed class CommandInfo2
    {
        private RoutedUICommand _command ;
        public  RoutedUICommand Command { get { return _command ; } set { _command = value ; } }
    }


    public class GroupInfo2 : GroupInfo
    {
        public ObservableCollection<RibbonItemInfo> Items { get; } = new ObservableCollection<RibbonItemInfo>();

        public GroupInfo2()
        {
        }
    }

    public class RibbonItemInfo
    {
        public bool? Checked { get; set; } 
        public Meta<Lazy<IControlView>> Content { get; set; }
        public string Title { get; set; }
    }
}