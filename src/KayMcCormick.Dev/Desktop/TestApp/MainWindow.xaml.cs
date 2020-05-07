using System ;
using System.Collections.Generic ;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO ;
using System.Linq ;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data;
using System.Windows.Input ;
using AnalysisAppLib;
using AnalysisControls;
using AnalysisControls.ViewModel ;
using AnalysisControls.Views ;
using Autofac ;
using DynamicData;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Serialization ;
using KayMcCormick.Lib.Wpf ;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Internal;

#if ENABLE_CONSOLE
using Vanara.PInvoke ;
#endif

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : AppWindow , IDisposable, INotifyPropertyChanged
    {
        private static   MainWindow _instance ;
        private readonly TestAppApp _testAppApp ;
        private ICollectionView _typeViewSource;
        private ICollectionView _assemblyCollectionView;
        private ListCollectionView _assemblyListCollectionView;
        private ObservableCollection<Assembly> _assemblyCollection = new ObservableCollection<Assembly>();
        private List<NamespaceNode> _namespaceNodesRoot;
        private Type _type;
#if ENABLE_CONSOLE
        private Kernel32.SafeHFILE _consoleScreenBuffer ;
        private HFILE _stdHandle ;
#endif

        public MainWindow ( [ CanBeNull ] ILifetimeScope lifetimeScope ) : base ( lifetimeScope )
        {

            var xx = AnalysisService.Load(@"C:\temp\Program.cs", "xx");
            if ( Instance != null )
            {
                throw new InvalidOperationException ( "MainWindow already instantiated." ) ;
            }

            _instance = this ;

            foreach (var referencedAssembly in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                Assembly.Load(referencedAssembly.FullName);
            }
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

            void take(Type t)
            {
                Types.Add(t);
            }

            take(typeof(object));

            foreach (var exportedType in typeof(object).Assembly.GetExportedTypes())
            {
                Types.Add(exportedType);
            }
            _testAppApp = new TestAppApp ( config ) ;
            InitializeComponent ( ) ;
            CustomControl2.TypeInfoProvider =
                new TypeInfoProvider2((ITypeSymbol) xx.Compilation.GetSymbolsWithName(x => true, SymbolFilter.Type).First());
            NamespaceNodesRoot = AssemblyInfoConverter.CreateNamespaceNodes(Assembly.GetExecutingAssembly());
        }

        public List<NamespaceNode> NamespaceNodesRoot
        {
            get { return _namespaceNodesRoot; }
            set
            {
                if (Equals(value, _namespaceNodesRoot)) return;
                _namespaceNodesRoot = value;
                OnPropertyChanged();
            }
        }

        public Guid ApplicationGuid { get ; } = new Guid ("50793c70-3902-4ba3-ad15-c28e2c9ca6a6");

        public static MainWindow Instance { get { return _instance ; } set { _instance = value ; } }

        public ObservableCollection<Type> Types { get; } = new ObservableCollection<Type>();

        public ICollectionView TypeViewSource
        {
            get
            {
                if (_typeViewSource == null)
                {
                    var s = CollectionViewSource.GetDefaultView(Types);
                    s.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                    TypeViewSource = s;

                }
                return _typeViewSource;
            }
            set
            {
                if (Equals(value, _typeViewSource)) return;
                _typeViewSource = value;
                OnPropertyChanged();
            }
        }

        public ICollectionView AssemblyCollectionView
        {
            get
            {
                if (_assemblyCollectionView == null)
                {
                    DebugUtils.WriteLine("Creating assembly colleciton view");
                    var v = CollectionViewSource.GetDefaultView(AssemblyCollection);
                    v.CurrentChanged += V_CurrentChanged;
                    v.CollectionChanged += AssemblyCollectionViewOnCollectionChanged;
                    _assemblyListCollectionView = v as ListCollectionView;
                    AssemblyCollectionView = v;
                    v.Filter += Filter;
                    AssemblyFilter.PropertyChanged += (sender, args) =>
                    {
                        v.Refresh();
                    };

                }
                DebugUtils.WriteLine("*** " + nameof(AssemblyCollectionView) + ": Returning " + _assemblyListCollectionView);
                return _assemblyCollectionView;
            }
            set
            {
                if (Equals(value, _assemblyCollectionView)) return;
                _assemblyCollectionView = value;
                OnPropertyChanged();
            }
        }

        private bool Filter(object obj)
        {
            Assembly a = obj as Assembly;
            if (AssemblyFilter.GacOnly && a.GlobalAssemblyCache == false)
            {
                return false;
            }

            return true;
        }

        private void V_CurrentChanged(object sender, EventArgs e)
        {
            DebugUtils.WriteLine(AssemblyCollectionView.CurrentItem.ToString());
        }

        private void AssemblyCollectionViewOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            DebugUtils.WriteLine($"{nameof(AssemblyCollectionOnCollectionChanged)} - {e.Action}");
            var items = new List<Assembly>();
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var eNewItem in e.NewItems)
                    {
                        items.Add((Assembly)eNewItem);
                    }
                    DebugUtils.WriteLine("*** Adding " + String.Join(", ", items.Select(x => x.FullName)));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    DebugUtils.WriteLine("Reset");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            
            
        }

        public ObservableCollection<Assembly> AssemblyCollection { get; } = new ObservableLoadedAssembliesCollection();

        public AssemblyFilter AssemblyFilter { get; } = new AssemblyFilter();

        //
        // {
        //     get
        //     {
        //         if (_assemblyCollection == null)
        //         {
        //             var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        //             _assemblyCollection = new ObservableCollection<Assembly>(assemblies);
        //             _assemblyCollection.CollectionChanged += AssemblyCollectionOnCollectionChanged;
        //             foreach (var assembly in assemblies)
        //             {
        //                 _assemblyCollection.Add(assembly);
        //             }
        //         }
        //         return _assemblyCollection;
        //     }
        // }
        //
        private void AssemblyCollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            DebugUtils.WriteLine($"{nameof(AssemblyCollectionOnCollectionChanged)} - {e.Action}");
            var items = new List<Assembly>();
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var eNewItem in e.NewItems)
                    {
                        items.Add((Assembly)eNewItem);
                    }
                    DebugUtils.WriteLine(nameof(AssemblyCollectionOnCollectionChanged) + ": *** Adding " + String.Join(", ", items.Select(x => x.FullName)));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    DebugUtils.WriteLine("Reset");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

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

            // foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            // {
            //     AssemblyCollection.Add(assembly);
            // }
            
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void FrameworkElement_OnSourceUpdated(object sender, DataTransferEventArgs e)
        {
            DebugUtils.WriteLine("updated");
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DebugUtils.WriteLine(e.NewValue?.ToString());
        }

        private void typesel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DebugUtils.WriteLine(nameof(DataContextChanged));
            DebugUtils.WriteLine(e.NewValue?.ToString());
        }

#if false
        public static IEnumerable<AssemblyName> GetGacAssemblyFullNames()
        {
            IApplicationContext applicationContext;
            IAssemblyEnum assemblyEnum;
            IAssemblyName assemblyName;

            Fusion.CreateAssemblyEnum(out assemblyEnum, null, null, 2, 0);
            while (assemblyEnum.GetNextAssembly(out applicationContext, out assemblyName, 0) == 0)
            {
                uint nChars = 0;
                assemblyName.GetDisplayName(null, ref nChars, 0);

                StringBuilder name = new StringBuilder((int)nChars);
                assemblyName.GetDisplayName(name, ref nChars, 0);

                AssemblyName a = null;
                try
                {
                    a = new AssemblyName(name.ToString());
                }
                catch (Exception)
                {
                }

                if (a != null)
                {
                    yield return a;
                }
            }
        }
#endif


        public Type Type
        {
            get { return _type; }
            set
            {
                if (Equals(value, _type)) return;
                _type = value;
                OnPropertyChanged();
            }
        }

        private void Typesel_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Any())
            {
                var node = e.AddedItems[0] as NamespaceNode;
                if (node != null)
                {
                    Type = node.Entity;
                }
            }

        }
    }

    public class AssemblyFilter : INotifyPropertyChanged
    {
        private bool _gacOnly;

        public bool GacOnly
        {
            get { return _gacOnly; }
            set
            {
                if (value == _gacOnly) return;
                _gacOnly = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


