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
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AnalysisAppLib;
using AnalysisAppLib.Syntax;
using AnalysisControls;
using AnalysisControls.Commands;
using AnalysisControls.RibbonM;
using AnalysisControls.Scripting;
using AnalysisControls.ViewModel;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Features.AttributeFilters;
using Autofac.Features.Metadata;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Command;
using KayMcCormick.Dev.Container;
using KayMcCormick.Dev.Metadata;
using KayMcCormick.Lib.Wpf;
using KayMcCormick.Lib.Wpf.Command;
using KayMcCormick.Lib.Wpf.View;
using KayMcCormick.Lib.Wpf.ViewModel;
using Microsoft.Extensions.Logging;
using NLog;
using ProjInterface;

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
            DebugUtils.WriteLine($"{componentRegistry}:{registrationSource}");
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

            registration.Preparing += (sender, args) => { };
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
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AssignableTo<IControlView>().AsSelf()
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
#endif
            builder.RegisterModule<AnalysisAppLibModule>();


            builder.RegisterType<Client2Window1>().AsSelf().As<Window>();
            builder.Register((c, o) =>
            {
                var r = new RibbonModel();
                r.AppMenu = c.Resolve<RibbonModelApplicationMenu>();
                r.ContextualTabGroups.Add(new RibbonModelContextualTabGroup() { Header = "Assemblies" });
		r.AppMenu.Items.Add(new RibbonModelAppMenuItem{Header="test"});
                var tabs = c.Resolve<IEnumerable<Meta<Lazy<RibbonModelTab>>>>();
                foreach (var meta in tabs)
                {
                    var ribbonModelTab = meta.Value.Value;
                    r.RibbonItems.Add(ribbonModelTab);
                }
                var tabProviders = c.Resolve<IEnumerable<IRibbonModelProvider<RibbonModelTab>>>();
                foreach (var ribbonModelProvider in tabProviders)
                {
                    var item = ribbonModelProvider.ProvideModelItem(c);
                    r.RibbonItems.Add(item);
                }

                return r;
            });
            builder.RegisterType<ClientModel>().AsSelf();
            builder.RegisterType<RibbonModelApplicationMenu>();
            builder.RegisterType<RibbonTabProvider1>().As<IRibbonModelProvider<RibbonModelTab>>().SingleInstance();
            builder.RegisterType<RibbonViewGroupProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<RibbonViewGroupProviderBaseImpl>().AsImplementedInterfaces().WithCallerMetadata().SingleInstance();
            builder.RegisterType<RibbonViewGroupProviderBaseImpl2>().AsImplementedInterfaces().WithCallerMetadata().SingleInstance();
            builder.RegisterType<TestRibbonTabDef>().As<RibbonModelTab>().SingleInstance().WithAttributeFiltering();
            builder.RegisterType<TestRibbonTabDef2>().As<RibbonModelTab>().SingleInstance();
            builder.RegisterType<TestRibbonTabDef3>().As<RibbonModelTab>().SingleInstance();
            builder.RegisterType<RibbonModelGroup1>().As<RibbonModelGroup>().SingleInstance();
            builder.RegisterType<RibbonModelGroupTest1>().As<RibbonModelGroup>().SingleInstance();
            builder.RegisterType<RibbonModelGroupTest2>().As<RibbonModelGroup>().SingleInstance();
            builder.RegisterType<CodeGenCommand>().AsImplementedInterfaces();
            builder.RegisterType<DatabasePopulateCommand>().AsImplementedInterfaces();
            builder.RegisterType<OpenFileCommand>().AsImplementedInterfaces();
        }

#pragma warning disable 1998
    }
}