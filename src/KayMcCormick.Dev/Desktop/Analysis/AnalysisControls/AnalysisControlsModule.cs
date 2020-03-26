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
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using AnalysisAppLib.ViewModel ;
using AnalysisControls.Views ;
using Autofac ;
using Autofac.Core ;
using KayMcCormick.Lib.Wpf ;

namespace AnalysisControls
{
    // made internal 3/11
    public class AnalysisControlsModule : Module
    {

        protected override void Load ( ContainerBuilder builder )
        {

            builder.RegisterType < TypesView > ( ).AsImplementedInterfaces ( ) ;
            builder.RegisterType < TypesViewModel > ( ).As < ITypesViewModel > ( ) ;
            // builder.RegisterType < PythonControl > ( ).AsImplementedInterfaces ( ).AsSelf ( ) ;
            builder.Register (
                              ( c , p ) => {
                                  var listView = Func ( c , p ) ;
                                  return new ContentControlView ( ) { Content = listView } ;
                              })
                   .WithMetadata ( "Title" , "Syntax Token View" )
                   .As < IControlView > ( ) ;
            // builder.RegisterType < PythonViewModel > ( ).AsSelf ( ) ;
        }

        private FrameworkElement Func ( IComponentContext c1 , IEnumerable < Parameter > p1 )
        {
            var gridView = new GridView ( ) { } ;
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
            gridView.Columns.Add ( new GridViewColumn
                                   {
                                       Header = "Raw Kind",
                                       DisplayMemberBinding = new Binding ( "RawKind" )
                                   } );

            var binding = new Binding("SyntaxItems") { Source     = c1.Resolve < ISyntaxTokenViewModel> ( ) } ;
            var listView = new ListView ( ) { View = gridView } ;
            listView.SetBinding ( ItemsControl.ItemsSourceProperty , binding ) ;
            return listView ;
        }


        // builder.RegisterAssemblyTypes ( ThisAssembly )
        // .Where (
        // type => typeof ( IViewWithTitle ).IsAssignableFrom ( type )
        // || typeof ( IViewModel ).IsAssignableFrom ( type )
        // || typeof ( AppWindow ).IsAssignableFrom ( type )
        // )
        // .AsSelf ( )
        // .AsImplementedInterfaces ( ) ;
    }

}

