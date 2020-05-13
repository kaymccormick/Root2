#region header

// Kay McCormick (mccor)
// 
// Deployment
// ProjInterface
// Client2Module.cs
// 
// 2020-03-08-7:55 PM
// 
// ---

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Windows;
using AnalysisAppLib;
using AnalysisControls;
using AnalysisControls.Commands;
using AnalysisControls.RibbonM;
using AnalysisControls.ViewModel;
using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
using Autofac.Core.Registration;
using Autofac.Extras.AttributeMetadata;
using Autofac.Features.AttributeFilters;
using Autofac.Features.Metadata;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Container;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Lib.Wpf;
using NLog;

#if EXPLORER
using ExplorerCtrl ;
#endif

#if MIGRADOC
using MigraDoc.DocumentObjectModel.Internals ;
#endif

namespace Client2
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

    public sealed class Client2Module : IocModule
    {
        #region Overrides of Module

        protected override void AttachToRegistrationSource(
            IComponentRegistryBuilder componentRegistry
            , IRegistrationSource registrationSource
        )
        {
            DebugUtils.WriteLine($"!! {componentRegistry}:{registrationSource}");
        }

        private ConcurrentDictionary<Guid, MyInfo> _regs =
            new ConcurrentDictionary<Guid, MyInfo>();

        protected override void AttachToComponentRegistration(
            IComponentRegistryBuilder componentRegistry
            , IComponentRegistration registration
        )
        {
            object guidFrom = null;
            if (registration.Metadata.TryGetValue("SeenTimes", out var times))
                registration.Metadata["SeenTimes"] = (int) times + 1;
            else
                registration.Metadata["SeenTimes"] = 0;

            if (registration.Metadata.TryGetValue("RandomGuid", out var guid))
            {
                guidFrom = registration.Metadata["GuidFrom"];
            }
            else
            {
                registration.Metadata["RandomGuid"] = Guid.NewGuid();
                guidFrom = GetType().FullName;
                registration.Metadata["GuidFrom"] = guidFrom;
            }

            registration.Preparing += (sender, args) => {DebugUtils.WriteLine($"{args.Component.Activator.LimitType.ToString()}"); };
            registration.Activating += (sender, args) => { };
            registration.Activated += (sender, args) => { };
            var registrationActivator = registration.Activator;
            var limitType = registrationActivator.LimitType;
            DebugUtils.WriteLine($"LimitType = {limitType}");
        }

        private void OnComponentRegistryOnRegistered(
            object sender
            , ComponentRegisteredEventArgs args
        )
        {
            DebugUtils.WriteLine(
                $"{sender} Logging reg {args.ComponentRegistration} ({args.ComponentRegistration.Lifetime})"
            );
            regs.OnNext(args.ComponentRegistration);
        }

        #endregion


        private Subject<IComponentRegistration>
            regs = new Subject<IComponentRegistration>();

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override void DoLoad([NotNull] ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AssignableTo<IControlView>().WithAttributedMetadata().AsSelf()
                .AsImplementedInterfaces().WithCallerMetadata();

            builder.RegisterInstance(regs)
                .AsSelf()
                .As<IObservable<IComponentRegistration>>();

            Logger.Trace(
                $"Loading module {typeof(Client2Module).AssemblyQualifiedName}"
            );
#if PYTHON
            builder.RegisterType<PythonControl>().AsSelf().As<IControlView>().WithMetadata(
                    "ImageSource"
                    , new Uri(
                        "pack://application:,,,/WpfLib;component/Assets/python1.jpg"
                    )
                )
                .WithMetadata("Ribbon", true);
            ;
            if (RegisterPython)
            {
                builder.RegisterType<PythonViewModel>()
                    .AsSelf()
                    .SingleInstance(); //.AutoActivate();
                builder.RegisterBuildCallback(
                    scope =>
                    {
                        var py = scope.Resolve<PythonViewModel>();
                        if (!(py is ISupportInitialize init)) return;

                        init.BeginInit();
                        init.EndInit();
                    }
                );
            }
#endif
            builder.RegisterModule<AnalysisAppLibModule>();


            builder.Register((c) =>
                    new Client2Window1(c.Resolve<ILifetimeScope>(), c.Resolve<ClientModel>(),
                        c.ResolveOptional<MyCacheTarget2>()))
                .As<Window>().WithCallerMetadata();
            
            builder.Register(Client2Window1.RibbonModelBuilder);
            builder.RegisterType<DummyResourceAdder>().AsImplementedInterfaces();
            builder.RegisterType<ClientModel>().AsSelf();
            builder.RegisterType<RibbonModelApplicationMenu>();
            builder.RegisterType<RibbonTabProvider1>().As<IRibbonModelProvider<RibbonModelTab>>().SingleInstance();
            builder.RegisterType<RibbonViewGroupProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<RibbonViewGroupProviderBaseImpl>().AsImplementedInterfaces().WithCallerMetadata().SingleInstance();
            builder.RegisterType<RibbonViewGroupProviderBaseImpl2>().AsImplementedInterfaces().WithCallerMetadata().SingleInstance();
            builder.RegisterType<TestRibbonTabDef>().As<RibbonModelTab>().SingleInstance().WithAttributeFiltering();
            builder.RegisterType<TestRibbonTabDef2>().As<RibbonModelTab>().SingleInstance();
            builder.RegisterType<TestRibbonTabDef3>().As<RibbonModelTab>().SingleInstance();
            builder.RegisterType<TestRibbonTabDef4>().As<RibbonModelTab>().SingleInstance();
            builder.RegisterType<RibbonModelGroup1>().As<RibbonModelGroup>().SingleInstance();
            builder.RegisterType<RibbonModelGroupTest1>().As<RibbonModelGroup>().SingleInstance();
            builder.RegisterType<RibbonModelGroupTest2>().As<RibbonModelGroup>().SingleInstance();
            builder.RegisterType<ContextualTabGroup1>().As<RibbonModelContextualTabGroup>().SingleInstance();
            builder.RegisterType<CodeGenCommand>().AsImplementedInterfaces();
            builder.RegisterType<DatabasePopulateCommand>().AsImplementedInterfaces();
            builder.RegisterType<OpenFileCommand>().AsImplementedInterfaces();
        }

        public bool RegisterPython { get; set; }

#pragma warning disable 1998
    }

    public class x : IConstructorSelector
    {
        public ConstructorParameterBinding SelectConstructorBinding(ConstructorParameterBinding[] constructorBindings,
            IEnumerable<Parameter> parameters)
        {
            return constructorBindings
                .Where(x => x.TargetConstructor.GetParameters().Any(info => info.ParameterType == typeof(ClientModel)))
                .OrderByDescending(x => x.TargetConstructor.GetParameters().Length).FirstOrDefault();
        }

    }

    public class DummyResourceAdder : IAddRuntimeResource
    {
        public void AddResource(ResourceNodeInfo node)
        {
            
        }
    }
}