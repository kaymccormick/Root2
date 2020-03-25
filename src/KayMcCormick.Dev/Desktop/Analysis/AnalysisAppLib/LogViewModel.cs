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
using System.Runtime.Serialization ;
using System.Threading ;
using AnalysisAppLib.ViewModel ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;

namespace AnalysisAppLib
{
    public sealed class LogViewModel : INotifyPropertyChanged, IViewModel
    {
        private LogEventInstanceObservableCollection _logEntries ;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private ObservableCollection < ViewerLoggerInfo > rootNodes ;
        private IDictionary < string , ViewerLoggerInfo > _dict ;
        private ViewerLoggerInfo                          rootLogger ;
        private SynchronizationContext                    _context ;
        private string                                    _displayName ;

        public LogViewModel ( )
        {
            _context = SynchronizationContext.Current ;
            rootLogger = new ViewerLoggerInfo ( )
                         {
                             LoggerName = "" , PartName = "" , DisplayName = "Root logger"
                         } ;
            _logEntries = new LogEventInstanceObservableCollection ( ) ;
            _dict       = new Dictionary < string , ViewerLoggerInfo > ( ) ;
            rootNodes   = new ObservableCollection < ViewerLoggerInfo > ( ) ;

            rootNodes.Add ( rootLogger ) ;

            _dict[ "" ] = rootLogger ;
        }

        public ViewerLoggerInfo RootLogger
        {
            get { return rootLogger ; }
            set { rootLogger = value ; }
        }

        public ObservableCollection < ViewerLoggerInfo > RootNodes { get { return rootNodes ; } }

        public LogEventInstanceObservableCollection LogEntries { get { return _logEntries ; } }

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
            var i = 0 ;
            var logger = rootLogger ;
            var loggerName1 = "" ;
            for ( i = 0 ; i < strings.Length ; i ++ )
            {
                loggerName1 = loggerName1 + strings[ i ] ;
                if ( ! logger.ChildrenLoggers.TryGetValue ( strings[ i ] , out var child ) )
                {
                    child = new ViewerLoggerInfo ( )
                            {
                                LoggerName  = loggerName
                              , PartName    = strings[ i ]
                              , DisplayName = strings[ i ]
                            } ;
                    logger.ChildrenLoggers[ strings[ i ] ] = child ;
                    logger.Children.Add ( child ) ;
                }

                logger      = child ;
                loggerName1 = loggerName1 + "." ;
            }
        }

        private ViewerLoggerInfo CheckForLogger ( string loggerName )
        {
            if ( _dict.ContainsKey ( loggerName ) )
            {
                return _dict[ loggerName ] ;
            }

            return null ;
        }

        public void AddEntry ( LogEventInstance logEvent )
        {
            //var loggers = logEvent.LoggerName.Split ( '.' ) ;

            _context.Post (
                           ( state ) => {
                               var logEventLoggerName = logEvent.LoggerName ;
                               ParseLoggerName ( logEventLoggerName ) ;
                               _logEntries.Add ( logEvent ) ;
                           }
                         , null
                          ) ;
        }

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        #region Implementation of ISerializable
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }
}