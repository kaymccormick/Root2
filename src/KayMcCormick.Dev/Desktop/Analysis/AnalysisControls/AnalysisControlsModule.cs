#region header

// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisControls
// AnalysisControlsModule.cs
// 
// 2020-03-06-12:50 PM
// 
// ---

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using AnalysisAppLib;
using AnalysisAppLib.Syntax;
using AnalysisAppLib.ViewModel;
using AnalysisControls.Commands;
using AnalysisControls.Controls;
using AnalysisControls.TypeDescriptors;
using AnalysisControls.ViewModel;
using AnalysisControls.Views;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Core.Resolving;
using Autofac.Extras.AttributeMetadata;
using Autofac.Features.AttributeFilters;
using Autofac.Features.Metadata;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Command;
using KayMcCormick.Dev.Container;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Lib.Wpf;
using KayMcCormick.Lib.Wpf.Command;
using KmDevLib;
using Microsoft.CodeAnalysis.CSharp;
using Container = Autofac.Core.Container;
using MethodInfo = AnalysisAppLib.MethodInfo;
using Module = Autofac.Module;

namespace AnalysisControls
{
    // made internal 3/11
    /// <summary>
    /// </summary>
    public sealed class AnalysisControlsModule : Module
    {
        private MyReplaySubject<LogEventInstance> _logEventInstanceReplaySubject;

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        // ReSharper disable once AnnotateNotNullParameter
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppSettingsWindow>().AsSelf().As<Window>().WithCallerMetadata();
            builder.RegisterType<AppSettingsViewModel>().SingleInstance().WithCallerMetadata();
            builder.RegisterAssemblyTypes(typeof(AnalysisControlsModule).Assembly)
                .Where(type => (typeof(IDisplayableAppCommand).IsAssignableFrom(type) && !type.IsAbstract && type != typeof(OpenFileCommand2))).As<IDisplayableAppCommand>()
                .WithCallerMetadata().WithAttributedMetadata();
//            builder.RegisterType<SyntaxNodeProperties>().AsImplementedInterfaces();
            builder.RegisterType<MiscInstanceInfoProvider>()
                .AsSelf()
                .As<TypeDescriptionProvider>()
                .WithCallerMetadata()
                .OnActivating(
                    args =>
                    {
                        foreach (var type in new[]
                        {
                            typeof(IComponentRegistration), typeof(ComponentRegistration)
                        })
                            TypeDescriptor.AddProvider(args.Instance, type);
                    }
                );

            if (RegisterControlViewCommandAdapters)
            {
                // builder
                    // .RegisterAdapter<Meta<Func<LayoutDocumentPane, IControlView>>,
                        // Func<LayoutDocumentPane, IDisplayableAppCommand>>(ControlViewCommandAdapter)
                    // .As<Func<LayoutDocumentPane, IDisplayableAppCommand>>()
                    // .WithMetadata("Category", Category.Management)
                    // .WithMetadata("Group", "misc")
                    // .WithCallerMetadata();
                var x = builder
                    .RegisterAdapter<Meta<Lazy<IControlView>>,
                        Meta<IDisplayableAppCommand>>(ControlViewCommandAdapter3)
                    .As<Meta<IDisplayableAppCommand>>()//.As<IBaseLibCommand>()
                    .WithMetadata("Category", Category.Management)
                    .WithMetadata("Group", "misc")
                    .WithCallerMetadata();
            }

            
#if false
            var kayTypes = AppDomain.CurrentDomain.GetAssemblies()
                .Where(
                    a => a
                        .GetCustomAttributes<AssemblyCompanyAttribute
                        >()
                        .Any(tx => tx.Company == "Kay McCormick")
                )
                .SelectMany(az => az.GetTypes())
                .ToList();
#if false
            // foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            // {

            // if (assembly.IsDynamic)
            // continue;
            // foreach (var exportedType in assembly.GetExportedTypes())
            // {
            // if (typeof(IDictionary).IsAssignableFrom(exportedType) ||
            // typeof(IEnumerable).IsAssignableFrom(exportedType))
            // {
            // kayTypes.Add(exportedType);
            // DebugUtils.WriteLine(exportedType.FullName);
            // }
            // }
            // }
#endif
            kayTypes.Add(typeof(IInstanceLookup));
            kayTypes.Add(typeof(ActivationInfo));
            kayTypes.Add(typeof(Container));
            kayTypes.Add(typeof(IResolveOperation));
            //kayTypes.Add(typeof(Type));
            kayTypes.Add(typeof(MethodInfo));
            kayTypes.Add(typeof(PropertyPath));
            kayTypes.Add(typeof(PropertyInfo));
            kayTypes.Add(typeof(MemberInfo));
            var collection = typeof(CSharpSyntaxNode).Assembly.GetExportedTypes()
                .Where(t => typeof(CSharpSyntaxNode).IsAssignableFrom(t)).ToList();
            foreach (var type in collection)
            {
                //DebugUtils.WriteLine("Type enabled for custom descriptor " + type.FullName);
                kayTypes.Add(type);
            }

		kayTypes.Clear();
            var xx = new CustomTypes(kayTypes);

            builder.RegisterInstance(xx).OnActivating(args =>
            {
                args.Instance.ComponentContext = args.Context;
            });

#endif
            _logEventInstanceReplaySubject = new MyReplaySubject<LogEventInstance>();
            builder.RegisterInstance(_logEventInstanceReplaySubject).AsSelf().AsImplementedInterfaces().WithCallerMetadata();
            builder.RegisterInstance(_logEventInstanceReplaySubject.Subject).AsSelf().AsImplementedInterfaces().WithCallerMetadata();
            //kayTypes.Clear();

            builder.RegisterType<UiElementTypeConverter>().SingleInstance().WithCallerMetadata();
            // builder.Register((c, p) =>
            // {
            // var lifetimeScope = c.Resolve<ILifetimeScope>();
            // Func<Type, TypeConverter> f = (t) =>
            // {                        return new UiElementTypeConverter(lifetimeScope);
            // };
            // return f;
            // }).As < Func<Type, TypeConverter>>();

            // var types = new[] { typeof ( AppTypeInfo ) , typeof ( SyntaxFieldInfo ) } ;
            // builder.RegisterInstance ( types )
            // .WithMetadata ( "Custom" , true )
            // .AsImplementedInterfaces ( )
            // .AsSelf ( ) ;


            // builder.RegisterType<ControlsProvider>().WithAttributeFiltering().InstancePerLifetimeScope().As<IControlsProvider>()
                // .AsSelf();


            // builder.RegisterType<AnalysisCustomTypeDescriptor>().AsSelf().AsImplementedInterfaces();

            builder.RegisterAdapter<IBaseLibCommand, IAppCommand>(
                    (
                        context
                        , parameters
                        , arg3
                    ) => new
                        LambdaAppCommand(
                            arg3
                                .ToString()
                            , (command, arg4)
                                => arg3
                                    .ExecuteAsync(arg4)
                            , arg3
                                .Argument
                            , arg3
                                .OnFault
                        )
                )
                .WithCallerMetadata();

            builder.RegisterAssemblyTypes(typeof(AnalysisControlsModule).Assembly).AssignableTo<IModelProvider>().AsImplementedInterfaces().WithAttributeFiltering().WithCallerMetadata();
            builder.RegisterType<Main1Model>().AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope().WithCallerMetadata();
            builder.RegisterType<DocumentHost>().AsImplementedInterfaces().InstancePerLifetimeScope().WithCallerMetadata();
            builder.RegisterType<ContentSelector>().AsImplementedInterfaces().InstancePerLifetimeScope().WithCallerMetadata();
            builder.RegisterType<Main1Mode2>().WithCallerMetadata();
            builder.RegisterType<MiscCommands>().WithCallerMetadata();
            builder.Register((c) =>
            {
                var ctx = c.Resolve<IComponentContext>();
                var docHost = c.Resolve<IDocumentHost>();
                var sel = c.Resolve<IContentSelector>();
                return new LambdaAppCommand("Syntax types view (1)", (command, o) =>
                {
                    var doc = DocModel.CreateInstance(command.DisplayName);
                    var syntaxTypesControl = new SyntaxTypesControl();
                    syntaxTypesControl.ViewModel = ctx.Resolve<TypesViewModel>();
                    doc.Content = syntaxTypesControl;
                    docHost.AddDocument(doc);
                    sel.ActiveContent = doc;
                    return Task.FromResult(AppCommandResult.Success);
                }, null);
            }).As<IDisplayableAppCommand>().WithCallerMetadata();
            builder.Register((c1) =>
            {
                var c = c1.Resolve<ILifetimeScope>();
                return new LambdaAppCommand("Build Types view", async (command, o) =>
                {
                    
                    await c.Resolve<MiscCommands>().BuildTypeViewAsync(command, c.Resolve<IAppDbContext1>(),
                        c, c.Resolve<AppDbContextHelper>(),
                        c.Resolve<JsonSerializerOptions>());
                    return AppCommandResult.Success;
                }, null);
            }).As<IDisplayableAppCommand>().WithCallerMetadata();

                builder.Register(
                    (context, parameters) =>
                    {
                        try
                        {
                            if (parameters.TypedAs<bool>() == false)
                                return new TypesViewModel(
                                    context
                                        .Resolve<
                                            JsonSerializerOptions
                                        >(), 
                                    context.Resolve<MyReplaySubject<AppTypeInfo>>()
                                );
                        }
                        catch (Exception ex)
                        {
                            DebugUtils.WriteLine(ex.ToString());
                        }

                        using (var stream = Assembly
                            .GetExecutingAssembly()
                            .GetManifestResourceStream(
                                "AnalysisControls.TypesViewModel.xaml"
                            ))
                        {
                            if (stream == null)
                            {
                                DebugUtils.WriteLine("no stream");
                                return new TypesViewModel(
                                    context
                                        .Resolve<
                                            JsonSerializerOptions
                                        >(), context
                                        .Resolve<
                                            MyReplaySubject<AppTypeInfo>
                                        >()
                                );
                            }

                            try
                            {
                                var v = (TypesViewModel) XamlReader
                                    .Load(stream);
                                stream.Close();
                                return v;
                            }
                            catch (Exception)
                            {
                                // return new TypesViewModel(
                                    // context
                                        // .Resolve<
                                            // JsonSerializerOptions
                                        // >()
                                // );
                                 //throw;
                            }
                            var m = new TypesViewModel(context.Resolve<JsonSerializerOptions>());
                            m.BeginInit();
                            m.EndInit();
                            return m;
                        }
                    }
                )
                .AsSelf()
                .SingleInstance()
                .AsImplementedInterfaces()
                .WithCallerMetadata();
        }

        private Meta<IDisplayableAppCommand> ControlViewCommandAdapter3(IComponentContext arg1, IEnumerable<Parameter> arg2, Meta<Lazy<IControlView>> arg3)
        {
            var cmd = ControlViewCommandAdapter2(arg1, arg2, arg3);
            IDictionary<string, object> d = new Dictionary<string, object>();
            d["CallerFilePath"] = "AnalysisControlsModule";
            return new Meta<IDisplayableAppCommand>(cmd, d);
        }

        private IDisplayableAppCommand ControlViewCommandAdapter2(IComponentContext arg1, IEnumerable<Parameter> arg2,
            Meta<Lazy<IControlView>> arg3)
        {
            arg3.Metadata.TryGetValue("Title", out var titleo);
            arg3.Metadata.TryGetValue("ImageSource", out var imageSource);
            arg3.Metadata.TryGetValue("RequiredOptionName", out var requireoption);
            // object res = r.ResolveResource ( imageSource ) ;
            // var im = res as ImageSource ;

            var title = (string) titleo ?? "no title";

            return new LambdaAppCommand(
                title
                , CommandFuncAsync2
                , Tuple.Create(arg3, arg1.Resolve<ILifetimeScope>()), null, (command, o) =>
                {
                    if (requireoption != null) return false;

                    return true;
                }) {LargeImageSourceKey = imageSource};

        }

#pragma warning disable 1998
        private async Task<IAppCommandResult> CommandFuncAsync2(LambdaAppCommand command)
#pragma warning restore 1998
        {
            var x = (Tuple<Meta<Lazy<IControlView>>, ILifetimeScope>) command.Argument;
            var view = x.Item1.Value;
            DebugUtils.WriteLine($"Calling view func ({command})");
            var n = DateTime.Now;
            DocModel doc1 = DocModel.CreateInstance(command.DisplayName);
                doc1.Content = view.Value;
            x.Item2.Resolve<IDocumentHost>().AddDocument(doc1);
            // var pane1 = x.Item2.Resolve<ReplaySubject<IControlView>>();
            // DebugUtils.WriteLine((DateTime.Now - n).ToString());
            // var props = MetaHelper.GetMetadataProps(x.Item1.Metadata);
            //var doc = new DocModel {Content = view, Title = props.Title};
            // pane1.OnNext(view.Value);
            DebugUtils.WriteLine("returning success");
            return AppCommandResult.Success;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool RegisterControlViewCommandAdapters { get; set; } = true;

        [NotNull]
        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        private FrameworkElement Func(
            [NotNull] IComponentContext c1
            // ReSharper disable once UnusedParameter.Local
            , IEnumerable<Parameter> p1
        )
        {
            var gridView = new GridView();
            gridView.Columns.Add(
                new GridViewColumn
                {
                    DisplayMemberBinding = new Binding("SyntaxKind"), Header = "Kind"
                }
            );
            gridView.Columns.Add(
                new GridViewColumn
                {
                    DisplayMemberBinding = new Binding("Token"), Header = "Token"
                }
            );
            gridView.Columns.Add(
                new GridViewColumn
                {
                    Header = "Raw Kind", DisplayMemberBinding = new Binding("RawKind")
                }
            );

            var binding = new Binding("SyntaxItems")
            {
                Source = c1.Resolve<ISyntaxTokenViewModel>()
            };
            var listView = new ListView {View = gridView};
            listView.SetBinding(ItemsControl.ItemsSourceProperty, binding);
            return listView;
        }

#pragma warning disable 1998

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="observable"></param>
        /// <param name="resources"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ListView ReplayListView<T>(ObservableCollection<T> collection, ReplaySubject<T> observable,
            ResourceDictionary resources, Type t =null)
        {
            
            var lv = new ListView() {Resources = resources,};
            lv.SetValue(ScrollViewer.CanContentScrollProperty, false);
            var gv = new GridView();
            //gv.Columns.Add(new GridViewColumn() {DisplayMemberBinding = new Binding(".")});
            lv.View = gv;
            lv.ItemsSource = collection;
            
            var props = TypeDescriptor.GetProperties(t);
            foreach (PropertyDescriptor prop in props)
            {
                if (!prop.IsBrowsable) continue;
                DebugUtils.WriteLine(prop.Name);
                var gridViewColumn = new GridViewColumn
                {
                    Header = prop.DisplayName, CellTemplateSelector = new ListViewTestSel(prop)
                };
                gv.Columns.Add(gridViewColumn);
            }

            observable.SubscribeOn(Scheduler.Default).ObserveOnDispatcher(DispatcherPriority.Send).Subscribe(
                collection.Add);
            return lv;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="observable"></param>
        /// <param name="resources"></param>
        /// <param name="xType1"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static UIElement ReplayItemsControl<T>(ObservableCollection<T> collection,
            [NotNull] ReplaySubject<T> observable,
            ResourceDictionary resources, Type xType1)
        {
            if (observable == null) throw new ArgumentNullException(nameof(observable));
            var itemsControl = new ItemsControl {ItemsSource = collection};
            var props = TypeDescriptor.GetProperties(typeof(T));
            // foreach (PropertyDescriptor prop in props)
            // {
            //     DebugUtils.WriteLine(prop.Name);
            //     var gridViewColumn = new GridViewColumn();
            //     gridViewColumn.Header = prop.DisplayName;
            //     gridViewColumn.CellTemplateSelector = new ListViewTestSel(prop);
            //     gv.Columns.Add(gridViewColumn);
            // }

            observable.SubscribeOn(Scheduler.Default).ObserveOnDispatcher(DispatcherPriority.Send).Subscribe(
                collection.Add);
            var d = new DockPanel() { LastChildFill = true };
            d.Children.Add(new ScrollViewer { Content = itemsControl });
            return d;

//            return itemsControl;
        }

        // ReSharper disable once UnusedMember.Local
        private static UIElement ReplayListBox<T>(ObservableCollection<T> collection, ReplaySubject<T> observable)
        {
            var lb = new ListBox {ItemsSource = collection};

            observable.SubscribeOn(Scheduler.Default).ObserveOnDispatcher(DispatcherPriority.Send).Subscribe(
                item => { collection?.Add(item); });
            var d = new DockPanel(){LastChildFill = true};
            d.Children.Add(new ScrollViewer { Content = lb });
            return d;
        }
    }
}