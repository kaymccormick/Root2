#if ANALYSISCONTROLS
#endif
using AnalysisFramework ;
using Autofac ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;
using Microsoft.CodeAnalysis ;
using NLog ;
using System ;
using System.Collections ;
using System.Collections.Concurrent ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Globalization ;
using System.IO ;
using System.Linq ;
using System.Reactive.Concurrency ;
using System.Reactive.Linq ;
using System.Runtime.CompilerServices ;
using System.Text ;
using System.Text.Json ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Input ;
using System.Windows.Threading ;
using AnalysisControls.Views ;
using KayMcCormick.Dev.Logging ;
using ProjLib.Interfaces ;
using Task = System.Threading.Tasks.Task ;

namespace ProjInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [ TitleMetadata ( "Main window" ) ]
    public partial class ProjMainWindow : AppWindow
      , IView < IWorkspacesViewModel >
      , IView1
      , IWorkspacesView
      , INotifyPropertyChanged
    {
        private readonly        ILifetimeScope _scope ;
        private static readonly Logger         Logger = LogManager.GetCurrentClassLogger ( ) ;

        // ReSharper disable once NotAccessedField.Local
        private TaskFactory _factory ;

        public IWorkspacesViewModel ViewModel
        {
            get { return _viewModel ; }
            set
            {
                _viewModel = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public ProjMainWindow ( ) : base ( null )
        {
            InitializeComponent ( ) ;
            _factory = new TaskFactory ( TaskScheduler.FromCurrentSynchronizationContext ( ) ) ;
        }

        public ProjMainWindow ( IWorkspacesViewModel viewModel , ILifetimeScope scope )
        {
            _scope = scope ;
            SetValue ( AttachedProperties.LifetimeScopeProperty , scope ) ;
            InitializeComponent ( ) ;
            _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext ( ) ;

            ViewModel = viewModel ;
            _factory  = new TaskFactory ( _taskScheduler ) ;

            var myCacheTarget2 = AppLoggingConfigHelper.CacheTarget2 ;
            myCacheTarget2?.Cache.SubscribeOn ( Scheduler.Default )
                           .Buffer ( TimeSpan.FromMilliseconds ( 100 ) )
                           .Where ( x => x.Any ( ) )
                           .ObserveOnDispatcher ( DispatcherPriority.Background )
                           .Subscribe (
                                       infos => {
                                           foreach ( var json in infos )
                                           {
                                               var i = JsonSerializer
                                                  .Deserialize < LogEventInstance > (
                                                                                     json
                                                                                   , new
                                                                                         JsonSerializerOptions ( )
                                                                                    ) ;
                                               if ( i.Properties != null )
                                               {
                                                   foreach ( var p in i.Properties )
                                                   {
                                                       var g = _eventView.View as GridView ;
                                                       if ( ! PropertiesColumns.ContainsKey (
                                                                                             p.Key
                                                                                            ) )
                                                       {
                                                           var gridViewColumn =
                                                               new GridViewColumn ( )
                                                               {
                                                                   Header = p.Key
                                                                 , DisplayMemberBinding =
                                                                       new Binding ( )
                                                                       {
                                                                           Converter =
                                                                               _propConverter
                                                                         , ConverterParameter =
                                                                               p.Key
                                                                       }
                                                               } ;
                                                           PropertiesColumns[ p.Key ] =
                                                               gridViewColumn ;
                                                           if ( g != null )
                                                           {
                                                               g.Columns.Add ( gridViewColumn ) ;
                                                           }

                                                           //               _eventView.UpdateLayout();
                                                       }
                                                   }
                                               }

                                               ViewModel.Events.Add ( i ) ;
                                           }
                                       }
                                      ) ;


            var myCacheTarget = MyCacheTarget.GetInstance ( 1000 ) ;
            myCacheTarget.Cache.SubscribeOn ( Scheduler.Default )
                         .Buffer ( TimeSpan.FromMilliseconds ( 100 ) )
                         .Where ( x => x.Any ( ) )
                         .ObserveOnDispatcher ( DispatcherPriority.Background )
                         .Subscribe (
                                     infos => {
                                         foreach ( var logEventInfo in infos )
                                         {
                                             ViewModel.EventInfos.Add ( logEventInfo ) ;
                                         }
                                     }
                                    ) ;
        }

        public Dictionary < string , GridViewColumn > PropertiesColumns { get ; set ; } =
            new Dictionary < string , GridViewColumn > ( ) ;


        protected override void OnInitialized ( EventArgs e )
        {
            base.OnInitialized ( e ) ;
            Logger.Info ( "{methodName} {typeEvent}" , nameof ( OnInitialized ) , e.GetType ( ) ) ;
            // Scope = Container.GetContainer();
            // ViewModel = Scope.Resolve<IWorkspacesViewModel>();
            // ViewModel.BeginInit();
        }

        private IWorkspacesViewModel _viewModel ;

        public event RoutedEventHandler TaskCompleted
        {
            add { AddHandler ( TaskCompleteEvent , value ) ; }
            remove { RemoveHandler ( TaskCompleteEvent , value ) ; }
        }

        public static RoutedEvent TaskCompleteEvent =
            EventManager.RegisterRoutedEvent (
                                              "task completed"
                                            , RoutingStrategy.Direct
                                            , typeof ( RoutedEventHandler )
                                            , typeof ( ProjMainWindow )
                                             ) ;

        private ConcurrentBag < Task > _tasks = new ConcurrentBag < Task > ( ) ;
        private TaskScheduler          _taskScheduler ;

        private ObservableCollection < TaskWrap > _obsTasks =
            new ObservableCollection < TaskWrap > ( ) ;

        private IValueConverter _propConverter = new PropertyConverter ( ) ;

        private void CommandBinding_OnExecuted2 ( object sender , ExecutedRoutedEventArgs e ) { }

        private async void CommandBinding_OnExecuted ( object sender , ExecutedRoutedEventArgs e )
        {
            
            if ( e.OriginalSource is ListView )
            {
                var projectBrowser = (ProjectBrowser)FindName("_projectBrowser");
                if ( projectBrowser == null )
                {
                    return ;
                }
                var v = CollectionViewSource.GetDefaultView (
                                                             projectBrowser.ViewModel.RootCollection
                                                            ) ;
                if ( v == null )
                {
                    throw new InvalidOperationException (
                                                         "Unable to retrieve default view for project browser."
                                                        ) ;
                }

                var viewCurrentItem = v.CurrentItem ;
                Logger.Debug ( "Running analysis on {project}" , viewCurrentItem ) ;
                await ViewModel.AnalyzeCommand ( viewCurrentItem ).ConfigureAwait(true);
                var jsonSerializerOptions = new JsonSerializerOptions ( ) ;
                jsonSerializerOptions.Converters.Add ( new LogInvocationConverter ( ) ) ;
                File.WriteAllText (
                                   "invocations.json"
                                 , JsonSerializer.Serialize (
                                                             ViewModel.LogInvocations.ToList ( )
                                                           , jsonSerializerOptions
                                                            )
                                  ) ;
            }
            else
            {
                const string path =
                    @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\Root2\src\KayMcCormick.Dev\KayMcCormick.Dev\Logging\AppLoggingConfigHelper.cs" ;
                ISemanticModelContext c = AnalysisService.Parse (
                                                                 File.ReadAllText ( path )
                                                               , "test"
                                                                ) ;
                ViewModel.Workspace = new AdhocWorkspace ( ) ;

                var projectId = ProjectId.CreateNewId ( "Test" ) ;
                ViewModel.Workspace.AddProject (
                                                ProjectInfo.Create (
                                                                    projectId
                                                                  , VersionStamp.Create ( )
                                                                  , "test project"
                                                                  , "tesst"
                                                                  , LanguageNames.CSharp
                                                                   )
                                               ) ;
                ViewModel.Workspace.AddDocument (
                                                 DocumentInfo.Create (
                                                                      DocumentId.CreateNewId (
                                                                                              projectId
                                                                                            , "test"
                                                                                             )
                                                                    , "test"
                                                                    , null
                                                                    , SourceCodeKind.Regular
                                                                    , new FileTextLoader (
                                                                                          path
                                                                                        , Encoding
                                                                                             .UTF8
                                                                                         )
                                                                    , path
                                                                     )
                                                ) ;
                ViewModel.Workspace.CurrentSolution.Projects.First ( )
                         .Documents.First ( )
                         .GetSemanticModelAsync ( )
                         .ContinueWith (
                                        ( task ) => {
                                            var w = _scope.Resolve < ICompilationView > (
                                                                                         new
                                                                                             TypedParameter (
                                                                                                             typeof
                                                                                                             ( ICodeAnalyseContext
                                                                                                             )
                                                                                                           , new
                                                                                                                 CodeAnalyseContext2 (
                                                                                                                                      task
                                                                                                                                         .Result
                                                                                                                                     )
                                                                                                            )
                                                                                        ) ;
                                            w.Show ( ) ;
                                        }
                                      , TaskScheduler.FromCurrentSynchronizationContext ( )
                                       ) ;
            }
        }

        private void AddTask ( Task task , TaskWrap tw )
        {
            Logger.Trace ( "Adding task {desc}" , tw?.Desc ?? "null" ) ;
            _obsTasks.Add ( tw ) ;
            task.ContinueWith (
                               delegate ( Task t ) {
                                   if ( tw != null )
                                   {
                                       tw.Status = t.Status.ToString ( ) ;
                                   }

                                   //ObsTasks.Remove ( t ) ;
                               }
                             , _taskScheduler
                              ) ;
            _tasks.Add ( task ) ;
        }

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        private void DataGrid_OnAutoGeneratingColumn (
            object                                sender
          , DataGridAutoGeneratingColumnEventArgs e
        )
        {
            switch ( e.PropertyName )
            {
                case "StackTrace" :
                case "MessageTemplateParameters" :
                case "HasStackTrace" :
                case "UserStackFrame" :
                case "UserStackFrameNumber" :
                case "CallerClassName" :
                case "CallerMemberName" :
                case "CallerFilePath" :
                case "CallerLineNumber" :
                case "LoggerShortName" :
                case "MessageFormatter" :
                case "Message" :
                case "HasProperties" :

                    e.Cancel = true ;
                    break ;
            }
        }

        #region Implementation of IView1
        public string ViewTitle { get { return "Main View" ; } }

        public ObservableCollection < TaskWrap > ObsTasks
        {
            get { return _obsTasks ; }
            set { _obsTasks = value ; }
        }
        #endregion

        private void _projectBrowser_OnSelected ( object sender , RoutedEventArgs e )
        {
            Logger.Info ( "selected {i}" , sender.ToString ( ) ) ;
        }
    }

    internal interface ICompilationView
    {
        void Show ( ) ;
    }

    internal class PropertyConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            var instance = value as LogEventInstance ;
            if ( instance               != null
                 && instance.Properties != null
                 && instance.Properties.TryGetValue (
                                                     ( string ) parameter
                                                     ?? throw new InvalidOperationException ( )
                                                   , out var elem
                                                    ) )
            {
                var process = Process ( ( JsonElement ) elem ) ;
                if ( targetType == typeof ( string ) )
                {
                    var convertToString = ConvertToString ( process ) ;
                    if ( convertToString.Length > 80 )
                    {
                        return convertToString.Substring ( 0 , 80 ) ;
                    }

                    return convertToString ;
                }

                return process ;
            }

            return null ;
        }

        private string ConvertToString ( object process )
        {
            if ( process is IDictionary d )
            {
                var strings = new List < string > ( ) ;
                foreach ( var dKey in d.Keys )
                {
                    strings.Add ( dKey + ": " + ConvertToString ( d[ dKey ] ) ) ;
                }

                return string.Join ( ", " , strings ) ;
            }

            if ( process is IEnumerable ie
                 && process.GetType ( ) != typeof ( string ) )
            {
                var strings = new List < string > ( ) ;
                foreach ( var dKey in ie )
                {
                    strings.Add ( ConvertToString ( dKey ) ) ;
                }

                return string.Join ( ", " , strings ) ;
            }

            return process.ToString ( ) ;
        }

        private object Process ( JsonElement elem )
        {
            switch ( elem.ValueKind )
            {
                case JsonValueKind.Undefined : return null ;
                case JsonValueKind.Object :
                    try
                    {
                        var dd = new Dictionary < string , object > ( ) ;
                        foreach ( var q in elem.EnumerateObject ( ) )
                        {
                            if ( dd.ContainsKey ( q.Name ) )
                            {
                                System.Diagnostics.Debug.WriteLine (
                                                                    "Uh oh key exists " + q.Name
                                                                   ) ;
                            }
                            else
                            {
                                dd[ q.Name ] = Process ( q.Value ) ;
                            }
                        }

                        return dd ;
                        // .ToDictionary ( property => property.Name , property
                        // => Process ( property.Value )) ;
                    }
                    catch ( Exception ex )
                    {
                        return ex.Message ;
                    }

#pragma warning disable CS0162 // Unreachable code detected
                    return "test" ;
#pragma warning restore CS0162 // Unreachable code detected
                case JsonValueKind.Array :  return elem.EnumerateArray ( ).Select ( Process ) ;
                case JsonValueKind.String : return elem.GetString ( ) ;
                case JsonValueKind.Number : return elem.GetInt32 ( ) ;
                case JsonValueKind.True :   return true ;
                case JsonValueKind.False :  return false ;
                case JsonValueKind.Null :   return null ;
                default :                   throw new ArgumentOutOfRangeException ( ) ;
            }
        }


        public object ConvertBack (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            return null ;
        }
        #endregion
    }

    public class TaskWrap : INotifyPropertyChanged
    {
        private string _status ;

        public Task Task { get ; }

        public string Desc { get ; }

        public string Status
        {
            get { return _status ; }
            set
            {
                _status = value ;
                OnPropertyChanged ( ) ;
                OnPropertyChanged ( "Task" ) ;
            }
        }

        public TaskWrap ( Task task , string desc )
        {
            Task = task ;
            Desc = desc ;
        }

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}