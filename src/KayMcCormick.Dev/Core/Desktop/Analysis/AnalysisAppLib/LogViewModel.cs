﻿#region header
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
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Logging ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LogViewModel : INotifyPropertyChanged , IViewModel
    {
        private readonly SynchronizationContext                    _context ;
        private readonly IDictionary < string , ViewerLoggerInfo > _dict ;
        private          string                                    _displayName ;


        /// <summary>
        /// 
        /// </summary>
        public LogViewModel ( )
        {
            _context = SynchronizationContext.Current ;
            RootLogger = new ViewerLoggerInfo
                         {
                             LoggerName = "" , PartName = "" , DisplayName = "Root logger"
                         } ;
            LogEntries = new LogEventInstanceObservableCollection ( ) ;
            _dict      = new Dictionary < string , ViewerLoggerInfo > ( ) ;
            RootNodes  = new ObservableCollection < ViewerLoggerInfo > ( ) ;

            RootNodes.Add ( RootLogger ) ;

            _dict[ "" ] = RootLogger ;
        }

        /// <summary>
        /// 
        /// </summary>
        public ViewerLoggerInfo RootLogger { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection < ViewerLoggerInfo > RootNodes { get ; }

        /// <summary>
        /// 
        /// </summary>
        public LogEventInstanceObservableCollection LogEntries { get ; }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get { return _displayName ; } set { _displayName = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged ;

        #region Implementation of ISerializable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerName"></param>
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
            var logger = RootLogger ;
            var loggerName1 = "" ;
            for ( i = 0 ; i < strings.Length ; i ++ )
            {
                loggerName1 = loggerName1 + strings[ i ] ;
                if ( ! logger.ChildrenLoggers.TryGetValue ( strings[ i ] , out var child ) )
                {
                    child = new ViewerLoggerInfo
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logEvent"></param>
        public void AddEntry ( LogEventInstance logEvent )
        {
            //var loggers = logEvent.LoggerName.Split ( '.' ) ;

            _context.Post (
                           state => {
                               var logEventLoggerName = logEvent.LoggerName ;
                               ParseLoggerName ( logEventLoggerName ) ;
                               LogEntries.Add ( logEvent ) ;
                           }
                         , null
                          ) ;
        }

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}