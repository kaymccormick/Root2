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
using System ;
using System.Collections;
using System.Collections.Generic ;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel ;
using System.Linq ;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection ;
using System.Text.Json ;
using System.Threading.Tasks;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data ;
using System.Windows.Markup ;
using System.Windows.Threading;
using AnalysisAppLib;
using AnalysisAppLib.Syntax ;
using AnalysisAppLib.ViewModel ;
using AnalysisControls.ViewModel ;
using AnalysisControls.Views ;
using Autofac ;
using Autofac.Core ;
using Autofac.Core.Registration;
using Autofac.Core.Resolving;
using Autofac.Features.AttributeFilters ;
using Autofac.Features.Metadata ;
using AvalonDock.Layout;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Command;
using KayMcCormick.Dev.Container ;
using KayMcCormick.Lib.Wpf ;
using KayMcCormick.Lib.Wpf.Command ;
using KayMcCormick.Lib.Wpf.ViewModel;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Container = Autofac.Core.Container;
using MethodInfo = AnalysisAppLib.MethodInfo;
using Module = Autofac.Module ;

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
        protected override void Load ( ContainerBuilder builder )
        {
            builder.RegisterType<MiscInstanceInfoProvider>()
                .AsSelf()
                .As<TypeDescriptionProvider>()
                .WithCallerMetadata()
                .OnActivating(
                    args => {
                        foreach (var type in new[]
                        {
                            typeof ( IComponentRegistration )
                            , typeof ( ComponentRegistration )
                        })
                        {
                            TypeDescriptor.AddProvider(args.Instance, type);
                        }
                    }
                );
            if (RegisterControlViewCommandAdapters)
            {
                
                builder
                    .RegisterAdapter<Meta<Func<LayoutDocumentPane, IControlView>>,
                        Func<LayoutDocumentPane, IDisplayableAppCommand>>(ControlViewCommandAdapter)
                    .As<Func<LayoutDocumentPane, IDisplayableAppCommand>>()
                    .WithMetadata("Category", Category.Management)
                    .WithMetadata("Group", "misc")
                    .WithCallerMetadata();
                builder
                    .RegisterAdapter<Meta<Lazy<IControlView>>,
                        IDisplayableAppCommand>(ControlViewCommandAdapter2)
                    .As<IDisplayableAppCommand>()
                    .WithMetadata("Category", Category.Management)
                    .WithMetadata("Group", "misc")
                    .WithCallerMetadata();
            }

            builder.RegisterType<AllResourcesTreeViewModel>()
                .AsSelf()
                .As<IAddRuntimeResource>()
                .SingleInstance()
                .WithCallerMetadata();

            if (RibbonFunc2)
            {
                builder.RegisterType<RibbonBuilder>();
                builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AssignableTo<IRibbonComponent>()
                    .AsImplementedInterfaces().AsSelf();
                //builder.RegisterAdapter<>()
                builder.RegisterType<AppRibbon>().AsImplementedInterfaces().AsSelf().WithCallerMetadata();
                builder.RegisterType<AppRibbonTab>().AsImplementedInterfaces().AsSelf().WithCallerMetadata();
                foreach (Category enumValue in typeof(Category).GetEnumValues())
                {
                    CategoryInfo cat = new CategoryInfo(enumValue);
                    builder.RegisterInstance(cat).As<CategoryInfo>();
                }

                builder.Register((c, p) =>
                {
                    var r = new AppRibbonTabSet();
                    foreach (var ct in c.Resolve<IEnumerable<CategoryInfo>>())
                    {
                        var tab = new AppRibbonTab {Category = ct};
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

                builder.RegisterAssemblyTypes(ThisAssembly).AssignableTo<IBaseLibCommand>().AsImplementedInterfaces()
                    .WithCallerMetadata();
                builder.Register((c, p) =>
                {

                    var cm = c.Resolve<IEnumerable<Meta<Lazy<IBaseLibCommand>>>>();

                    Dictionary<Category, Info1> dict = new Dictionary<Category, Info1>();
                    foreach (var c1 in cm)
                    {
                        foreach (var m1 in c1.Metadata)
                        {
                            DebugUtils.WriteLine($"{m1.Key} = {m1.Value}");
                        }

                        CommandInfo ci = new CommandInfo {Command = c1};

                        if (c1.Metadata.TryGetValue("Category", out var cv))
                        {
                        }

                        Category cat = Category.None;
                        try
                        {
                            cat = (Category) cv;
                        }
                        catch (Exception ex)
                        {

                        }

                        c1.Metadata.TryGetValue("Group", out var group);
                        if (group == null)
                        {
                            group = "no group";
                        }

                        if (!dict.TryGetValue(cat, out var i1))
                        {
                            i1 = new Info1()
                            {
                                Category = (Category) cat,
                            };
                            dict[cat] = i1;
                        }

                        if (group == null)
                        {
                            i1.Ungrouped.Add(ci);
                        }
                        else
                        {
                            if (!i1.Infos.TryGetValue((string) group, out var i2))
                            {

                                i2 = new Info2 {Group = (string) group};
                                i1.Infos[(string) group] = i2;
                            }

                            i2.Infos.Add(ci);
                        }
                    }

                    DebugUtils.WriteLine("***");
                    foreach (var k in dict.Keys)
                    {
                        DebugUtils.WriteLine(k.ToString());
                        foreach (var cx in dict[k].Infos)
                        {
                            foreach (var cxx in cx.Value.Infos)
                            {
                                DebugUtils.WriteLine(cxx.Command.ToString());
                            }

                        }

                    }

                    return dict;

                }).AsSelf().WithCallerMetadata().SingleInstance();

            }

            var kayTypes = AppDomain.CurrentDomain.GetAssemblies ( )
                                        .Where (
                                                a => a
                                                    .GetCustomAttributes < AssemblyCompanyAttribute
                                                     > ( )
                                                    .Any ( tx => tx.Company == "Kay McCormick" )
                                               )
                                        .SelectMany ( az => az.GetTypes ( ) )
                                        .ToList ( ) ;
            kayTypes.Add(typeof(IInstanceLookup));
            kayTypes.Add(typeof(Container));
            kayTypes.Add(typeof(IResolveOperation));
            kayTypes.Add(typeof(Type));
            kayTypes.Add(typeof(MethodInfo));
            kayTypes.Add(typeof(PropertyInfo));
            kayTypes.Add(typeof(MemberInfo));

            var xx = new CustomTypes(kayTypes);
                builder.RegisterInstance(xx);
                //builder.Register((c, p) => { return kayTypes; }).Keyed<IEnumerable<Type>>("Custom");
                builder.RegisterType<UiElementTypeConverter>().SingleInstance().WithCallerMetadata();
                builder.Register((c, p) =>
                {
                    var lifetimeScope = c.Resolve<ILifetimeScope>();
                    Func<Type, TypeConverter> f = (t) =>
                    {                        return new UiElementTypeConverter(lifetimeScope);
                    };
                    return f;
                }).As < Func<Type, TypeConverter>>();

                var types = new[] { typeof ( AppTypeInfo ) , typeof ( SyntaxFieldInfo ) } ;
                builder.RegisterInstance ( types )
                       .WithMetadata ( "Custom" , true )
                       .AsImplementedInterfaces ( )
                       .AsSelf ( ) ;
                builder.RegisterType < ControlsProvider > ( ).WithAttributeFiltering ( ).SingleInstance().As<IControlsProvider>().AsSelf();
                // .WithParameter (
                // new NamedParameter ( "types" , types )
                // ) .AsSelf (  ) ;
                builder.RegisterType < AnalysisCustomTypeDescriptor > ( )
                       .AsSelf ( )
                       .AsImplementedInterfaces ( ) ;


                builder.RegisterAdapter < IBaseLibCommand , IAppCommand > (
                                                                           (
                                                                               context
                                                                             , parameters
                                                                             , arg3
                                                                           ) => new
                                                                               LambdaAppCommand (
                                                                                                 arg3
                                                                                                    .ToString ( )
                                                                                               , command
                                                                                                     => arg3
                                                                                                        .ExecuteAsync ( )
                                                                                               , arg3
                                                                                                    .Argument
                                                                                               , arg3
                                                                                                    .OnFault
                                                                                                )
                                                                          )
                       .WithCallerMetadata ( ) ;
                builder.RegisterType < TypesView > ( )
                       .AsSelf ( )
                       .As < IControlView > ( )
                       .WithMetadata (
                                      "ImageSource"
                                    , "pack://application:,,,/KayMcCormick.Lib.Wpf;component/Assets/StatusAnnotations_Help_and_inconclusive_32xMD_color.png"
                                     )
                       .WithMetadata ( "Ribbon" , true )
                       .WithCallerMetadata ( ) ;
                builder.RegisterType<UiElementTypeConverter>().AsSelf();
                
            builder.RegisterType<Main1Model>();
           
            builder.Register (
                                  ( context , parameters ) => {
                                      try
                                      {
                                          if ( parameters.TypedAs < bool > ( ) == false )
                                          {
                                              return new TypesViewModel (
                                                                         context
                                                                            .Resolve <
                                                                                 JsonSerializerOptions
                                                                             > ( )
                                                                        ) ;
                                          }
                                      }
                                      catch ( Exception ex )
                                      {
                                          DebugUtils.WriteLine ( ex.ToString ( ) ) ;
                                      }

                                      using ( var stream = Assembly
                                                          .GetExecutingAssembly ( )
                                                          .GetManifestResourceStream (
                                                                                      "AnalysisControls.TypesViewModel.xaml"
                                                                                     ) )
                                      {
                                          if ( stream == null )
                                          {
                                              DebugUtils.WriteLine ( "no stream" ) ;
                                              return new TypesViewModel (
                                                                         context
                                                                            .Resolve <
                                                                                 JsonSerializerOptions
                                                                             > ( )
                                                                        ) ;
                                          }

                                          try
                                          {
                                              var v = ( TypesViewModel ) XamlReader
                                                 .Load ( stream ) ;
                                              stream.Close ( ) ;
                                              return v ;
                                          }
                                          catch ( Exception )
                                          {
                                              return new TypesViewModel (
                                                                         context
                                                                            .Resolve <
                                                                                 JsonSerializerOptions
                                                                             > ( )
                                                                        ) ;
                                          }
                                      }
                                  }
                                 )
                       .AsSelf ( )
                       .SingleInstance()
                       .AsImplementedInterfaces ( )
                       .WithCallerMetadata ( ) ;

            builder.RegisterType < SyntaxPanel > ( )
                       .Keyed < IControlView > ( typeof ( CompilationUnitSyntax ) )
                       .AsSelf ( )
                       .WithCallerMetadata ( ) ;
                builder.RegisterType < SyntaxPanelViewModel > ( )
                       .AsImplementedInterfaces ( )
                       .AsSelf ( )
                       .WithCallerMetadata ( ) ;
        }

        public bool RibbonFunc2 { get; set; }

        private IDisplayableAppCommand ControlViewCommandAdapter2(IComponentContext arg1, IEnumerable<Parameter> arg2, Meta<Lazy<IControlView>> arg3)
        {
            LayoutDocumentPane pane = null;
            arg3.Metadata.TryGetValue("Title", out var titleo);
            arg3.Metadata.TryGetValue("ImageSource", out var imageSource);
            // object res = r.ResolveResource ( imageSource ) ;
            // var im = res as ImageSource ;

            var title = (string)titleo ?? "no title";

            return (IDisplayableAppCommand)new LambdaAppCommand(
                title
                , CommandFuncAsync2
                , Tuple.Create(arg3, arg1.Resolve<ILifetimeScope>())
                
            )
            {
                LargeImageSourceKey = imageSource
            };
        }

        private async Task<IAppCommandResult> CommandFuncAsync2(LambdaAppCommand command)
        {
            var x = (Tuple<Meta<Lazy<IControlView>>, ILifetimeScope>)command.Argument;
            var view = x.Item1.Value;
            DebugUtils.WriteLine($"Calling view func ({command})");
            var n = DateTime.Now;
            var pane1 = x.Item2.Resolve<LayoutDocumentPane>();
            DebugUtils.WriteLine((DateTime.Now - n).ToString());
            var doc = new LayoutDocument { Content = view };
            pane1.Children.Add(doc);
            pane1.SelectedContentIndex = pane1.Children.IndexOf(doc);
            DebugUtils.WriteLine("returning success");
            return AppCommandResult.Success;
        }

        public bool RegisterControlViewCommandAdapters { get; set; } = true;

        [ NotNull ]
            // ReSharper disable once UnusedMember.Local
            // ReSharper disable once UnusedParameter.Local
            private FrameworkElement Func (
                [ NotNull ] IComponentContext c1
                // ReSharper disable once UnusedParameter.Local
              , IEnumerable<Parameter> p1
            )
            {
                var gridView = new GridView ( ) ;
                gridView.Columns.Add (
                                      new GridViewColumn
                                      {
                                          DisplayMemberBinding = new Binding ( "SyntaxKind" )
                                        , Header               = "Kind"
                                      }
                                     ) ;
                gridView.Columns.Add (
                                      new GridViewColumn
                                      {
                                          DisplayMemberBinding = new Binding ( "Token" )
                                        , Header               = "Token"
                                      }
                                     ) ;
                gridView.Columns.Add (
                                      new GridViewColumn
                                      {
                                          Header               = "Raw Kind"
                                        , DisplayMemberBinding = new Binding ( "RawKind" )
                                      }
                                     ) ;

                var binding = new Binding ( "SyntaxItems" )
                              {
                                  Source = c1.Resolve < ISyntaxTokenViewModel > ( )
                              } ;
                var listView = new ListView { View = gridView } ;
                listView.SetBinding ( ItemsControl.ItemsSourceProperty , binding ) ;
                return listView ;
            }

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

                var title = (string)titleo ?? "no title";

                return pane => (IDisplayableAppCommand)new LambdaAppCommand(
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
            public static async Task<IAppCommandResult> CommandFuncAsync(
#pragma warning restore 1998
                [NotNull] LambdaAppCommand command
            )
            {
                //await JoinableTaskFactory.SwitchToMainThreadAsync (  ) ;
                var (viewFunc1, pane1) =
                    (Tuple<Func<LayoutDocumentPane, IControlView>, LayoutDocumentPane>)
                    command.Argument;

                DebugUtils.WriteLine($"Calling viewfunc ({command})");
                var n = DateTime.Now;
                var view = viewFunc1(pane1);
                DebugUtils.WriteLine((DateTime.Now - n).ToString());
                var doc = new LayoutDocument { Content = view };
                pane1.Children.Add(doc);
                pane1.SelectedContentIndex = pane1.Children.IndexOf(doc);
                DebugUtils.WriteLine("returning success");
                return AppCommandResult.Success;
            }

            public static ListView ReplayListView<T>(ObservableCollection<T> collection, ReplaySubject<T> observable,
                ResourceDictionary resources)
            {
                ListView lv = new ListView() {Resources = resources};
                var gv = new GridView();
                //gv.Columns.Add(new GridViewColumn() {DisplayMemberBinding = new Binding(".")});
                lv.View = gv;
                lv.ItemsSource = collection;
                var props  = TypeDescriptor.GetProperties(typeof(T));
                foreach (PropertyDescriptor prop in props)
                {
                    if (!prop.IsBrowsable)
                    {
                        continue;
                    }
                    DebugUtils.WriteLine(prop.Name);
                    var gridViewColumn = new GridViewColumn();
                    gridViewColumn.Header = prop.DisplayName;
                    gridViewColumn.CellTemplateSelector = new ListViewTestSel(prop);
                    gv.Columns.Add(gridViewColumn);
                }

                observable.SubscribeOn(Scheduler.Default).ObserveOnDispatcher(DispatcherPriority.Send).Subscribe(
                    item =>
                    {
                        collection.Add(item);
                    });
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
            public static ItemsControl ReplayItemsControl<T>(ObservableCollection<T> collection, ReplaySubject<T> observable,
                ResourceDictionary resources)
            {
                ItemsControl itemsControl = new ItemsControl() {Resources = resources};
                itemsControl.ItemsSource = collection;
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
                    item =>
                    {
                        collection.Add(item);
                    });
                return itemsControl;
            }

        private static ListBox ReplayListBox<T>(ObservableCollection<T> collection, ReplaySubject<T> observable)
            {
                ListBox lb = new ListBox();
                lb.ItemsSource = collection;

                observable.SubscribeOn(Scheduler.Default).ObserveOnDispatcher(DispatcherPriority.Send).Subscribe(
                    item =>
                    {
                        collection.Add(item);
                    });
                return lb;
            }
    }
    public  class MyRibbonComboBox : RibbonComboBox
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {

            base.PrepareContainerForItemOverride(element, item);
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return base.IsItemItsOwnContainerOverride(item);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return base.GetContainerForItemOverride();
        }

        public MyRibbonComboBox()
        {
            
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }
    }

    public class MyItemContainerGenerator : IItemContainerGenerator
    {
        public ItemContainerGenerator GetItemContainerGeneratorForPanel(Panel panel)
        {
            return null;
            ;
        }

        public IDisposable StartAt(GeneratorPosition position, GeneratorDirection direction)
        {
            throw new NotImplementedException();
        }

        public IDisposable StartAt(GeneratorPosition position, GeneratorDirection direction, bool allowStartAtRealizedItem)
        {
            throw new NotImplementedException();
        }

        public DependencyObject GenerateNext()
        {
            throw new NotImplementedException();
        }

        public DependencyObject GenerateNext(out bool isNewlyRealized)
        {
            throw new NotImplementedException();
        }

        public void PrepareItemContainer(DependencyObject container)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(GeneratorPosition position, int count)
        {
            throw new NotImplementedException();
        }

        public GeneratorPosition GeneratorPositionFromIndex(int itemIndex)
        {
            throw new NotImplementedException();
        }

        public int IndexFromGeneratorPosition(GeneratorPosition position)
        {
            throw new NotImplementedException();
        }
    }
}
