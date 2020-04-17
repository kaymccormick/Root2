using System ;
using System.Collections.Generic ;
using System.IO ;
using System.Linq ;
using System.Text.Json ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Input ;
using AnalysisControls.ViewModel ;
using AnalysisControls.Views ;
using Autofac ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Serialization ;
using KayMcCormick.Lib.Wpf ;

#if ENABLE_CONSOLE
using Vanara.PInvoke ;
#endif

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : AppWindow , IDisposable
    {
        private static   MainWindow _instance ;
        private readonly TestAppApp _testAppApp ;
#if ENABLE_CONSOLE
        private Kernel32.SafeHFILE _consoleScreenBuffer ;
        private HFILE _stdHandle ;
#endif

        public MainWindow ( [ CanBeNull ] ILifetimeScope lifetimeScope ) : base ( lifetimeScope )
        {
            if ( Instance != null )
            {
                throw new InvalidOperationException ( "MainWindow already instantiated." ) ;
            }

            _instance = this ;

            
#if ENABLE_CONSOLE
            var allocConsole = Kernel32.AllocConsole ( ) ;
            if ( !allocConsole )
            {
                // var win32Error = Kernel32.GetLastError ( ) ;
                // DebugUtils.WriteLine(win32Error.ToHRESULT().Code.ToString("08x"));
            }

            _consoleScreenBuffer =
 Kernel32.CreateConsoleScreenBuffer (ACCESS_MASK.GENERIC_READ|ACCESS_MASK.GENERIC_WRITE, FileShare.ReadWrite
                                                                      ) ;
            if ( _consoleScreenBuffer.IsInvalid )
            {
                Kernel32.GetLastError().ThrowIfFailed();
            }

            Kernel32.SetConsoleActiveScreenBuffer ( _consoleScreenBuffer ) ;
            _stdHandle = Kernel32.GetStdHandle ( Kernel32.StdHandleType.STD_OUTPUT_HANDLE ) ;
            if ( _stdHandle.IsInvalid ) throw new InvalidOperationException() ;
#endif
            ApplicationInstance.LogMethodDelegate logMethod = LogMethod ;
            var config = new ApplicationInstance.ApplicationInstanceConfiguration ( logMethod , ApplicationGuid ) ;

            _testAppApp = new TestAppApp ( config ) ;
            InitializeComponent ( ) ;
        }

        public Guid ApplicationGuid { get ; } = new Guid ("50793c70-3902-4ba3-ad15-c28e2c9ca6a6");

        public static MainWindow Instance { get { return _instance ; } set { _instance = value ; } }

        public void LogMethod ( [ NotNull ] string message )
        {
            if ( string.IsNullOrEmpty ( message ) )
            {
                return ;
            }

#if ENABLE_CONSOLE
            var writeConsole = Kernel32.WriteConsole(
                                                     _consoleScreenBuffer
                                       , message
                                       , (uint)message.Length
                                       , out uint writen
                                       , IntPtr.Zero
                                                    );
            writeConsole = Kernel32.WriteConsole(
                                                     _consoleScreenBuffer
                                       , "\n\r"
                                       , 2
                                       , out  writen
                                       , IntPtr.Zero
                                                    );

            DebugUtils.WriteLine ($"{writen} bytes written to console"  );
#endif
            DebugUtils.WriteLine ( message ) ;
        }

        public MainWindow ( ) : this ( null ) { }

        #region Overrides of FrameworkElement
        public override void OnApplyTemplate ( )
        {
            var p = new PaneService ( ) ;
            var pane = p.GetPane ( ) ;
            var b = new ExceptionUserControl
                    {
                        DataContext = new ExceptionDataInfo
                                      {
                                          Exception = new InvalidOperationException ( )
                                        , ParsedExceptions = new ParsedExceptions
                                                             {
                                                                 ParsedList =
                                                                     new
                                                                     List < ParsedStackInfo >
                                                                     {
                                                                         new ParsedStackInfo
                                                                         {
                                                                             StackTraceEntries =
                                                                                 Utils
                                                                                    .ParseStackTrace (
                                                                                                      Environment
                                                                                                         .StackTrace
                                                                                                     )
                                                                                    .ToList ( )
                                                                         }
                                                                     }
                                                             }
                                      }
                    } ;

            pane.AddChild ( b ) ;
            //var x = new LayoutService ( _dockLayout.leftPane ) ;
            //x.AddToLayout ( pane ) ;
        }
        #endregion

        #region IDisposable
        public void Dispose ( ) { _testAppApp?.Dispose ( ) ; }
        #endregion

        // ReSharper disable once UnusedMember.Local
        private void UIElement_OnKeyDown ( object sender , [ NotNull ] KeyEventArgs e )
        {
            if ( e.Key == Key.Enter )
            {
                if ( sender is TextBox box )
                {
                    LogMethod ( box.Text ) ;
                }
            }
        }

#if ENABLE_CONSOLE
        [DllImport( "kernel32.dll")]
        [return: MarshalAs( UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport( "Kernel32.dll")]
        static extern IntPtr CreateConsoleScreenBuffer(
            UInt32 dwDesiredAccess,
            UInt32 dwShareMode,
            IntPtr secutiryAttributes,
            UInt32 flags,
            IntPtr screenBufferData
        );


        // http://pinvoke.net/default.aspx/kernel32/WriteConsole.html
        [DllImport( "kernel32.dll", SetLastError = true)]
        static extern bool WriteConsole(
            IntPtr   hConsoleOutput,
            string   lpBuffer,
            uint     nNumberOfCharsToWrite,
            out uint lpNumberOfCharsWritten,
            IntPtr   lpReserved
        );
#endif
        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        private void CommandBinding_OnExecuted ( object sender , ExecutedRoutedEventArgs e )
        {
            DebugUtils.WriteLine ( "Persisting" ) ;
            var typesView = ( TypesView ) sender ;
            try
            {
                var options = AnalysisControls.JsonConverters.CreateJsonSerializeOptions ( ) ;
                options.Converters.Add ( new JsonTypeConverterFactory ( ) ) ;
                options.Converters.Add ( new JsonTypeInfoConverter ( ) ) ;

                options.WriteIndented = true ;
                var json = JsonSerializer.Serialize ( typesView.ViewModel.Root, options ) ;
                File.WriteAllText ( @"C:\data\logs\viewmodel.json" , json ) ;
            }
            catch ( JsonException ex )
            {
                MessageBox.Show ( "Json failure" , ex.Message ) ;
            }
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable twice UnusedParameter.Local
        private void CommandBinding_OnExecuted2 ( object sender , ExecutedRoutedEventArgs e )
        {
            try
            {
                var options = AnalysisControls.JsonConverters.CreateJsonSerializeOptions ( ) ;
                options.Converters.Add ( new JsonTypeConverterFactory ( ) ) ;
                options.Converters.Add ( new JsonTypeInfoConverter ( ) ) ;

                options.WriteIndented = true ;
                // ReSharper disable once UnusedVariable
                var model =
                    JsonSerializer.Deserialize < TypesViewModel > (
                                                                   File.ReadAllText (
                                                                                     @"C:\data\logs\viewmodel.json"
                                                                                    )
                                                                  ) ;
            }
            catch ( JsonException ex )
            {
                MessageBox.Show ( "Json failure" , ex.Message ) ;
            }
        }
    }
}