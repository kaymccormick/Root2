using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.ExceptionServices ;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading ;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App ( ) {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomainOnFirstChanceException;
            DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private void OnDispatcherUnhandledException (
            object                                sender
          , DispatcherUnhandledExceptionEventArgs e
        )
        {
            if ( e.Exception is TypeLoadException )
            {
                e.Handled = true ;
                TestApp.MainWindow.Instance?.LogMethod ( "Handled:" ) ;
                TestApp.MainWindow.Instance?.LogMethod(e.ToString());
            }
        }

        private void CurrentDomainOnFirstChanceException (
            object                        sender
          , FirstChanceExceptionEventArgs e
        )
        {
            if ( e.Exception is TypeLoadException )
            {
            }
        }
    }
}
