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
using System.Collections.Generic ;
using System.Reflection ;
using System.Text.Json ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Markup ;
using AnalysisAppLib.ViewModel ;
using AnalysisControls.ViewModel ;
using AnalysisControls.Views ;
using Autofac ;
using Autofac.Core ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Container ;
using KayMcCormick.Lib.Wpf ;
using KayMcCormick.Lib.Wpf.Command ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
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
                                                                       ) => new LambdaAppCommand (
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
                                                                      ).WithCallerMetadata();
            builder.RegisterType < TypesView > ( )
                   .AsSelf ( )
                   .As < IControlView > ( )
                   .WithMetadata (
                                  "ImageSource"
                                , "pack://application:,,,/KayMcCormick.Lib.Wpf;component/Assets/StatusAnnotations_Help_and_inconclusive_32xMD_color.png"
                                 )
                   .WithMetadata ( "Ribbon" , true ).WithCallerMetadata();



            //
            // var names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            // foreach ( var name in names )
            // {
            //     var info = Assembly.GetExecutingAssembly ( ).GetManifestResourceInfo ( name ) ;
            //     DebugUtils.WriteLine ( info.ResourceLocation ) ;
            //
            // }

            builder.Register (
                              ( context , parameters ) => {
                                  try
                                  {
                                      if ( parameters.TypedAs < bool > ( ) == false )
                                      {
                                          return new TypesViewModel (context.Resolve<JsonSerializerOptions>()) ;
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
                                          return new TypesViewModel (context.Resolve<JsonSerializerOptions>()) ;
                                      }

                                      try
                                      {
                                          var v = ( TypesViewModel ) XamlReader.Load ( stream ) ;
                                          stream.Close ( ) ;
                                          return v ;
                                      }
                                      catch ( Exception )
                                      {
                                          return new TypesViewModel (context.Resolve<JsonSerializerOptions>() ) ;
                                      }
                                  }
                              }
                             )
                   .AsSelf ( )
                   .AsImplementedInterfaces ( ).WithCallerMetadata();


            builder.RegisterType < SyntaxPanel > ( )
                   .Keyed < IControlView > ( typeof ( CompilationUnitSyntax ) )
                   .AsSelf ( ).WithCallerMetadata();
            builder.RegisterType < SyntaxPanelViewModel > ( )
                   .AsImplementedInterfaces ( )
                   .AsSelf ( ).WithCallerMetadata();
#endif
        }

        [ NotNull ]
        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        private FrameworkElement Func (
            [ NotNull ] IComponentContext c1
            // ReSharper disable once UnusedParameter.Local
          , IEnumerable < Parameter >     p1
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

            var binding =
                new Binding ( "SyntaxItems" )
                {
                    Source = c1.Resolve < ISyntaxTokenViewModel > ( )
                } ;
            var listView = new ListView { View = gridView } ;
            listView.SetBinding ( ItemsControl.ItemsSourceProperty , binding ) ;
            return listView ;
        }
    }
}