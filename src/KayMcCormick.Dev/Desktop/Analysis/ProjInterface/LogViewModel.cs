#region header
// Kay McCormick (mccor)
// 
// LogViewer1
// LogViewer1
// LogViewModel.cs
// 
// 2020-03-16-12:02 AM
// 
// ---
#endregion
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Runtime.CompilerServices ;
using System.Threading ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;


namespace LogViewer1
{
    public class LogViewModel : INotifyPropertyChanged
    {
        private LogEventInstanceCollection _logEntries ;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private ObservableCollection <LoggerInfo> rootNodes ;
        private IDictionary<string, LoggerInfo>   _dict ;
        private LoggerInfo rootLogger ;
        private SynchronizationContext _context ;
        private string _displayName ;

        public LogViewModel ( ) {
            _context = SynchronizationContext.Current ;
            rootLogger = new LoggerInfo() { LoggerName = "", PartName = "", DisplayName = "Root logger"};
            _logEntries = new LogEventInstanceCollection ( ) ;
            _dict = new Dictionary < string , LoggerInfo> () ;
            rootNodes = new ObservableCollection < LoggerInfo> () ;
            // var treeNode = new TreeNode() { DisplayName = "Root logger", HierarchyName = ""} ;
            rootNodes.Add (rootLogger);
            // rootLogger.TreeNode = treeNode ;
            _dict[ "" ] = rootLogger ;
        }

        public LoggerInfo RootLogger { get => rootLogger ; set => rootLogger = value ; }

        public ObservableCollection < LoggerInfo > RootNodes
        {
            get => rootNodes ;
        }

        public LogEventInstanceCollection  LogEntries
        {
            get => _logEntries ;
        }

        public string DisplayName { get { return _displayName ; } set { _displayName = value ; } }

        public void ParseLoggerName ( string loggerName )
        {
            //Regex x = new Regex(@"\.[^\.]*$", RegexOptions.Compiled);
            //var m = x.Match(LoggerName);
            
                        RegisterLogger ( loggerName ) ;
        }

        private void RegisterLogger ( string loggerName )
        {
            var strings = loggerName.Split ( '.' ) ;
            int i = 0 ;
            LoggerInfo logger = rootLogger ;
            var loggerName1 = "" ;
            for ( i = 0 ; i < strings.Length ; i ++ )
            {
                loggerName1 = loggerName1 + strings[ i ] ;
                if(!logger.ChildrenLoggers.TryGetValue ( strings[ i ] , out var child ))
                {
                    child = new LoggerInfo ( ) { LoggerName = loggerName, PartName = strings[i], DisplayName = strings[i] } ; 
                    logger.ChildrenLoggers[ strings[ i ] ] = child ;
                    logger.Children.Add ( child ) ;
                }

                logger = child ;
                loggerName1 = loggerName1 + "." ;
            }

        }

        private LoggerInfo CheckForLogger ( string loggerName )
        {
            if(_dict.ContainsKey ( loggerName ))
            {
                return _dict[ loggerName ] ;

            }

            return null ;
        }

        public void  AddEntry ( LogEventInstance logEvent )
        {
            //var loggers = logEvent.LoggerName.Split ( '.' ) ;

            _context.Post (
                           ( state) => {

                               var logEventLoggerName = logEvent.LoggerName ;
                               ParseLoggerName ( logEventLoggerName ) ;
                               _logEntries.Add ( logEvent ) ;
                           }, null
                          ) ;

        }

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}