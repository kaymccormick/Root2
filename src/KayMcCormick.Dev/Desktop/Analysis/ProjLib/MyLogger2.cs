#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// MyLogger2.cs
// 
// 2020-03-04-10:52 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Linq ;
using System.Reflection ;
using Microsoft.Build.Framework ;
using NLog ;
using NLog.Fluent ;
using ILogger = Microsoft.Build.Framework.ILogger ;

namespace ProjLib
{
    public class MyLogger2 : ILogger
    {
        private static Logger          Logger     = LogManager.GetCurrentClassLogger ( ) ;
        private        LoggerVerbosity _verbosity = LoggerVerbosity.Diagnostic ;
        private        string          _parameters ;
        #region Implementation of ILogger
        public delegate void Handler ( object sender , BuildEventArgs e ) ;

        public void Initialize ( IEventSource eventSource )
        {
            var eventInfos = eventSource.GetType ( ).GetEvents ( ) ;
            Debug.Assert ( eventInfos.Any ( ) ) ;
            foreach ( var eventInfo in eventInfos )
            {
                Logger.Info ( "adding event handler {name}" , eventInfo.Name ) ;
                try
                {
                    var cdata = eventInfo.EventHandlerType.GetConstructors ( )
                                         .Select (
                                                  ( info , i )
                                                      => info.GetParameters ( )
                                                             .Select (
                                                                      ( parameterInfo , i1 )
                                                                          => Tuple.Create (
                                                                                           parameterInfo
                                                                                              .Name
                                                                                         , parameterInfo
                                                                                              .ParameterType
                                                                                          )
                                                                     )
                                                 )
                                         .ToList ( ) ;
                    Logger.Debug ( "cdata: {cdata}" , cdata ) ;
                    eventInfo.AddEventHandler (
                                               this
                                             , new EventHandler < BuildEventArgs > ( Handle )
                                              ) ; //new Handler ( Handle ))) ;
                }
                catch ( Exception ex )
                {
                    Logger.Warn( ex , ex.ToString ( ) ) ;
                }
            }
        }

        private void Handle ( object sender , BuildEventArgs e )
        {
            Logger.Info ( e.Message ) ;
            try
            {
                var l = new List < Tuple < string , object > > ( ) ;
                foreach ( var fieldInfo in e.GetType ( ).GetFields ( BindingFlags.Default ) )
                {
                    var val = fieldInfo.GetValue ( e ) ;
                    l.Add ( Tuple.Create ( fieldInfo.Name , val ) ) ;
                }

                var msgTemplate = string.Join (
                                               ""
                                             , l.Select (
                                                         tuple
                                                             => $"{tuple.Item1} = {{{tuple.Item1}; "
                                                        )
                                              ) ;
                new LogBuilder ( LogManager.GetLogger ( e.SenderName ) )
                   .Message ( msgTemplate , l.Select ( tuple => tuple.Item2 ).ToArray ( ) )
                   .Property ( "BuildEventArgs" , e )
                   .Write ( ) ;
            }
            catch ( Exception ex )
            {
                Logger.Warn ( ex , ex.ToString ( ) ) ;
            }
        }

        public void Shutdown ( ) { Logger.Warn ( "Shutting down" ) ; }

        public LoggerVerbosity Verbosity { get => _verbosity ; set => _verbosity = value ; }

        public string Parameters { get => _parameters ; set => _parameters = value ; }
        #endregion
    }
}