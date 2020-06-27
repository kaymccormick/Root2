using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using AnalysisAppLib;
using AnalysisAppLib.ViewModel;
using AnalysisControls.Commands;
using AnalysisControls.Converters;
using AnalysisControls.Ribbon2;
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
using RibbonLib.Model;

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

            builder.RegisterInstance(_regSubject)
                .AsSelf()
                .As<IObservable<IComponentRegistration>>().WithCallerMetadata();

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
            builder.RegisterType<RibbonModelTab>().AsSelf().AsImplementedInterfaces().WithCallerMetadata();
            builder.RegisterType<RibbonBuilder1>().WithAttributeFiltering().WithCallerMetadata();
            builder.Register((c, o) => c.Resolve<RibbonBuilder1>().BuildRibbon()).WithCallerMetadata();
            builder.RegisterType<DummyResourceAdder>().AsImplementedInterfaces().WithCallerMetadata();
            builder.RegisterType<ClientModel>().AsSelf().AsImplementedInterfaces()
                .WithCallerMetadata();
            builder.RegisterType<RibbonModelApplicationMenu>().WithCallerMetadata();
            // builder.RegisterType<FunTabProvider>().As<IRibbonModelProvider<RibbonModelTab>>().WithCallerMetadata()//.SingleInstance()
                // .WithAttributeFiltering();
            // builder.RegisterType<CodeTab1>().As<IRibbonModelProvider<RibbonModelTab>>().WithCallerMetadata()//.SingleInstance()
                // .WithAttributeFiltering();
            // builder.RegisterType<CodeTab2>().As<IRibbonModelProvider<RibbonModelTab>>().WithCallerMetadata()//.SingleInstance()
                // .WithAttributeFiltering();
            // builder.RegisterType<NavigationTabProvider>().AsImplementedInterfaces()
                // .WithCallerMetadata();

            // builder.RegisterType<CodeAnalysisContextualTabGroupProvider>().AsImplementedInterfaces()
                // .WithCallerMetadata();
            builder.RegisterType<CodeGenCommand>().AsImplementedInterfaces().WithAttributeFiltering().WithCallerMetadata();
            builder.RegisterType<DatabasePopulateCommand>().AsImplementedInterfaces().WithAttributeFiltering().WithCallerMetadata();
#if true
            builder.RegisterType<ExtractDocCommentsCommand>().WithCallerMetadata();
            builder.Register((c) =>
            {
                var zz = c.Resolve<ExtractDocCommentsCommand>();
                    return new OpenFileCommand2(new LambdaAppCommand("Extract docs",
                    async (l, arg) => await zz.ProcessSolutionAsync(l, (string) arg).ConfigureAwait(false),
                    null));

            }).AsSelf().AsImplementedInterfaces().WithMetadata("Title", "Extract docs").WithCallerMetadata();
            // builder.RegisterType<OpenFileCommand>().AsImplementedInterfaces().WithAttributeFiltering();
#endif
            builder.RegisterType<AppCommandTypeConverter>().AsSelf().WithCallerMetadata();
            builder.RegisterType<ObjectStringTypeConverter>().AsSelf().WithCallerMetadata();
            builder.RegisterType<MicrosoftUserViewModel>().AsImplementedInterfaces().AsSelf().WithCallerMetadata();

        }

        public bool RegisterPython { get; set; }

#pragma warning disable 1998
    }
}