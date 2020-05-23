using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using AnalysisAppLib;
using AnalysisControls.Commands;
using AnalysisControls.Converters;

using AnalysisControls.RibbonModel;
using AnalysisControls.RibbonModel.Definition;
using AnalysisControls.ViewModel;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Extras.AttributeMetadata;
using Autofac.Features.AttributeFilters;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Container;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Lib.Wpf;
using KayMcCormick.Lib.Wpf.Command;
using NLog;

namespace AnalysisControls
{
    public sealed class Client2Module1 : IocModule
    {
        #region Overrides of Module

        protected override void AttachToRegistrationSource(
            IComponentRegistryBuilder componentRegistry
            , IRegistrationSource registrationSource
        )
        {
            DebugUtils.WriteLine($"!! {componentRegistry}:{registrationSource}");
        }

        
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

            registration.Preparing += (sender, args) =>
            {
                // DebugUtils.WriteLine($"{args.Component.Activator.LimitType}");
            };
            registration.Activating += (sender, args) => { };
            registration.Activated += (sender, args) => { };
            var registrationActivator = registration.Activator;
            var limitType = registrationActivator.LimitType;
            // DebugUtils.WriteLine($"LimitType = {limitType}");
        }

        private void OnComponentRegistryOnRegistered(
            object sender
            , ComponentRegisteredEventArgs args
        )
        {
            DebugUtils.WriteLine(
                $"{sender} Logging reg {args.ComponentRegistration} ({args.ComponentRegistration.Lifetime})"
            );
            _regSubject.OnNext(args.ComponentRegistration);
        }

        #endregion


        private readonly Subject<IComponentRegistration>
            _regSubject = new Subject<IComponentRegistration>();

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override void DoLoad([NotNull] ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AssignableTo<IControlView>()
                .WithAttributedMetadata().AsSelf()
                .AsImplementedInterfaces().WithCallerMetadata();

            builder.RegisterInstance<Subject<IComponentRegistration>>(_regSubject)
                .AsSelf()
                .As<IObservable<IComponentRegistration>>();

            Logger.Trace(
                $"Loading module {typeof(Client2Module1).AssemblyQualifiedName}"
            );
#if PYTHON
            builder.RegisterType<PythonControl>().AsSelf().As<IControlView>().WithMetadata(
                    "ImageSource"
                    , new Uri(
                        "pack://application:,,,/WpfLib;component/Assets/python1.jpg"
                    )
                )
                .WithMetadata("PrimaryRibbon", true);
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

      builder.Register((c, o) => RibbonBuilder1.RibbonModelBuilder(c.Resolve<RibbonModelApplicationMenu>(),
                c.Resolve<IEnumerable<RibbonModelContextualTabGroup>>(), c.Resolve<IEnumerable<RibbonModelTab>>(),
                c.Resolve<IEnumerable<IRibbonModelProvider<RibbonModelTab>>>(),
                
                c.Resolve<IEnumerable<IRibbonModelProvider<RibbonModelContextualTabGroup>>>(),
                c.Resolve<JsonSerializerOptions>()));
            builder.RegisterType<DummyResourceAdder>().AsImplementedInterfaces();
            builder.RegisterType<ClientModel>().AsSelf().SingleInstance().AsImplementedInterfaces()
                .WithCallerMetadata();
            builder.RegisterType<RibbonModelApplicationMenu>();
            builder.RegisterType<FunTabProvider>().As<IRibbonModelProvider<RibbonModelTab>>().SingleInstance()
                .WithAttributeFiltering();
            builder.RegisterType<CodeTab1>().As<IRibbonModelProvider<RibbonModelTab>>().SingleInstance()
                .WithAttributeFiltering();

            builder.RegisterType<RibbonViewGroupProviderBaseImpl>().AsImplementedInterfaces().WithCallerMetadata()
                .SingleInstance().WithAttributeFiltering();
            builder.RegisterType<SuperGroup>().AsImplementedInterfaces().WithCallerMetadata().SingleInstance()
                .WithAttributeFiltering();
         builder.RegisterType<CodeAnalysisContextualTabGroupProvider>().AsImplementedInterfaces()
                .WithCallerMetadata(); builder.RegisterType<AssembliesTypesGroup>().As<RibbonModelGroup>().SingleInstance()
                .WithAttributeFiltering();
            builder.RegisterType<DisplayableAppCommandGroup>().As<RibbonModelGroup>().SingleInstance()
                .WithAttributeFiltering();
            builder.RegisterType<BaseLibCommandGroup>().As<RibbonModelGroup>().SingleInstance()
                .WithAttributeFiltering();
            builder.RegisterType<CodeGenCommand>().AsImplementedInterfaces().WithAttributeFiltering();
            builder.RegisterType<DatabasePopulateCommand>().AsImplementedInterfaces().WithAttributeFiltering();
            builder.Register((c) =>
            {
                return new LambdaAppCommand("Extract docs", l => ExtractDocCommentsCommand.ProcessSolutionAsync(l, ""),
                    null);

            }).AsImplementedInterfaces().WithMetadata("Title", "Extract docs");
            builder.RegisterType<OpenFileCommand>().AsImplementedInterfaces().WithAttributeFiltering();
            builder.RegisterType<AppCommandTypeConverter>().AsSelf();
            builder.RegisterType<ObjectStringTypeConverter>().AsSelf();
        }

        public bool RegisterPython { get; set; }

#pragma warning disable 1998
    }
}