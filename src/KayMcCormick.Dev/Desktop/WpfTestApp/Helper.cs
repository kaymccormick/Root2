using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Windows ;
using AnalysisControls ;
using Autofac ;
using AvalonDock ;
using AvalonDock.Layout ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;
using NLog ;
using ProjInterface ;

namespace WpfTestApp
{
    public static class Helper
    {
        public static Window Doit ( )
        {
            var instance = new ApplicationInstance ( new ApplicationInstanceConfiguration ( message => Debug.WriteLine ( message ) ) ) ;
            instance.AddModule ( new ProjInterfaceModule ( ) ) ;
            instance.AddModule ( new AnalysisControlsModule ( ) ) ;
            instance.Initialize ( ) ;
            var lifetimescope = instance.GetLifetimeScope ( ) ;
            LogManager.ThrowExceptions = true ;
            var funcs = lifetimescope
               .Resolve < IEnumerable < Func < LayoutDocumentPane , IDisplayableAppCommand > >
                > ( ) ;


            var m = new DockingManager ( ) ;

            var pane = new LayoutDocumentPane ( ) ;

            var group = new LayoutDocumentPaneGroup ( pane ) ;

            var mLayoutRootPanel = new LayoutPanel ( group ) ;
            var layout = new LayoutRoot { RootPanel = mLayoutRootPanel } ;
            m.Layout = layout ;
            Window w = new AppWindow ( lifetimescope ) ;
            w.Content = m ;

            foreach ( var func in funcs )
            {
                try
                {
                    Debug.WriteLine ( "func is {func}" ) ;
                    var xx = func ( pane ) ;
                    Debug.WriteLine ( xx.DisplayName ) ;
                    xx.ExecuteAsync ( )
                      .ContinueWith (
                                     task => {
                                         if ( task.IsFaulted )
                                         {
                                             Debug.WriteLine ( task.Exception ) ;
                                         }
                                         else
                                         {
                                             Debug.WriteLine ( task.Result ) ;
                                         }
                                     }
                                    )
                      .Wait ( ) ;
                }
                catch ( Exception ex )
                {
                    Debug.WriteLine ( ex.ToString ( ) ) ;
                }
            }

            // x.TCS = source;
            // x.Run(w);
            // Task.WaitAll(x.TCS.Task);
            // Debug.WriteLine(source.Task.Result);
            return w ;
        }
    }
}

