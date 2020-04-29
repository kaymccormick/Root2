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
using System.Collections ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Diagnostics;
using System.IO;
using System.Linq ;
using System.Reflection ;
using System.Text.Json ;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Input;
using System.Windows.Markup ;
using AnalysisAppLib;
using AnalysisAppLib.Project;
using AnalysisAppLib.Syntax ;
using AnalysisAppLib.ViewModel ;
using AnalysisControls.ViewModel ;
using AnalysisControls.Views ;
using Autofac ;
using Autofac.Core ;
using Autofac.Core.Registration;
using Autofac.Features.AttributeFilters ;
using Autofac.Features.Metadata ;
using AvalonDock.Layout;
using FindLogUsages;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Command;
using KayMcCormick.Dev.Container ;
using KayMcCormick.Dev.Metadata ;
using KayMcCormick.Lib.Wpf ;
using KayMcCormick.Lib.Wpf.Command ;
using KayMcCormick.Lib.Wpf.ViewModel;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.Win32;
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
            }

            builder.RegisterType<AllResourcesTreeViewModel>()
                .AsSelf()
                .As<IAddRuntimeResource>()
                .SingleInstance()
                .WithCallerMetadata();


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
                    var tab = new AppRibbonTab();
                    tab.Category = ct;
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
            foreach ( var qq in typeof ( AnalysisCommands ).GetProperties (
                                                                           BindingFlags.Static
                                                                           | BindingFlags.Public
                                                                          ) )
            {
                var t = qq.PropertyType ;
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
                foreach ( var kayType in kayTypes )

                {
                    // builder.RegisterType ( kayType )
                           // .WithMetadata < TypeUsageMetadata > (
                                                                // m => m.For (
                                                                            // metadata
                                                                                // => metadata
                                                                                          // .UiConversion
                                                                                       // = true
                                                                           // )
                                                               // ) ;

                    // m.For ( tm => tm.UiConversion = true ));
                }


                //builder.RegisterGeneric(typeof(TypeServices<>));
                // builder.RegisterType <TypeConverter1> ( ).Where(a => a.As < TypeConverter > ( ) ;
                var types = new[] { typeof ( AppTypeInfo ) , typeof ( SyntaxFieldInfo ) } ;
                builder.RegisterInstance ( types )
                       .WithMetadata ( "Custom" , true )
                       .AsImplementedInterfaces ( )
                       .AsSelf ( ) ;
                builder.RegisterType < ControlsProvider > ( ).WithAttributeFiltering ( ) ;
                // .WithParameter (
                // new NamedParameter ( "types" , types )
                // ) .AsSelf (  ) ;
                builder.RegisterType < AnalysisCustomTypeDescriptor > ( )
                       .AsSelf ( )
                       .AsImplementedInterfaces ( ) ;

#if false
            builder.RegisterAssemblyTypes(ThisAssembly).Where(type => {
                                                                  var isAssignableFrom =
 typeof ( IViewModel )
                                                                                            .IsAssignableFrom (
                                                                                                               type
                                                                                                              )
                                                                                         || typeof ( IView1 )
                                                                                            .IsAssignableFrom (
                                                                                                               type
                                                                                                              ) ;
                                                                  return isAssignableFrom ;
                                                              }              ).AsImplementedInterfaces().AsSelf().WithAttributedMetadata();

#else

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

            // var names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            // foreach ( var name in names )
            // {
            //     var info = Assembly.GetExecutingAssembly ( ).GetManifestResourceInfo ( name ) ;
            //     DebugUtils.WriteLine ( info.ResourceLocation ) ;
            //
            // }

            builder.RegisterCallback((x) =>
            {
                
            });
            builder.Register((c, p)=>
            {

                var cm = c.Resolve<IEnumerable<Meta<Lazy<IBaseLibCommand>>>>();

                Dictionary<Category, Info1> dict = new Dictionary<Category, Info1>();
                foreach (var c1 in cm)
                {
                    foreach (var m1 in c1.Metadata)
                    {
                        DebugUtils.WriteLine($"{m1.Key} = {m1.Value}");
                    }
                    CommandInfo ci = new CommandInfo { Command = c1 };

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
                            Category = (Category)cat,
                        };
                        dict[cat] = i1;
                    }
                    if (group == null)
                    {
                        i1.Ungrouped.Add(ci);
                    }
                    else
                    {
                        if (!i1.Infos.TryGetValue((string)group, out var i2))
                        {

                            i2 = new Info2 { Group = (string)group };
                            i1.Infos[(string)group] = i2;
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
#endif
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
            public static Func<LayoutDocumentPane, IDisplayableAppCommand>
                ControlViewCommandAdapter(
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

    }

    public class OpenFileCommand : AppCommand
    {
        private IAnalyzeCommand analyzeCommand;

        public OpenFileCommand(IAnalyzeCommand analyzeCommand) : base("Open File")
        {
            this.analyzeCommand = analyzeCommand;
        }

        public override object Argument { get; set; }

        public override async Task<IAppCommandResult> ExecuteAsync()
        {
            var filters = new List<Filter>
            {
                new Filter {Extension = ".sln", Description = "Solution Files"},
                new Filter {Extension = ".xml", Description = "XML files"}, new Filter
                {
                    Extension = ".cs", Description = "CSharp source files",
                    // ReSharper disable once UnusedAnonymousMethodSignature
#pragma warning disable 1998
                    Handler = async delegate(string filename1)
                    {
#pragma warning restore 1998
                    }
                }
            };
            var dlg = new OpenFileDialog
            {
                DefaultExt = ".cs",
                Filter = string.Join(
                    "|"
                    , filters.Select(
                        f => $"{f.Description} (*{f.Extension})|*{f.Extension}"
                    )
                )
            };

            var result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result != true)
            {
                return AppCommandResult.Cancelled;
            }

            

                // Open document
                var filename = dlg.FileName;
                if (Path.GetExtension(filename).ToLowerInvariant() == ".sln")
                {


                    var node = new ProjectBrowserNode
                    {
                        Name = "Loaded solution",
                        SolutionPath = filename
                    };
                    DebugUtils.WriteLine("await command");
                    await analyzeCommand.AnalyzeCommandAsync(
                            node
                            , new ActionBlock<RejectedItem>(
                                x => Debug
                                    .WriteLine(
                                        x.Statement
                                            .ToString()
                                    )
                            )
                        )
                        .ConfigureAwait(false);
                    DebugUtils.WriteLine("herecommand");
                    return AppCommandResult.Success;
                }

                return AppCommandResult.Failed;
        }

        public override void OnFault(AggregateException exception)
        {
            
        }

        public override object LargeImageSourceKey { get; set; }


        public override bool CanExecute(object parameter)
        {
            return true;
        }

        
        
    }

}