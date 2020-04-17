using System;
using System.Runtime.ExceptionServices ;
using System.Windows;
using System.Windows.Threading ;
using JetBrains.Annotations ;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public sealed partial class App : Application
    {
        public App ( ) {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomainOnFirstChanceException;
            DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private void OnDispatcherUnhandledException (
            object                                sender
          , [ NotNull ] DispatcherUnhandledExceptionEventArgs e
        )
        {
            if ( e == null )
            {
                throw new ArgumentNullException ( nameof ( e ) ) ;
            }

            if ( ! ( e.Exception is TypeLoadException ) )
            {
                return ;
            }

            e.Handled = true ;
            TestApp.MainWindow.Instance?.LogMethod ( "Handled:" ) ;
            TestApp.MainWindow.Instance?.LogMethod(e.ToString());
        }

        private void CurrentDomainOnFirstChanceException (
            object                        sender
          , [ NotNull ] FirstChanceExceptionEventArgs e
        )
        {
            if ( e.Exception is TypeLoadException )
            {
            }
        }
    }
}
