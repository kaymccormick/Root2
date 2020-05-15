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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
using AnalysisAppLib.ViewModel;
using AnalysisControls.ViewModel;
using AnalysisControls.Views;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Core.Resolving;
using Autofac.Features.AttributeFilters;
using Autofac.Features.Metadata;
using AvalonDock.Layout;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Command;
using KayMcCormick.Dev.Container;
using KayMcCormick.Lib.Wpf;
using KayMcCormick.Lib.Wpf.Command;
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
        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        // ReSharper disable once AnnotateNotNullParameter
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SyntaxNodeProperties>().AsImplementedInterfaces();
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
                        IDisplayableAppCommand>(ControlViewCommandAdapter2)
                    .As<IDisplayableAppCommand>().As<IBaseLibCommand>()
                    .WithMetadata("Category", Category.Management)
                    .WithMetadata("Group", "misc")
                    .WithCallerMetadata();
            }

            // builder.RegisterType<AllResourcesTreeViewModel>()
            //     .AsSelf()
            //     .As<IAddRuntimeResource>()
            //     .SingleInstance()
            //     .WithCallerMetadata();
            //
            if (RibbonFunc2)
            {
                OldRibbonBuilder(builder);
            }

            var kayTypes = AppDomain.CurrentDomain.GetAssemblies()
                .Where(
                    a => a
                        .GetCustomAttributes<AssemblyCompanyAttribute
                        >()
                        .Any(tx => tx.Company == "Kay McCormick")
                )
                .SelectMany(az => az.GetTypes())
                .ToList();
            kayTypes.Add(typeof(IInstanceLookup));
            kayTypes.Add(typeof(Container));
            kayTypes.Add(typeof(IResolveOperation));
            kayTypes.Add(typeof(Type));
            kayTypes.Add(typeof(MethodInfo));
            kayTypes.Add(typeof(PropertyInfo));
            kayTypes.Add(typeof(MemberInfo));
            var collection = typeof(CSharpSyntaxNode).Assembly.GetExportedTypes()
                .Where(t => typeof(CSharpSyntaxNode).IsAssignableFrom(t)).ToList();
            foreach (var type in collection)
            {
                DebugUtils.WriteLine("Type enabled for custom descriptor " + type.FullName);
                kayTypes.Add(type);
            }

            var xx = new CustomTypes(kayTypes);
            builder.RegisterInstance(xx).OnActivating(args =>
            {
                args.Instance.ComponentContext = args.Context;
            });
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
            builder.RegisterType<ControlsProvider>().WithAttributeFiltering().InstancePerLifetimeScope().As<IControlsProvider>()
                .AsSelf();
            // .WithParameter (
            // new NamedParameter ( "types" , types )
            // ) .AsSelf (  ) ;
//		builder.RegisterGeneric(typeof(AnalysisCustomTypeDescriptor<>)).AsSelf().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<AnalysisCustomTypeDescriptor>().AsSelf().AsImplementedInterfaces();


            builder.RegisterAdapter<IBaseLibCommand, IAppCommand>(
                    (
                        context
                        , parameters
                        , arg3
                    ) => new
                        LambdaAppCommand(
                            arg3
                                .ToString()
                            , command
                                => arg3
                                    .ExecuteAsync()
                            , arg3
                                .Argument
                            , arg3
                                .OnFault
                        )
                )
                .WithCallerMetadata();
            builder.RegisterType<TypesView>()
                .AsSelf()
                .As<IControlView>()
                .WithMetadata(
                    "ImageSource"
                    , "pack://application:,,,/WpfLib;component/Assets/StatusAnnotations_Help_and_inconclusive_32xMD_color.png"
                )
                .WithMetadata("PrimaryRibbon", true)
                .WithCallerMetadata();
            builder.RegisterType<UiElementTypeConverter>().AsSelf();

            builder.RegisterType<Main1Model>();

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
                                        >()
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
                                return new TypesViewModel(
                                    context
                                        .Resolve<
                                            JsonSerializerOptions
                                        >()
                                );
                            }
                        }
                    }
                )
                .AsSelf()
                .SingleInstance()
                .AsImplementedInterfaces()
                .WithCallerMetadata();
        }


        /// <summary>
        /// 
        /// </summary>
        public bool RibbonFunc2 { get; set; }

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
            var pane1 = x.Item2.Resolve<ReplaySubject<IControlView>>();
            DebugUtils.WriteLine((DateTime.Now - n).ToString());
            var props = MetaHelper.GetMetadataProps(x.Item1.Metadata);
            //var doc = new DocModel {Content = view, Title = props.Title};
            pane1.OnNext(view.Value);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="p"></param>
        /// <param name="metaFunc"></param>
        /// <returns></returns>
        [NotNull]
        public static Func<LayoutDocumentPane, IDisplayableAppCommand> ControlViewCommandAdapter(
            [NotNull] IComponentContext c
            , IEnumerable<Parameter> p
            , [NotNull] Meta<Func<LayoutDocumentPane, IControlView>> metaFunc
        )
        {
            metaFunc.Metadata.TryGetValue("Title", out var titleo);
            metaFunc.Metadata.TryGetValue("ImageSource", out var imageSource);
            // object res = r.ResolveResource ( imageSource ) ;
            // var im = res as ImageSource ;

            var title = (string) titleo ?? "no title";

            return pane => (IDisplayableAppCommand) new LambdaAppCommand(
                title
                , CommandFuncAsync
                , Tuple.Create(
                    metaFunc
                        .Value
                    , pane
                )
            )
            {
                LargeImageSourceKey = imageSource
            };
        }
#pragma warning disable 1998
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static async Task<IAppCommandResult> CommandFuncAsync(
#pragma warning restore 1998
#pragma warning restore 1998
            [NotNull] LambdaAppCommand command
        )
        {
            var (viewFunc1, pane1) =
                (Tuple<Func<LayoutDocumentPane, IControlView>, LayoutDocumentPane>)
                command.Argument;

            DebugUtils.WriteLine($"Calling view func ({command})");
            var n = DateTime.Now;
            var view = viewFunc1(pane1);
            DebugUtils.WriteLine((DateTime.Now - n).ToString());
            var doc = new LayoutDocument {Content = view};
            pane1.Children.Add(doc);
            pane1.SelectedContentIndex = pane1.Children.IndexOf(doc);
            DebugUtils.WriteLine("returning success");
            return AppCommandResult.Success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="observable"></param>
        /// <param name="resources"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ListView ReplayListView<T>(ObservableCollection<T> collection, ReplaySubject<T> observable,
            ResourceDictionary resources)
        {
            var lv = new ListView() {Resources = resources};
            var gv = new GridView();
            //gv.Columns.Add(new GridViewColumn() {DisplayMemberBinding = new Binding(".")});
            lv.View = gv;
            lv.ItemsSource = collection;
            var props = TypeDescriptor.GetProperties(typeof(T));
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
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ItemsControl ReplayItemsControl<T>(ObservableCollection<T> collection,
            ReplaySubject<T> observable,
            ResourceDictionary resources)
        {
            var itemsControl = new ItemsControl {Resources = resources, ItemsSource = collection};
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
            return itemsControl;
        }

        // ReSharper disable once UnusedMember.Local
        private static ListBox ReplayListBox<T>(ObservableCollection<T> collection, ReplaySubject<T> observable)
        {
            var lb = new ListBox {ItemsSource = collection};

            observable.SubscribeOn(Scheduler.Default).ObserveOnDispatcher(DispatcherPriority.Send).Subscribe(
                item => { collection?.Add(item); });
            return lb;
        }

        private static void OldRibbonBuilder(ContainerBuilder builder)
        {
            builder.RegisterType<RibbonBuilder>();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AssignableTo<IRibbonComponent>()
                .AsImplementedInterfaces().AsSelf();
            //builder.RegisterAdapter<>()
            builder.RegisterType<AppRibbon>().AsImplementedInterfaces().AsSelf().WithCallerMetadata();
            builder.RegisterType<AppRibbonTab>().AsImplementedInterfaces().AsSelf().WithCallerMetadata();
            foreach (Category enumValue in typeof(Category).GetEnumValues())
            {
                var cat = new CategoryInfo(enumValue);
                builder.RegisterInstance(cat).As<CategoryInfo>();
            }

            builder.Register((c, p) =>
            {
                var r = new AppRibbonTabSet();
                foreach (var ct in c.Resolve<IEnumerable<CategoryInfo>>())
                {
                    var tab = new AppRibbonTab { Category = ct };
                    r.TabSet.Add(tab);
                }

                foreach (var meta in c.Resolve<IEnumerable<Meta<Lazy<IBaseLibCommand>>>>())
                {
                    DebugUtils.WriteLine(meta.ToString());
                    var props = MetaHelper.GetProps(meta);
                    DebugUtils.WriteLine(props.ToString());
                }

                return r;
            });

            // builder.RegisterAssemblyTypes(ThisAssembly).AssignableTo<IBaseLibCommand>().AsImplementedInterfaces()
            // .WithCallerMetadata();
            builder.Register((c, p) =>
            {
                var cm = c.Resolve<IEnumerable<Meta<Lazy<IBaseLibCommand>>>>();

                var dict = new Dictionary<Category, Info1>();
                foreach (var c1 in cm)
                {
                    foreach (var m1 in c1.Metadata) DebugUtils.WriteLine($"{m1.Key} = {m1.Value}");

                    var ci = new CommandInfo { Command = c1 };

                    if (c1.Metadata.TryGetValue("Category", out var cv))
                    {
                    }

                    var cat = Category.None;
                    try
                    {
                        if (cv != null) cat = (Category)cv;
                    }
                    catch (Exception ex)
                    {
                        // ignored
                    }

                    c1.Metadata.TryGetValue("Group", out var group);
                    if (@group == null) @group = "no group";

                    if (!dict.TryGetValue(cat, out var i1))
                    {
                        i1 = new Info1()
                        {
                            Category = (Category)cat
                        };
                        dict[cat] = i1;
                    }

                    if (@group == null)
                    {
                        i1.Ungrouped.Add(ci);
                    }
                    else
                    {
                        if (!i1.Infos.TryGetValue((string)@group, out var i2))
                        {
                            i2 = new Info2 { Group = (string)@group };
                            i1.Infos[(string)@group] = i2;
                        }

                        i2.Infos.Add(ci);
                    }
                }

                DebugUtils.WriteLine("***");
                foreach (var k in dict.Keys)
                {
                    DebugUtils.WriteLine(k.ToString());
                    foreach (var cx in dict[k].Infos)
                        foreach (var cxx in cx.Value.Infos)
                            DebugUtils.WriteLine(cxx.Command.ToString());
                }

                return dict;
            }).AsSelf().WithCallerMetadata().SingleInstance();
        }

    }
}