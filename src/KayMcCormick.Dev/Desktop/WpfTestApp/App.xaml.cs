using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO ;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup ;

namespace WpfTestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Overrides of Application
        protected override void OnStartup ( StartupEventArgs e )
        {
            base.OnStartup ( e ) ;
            var w = Helper.Doit ( ) ;
            w.ShowDialog ( ) ;
            EventManager.RegisterClassHandler(typeof(Window), Window.LoadedEvent, new RoutedEventHandler(Target));
        }

        private void Target ( object sender , RoutedEventArgs e )
        {
            // using (var f = new StreamWriter(@"C:\data\logs\stream.txt"))
            // {
                // XamlWriter.Save(sender, f);
            // }
        }
        #endregion
    }
}
