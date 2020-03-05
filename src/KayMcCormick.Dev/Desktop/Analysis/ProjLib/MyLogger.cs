#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// MyLogger.cs
// 
// 2020-03-04-10:52 AM
// 
// ---
#endregion
using System ;
using System.Runtime.CompilerServices ;
using JetBrains.Annotations ;
using Microsoft.Build.Framework ;
using NLog ;
using NLog.Fluent ;
using ILogger = Microsoft.Build.Framework.ILogger ;

namespace ProjLib
{
    public class MyLogger : ILogger
    {
        public bool EnableAnyEvent { get ; set ; } = false ;

        private LoggerVerbosity _verbosity = LoggerVerbosity.Quiet ;

        private string _parameters ;
        #region Implementation of ILogger
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public void Initialize ( IEventSource eventSource )
        {
            try
            {
#if DEBUG
                eventSource.MessageRaised += EventSourceOnMessageRaised;
                eventSource.WarningRaised += EventSourceOnWarningRaised;
                eventSource.TaskStarted += EventSourceOnTaskStarted;
                eventSource.TaskFinished += EventSourceOnTaskFinished;
#endif
                eventSource.ErrorRaised += EventSourceOnErrorRaised;

                eventSource.BuildStarted    += EventSourceOnBuildStarted;
                eventSource.BuildFinished   += EventSourceOnBuildFinished;
                eventSource.ProjectStarted  += EventSourceOnProjectStarted;
                eventSource.ProjectFinished += EventSourceOnProjectFinished;

                if ( EnableAnyEvent )
                {
                    eventSource.AnyEventRaised += EventSourceOnAnyEventRaised ;
                }

                eventSource.CustomEventRaised += EventSourceOnCustomEventRaised ;
                eventSource.StatusEventRaised += EventSourceOnStatusEventRaised ;
                
                eventSource.TargetStarted  += EventSourceOnTargetStarted ;
                eventSource.TargetFinished += EventSourceOnTargetFinished ;
            }
            catch ( Exception ex )
            {
                Logger.Warn ( ex , ex.ToString ( ) ) ;
            }
        }

        [ UsedImplicitly ]
        private void EventSourceOnTaskFinished ( object sender , TaskFinishedEventArgs e )
        {
            LB ( e ).Write ( );
        }

        private void EventSourceOnProjectStarted ( object sender , ProjectStartedEventArgs e )
        {
            LB(e).Level ( e.TargetNames.StartsWith("_") ? LogLevel.Debug :  LogLevel.Info ).Write();
        }

        private void EventSourceOnProjectFinished ( object sender , ProjectFinishedEventArgs e )
        {
            LB(e).Level (e.Succeeded ? LogLevel.Debug: LogLevel.Debug).Write();
        }

        private void EventSourceOnTargetFinished ( object sender , TargetFinishedEventArgs e )
        {
#if !DEBUG
            if ( e.TargetName.StartsWith ( "_" ) )
            {
                return ;
            }
#endif
            var lb = LB ( e ) .Level(LogLevel.Info);
            if ( String.IsNullOrWhiteSpace ( e.Message ) )
            {
                lb = lb.Message (
                                 "Target finished with {success}. Outputs: {outputs}"
                               , e.Succeeded ? "success" : "failure"
                               , e.TargetOutputs
                                ) ;
            }
            if ( e.Succeeded == false )
            {
                lb = lb.Level ( LogLevel.Warn ) ;
            }
            lb.Write();
        }

        private void EventSourceOnTargetStarted ( object sender , TargetStartedEventArgs e )
        {
#if !DEBUG
            if (e.TargetName.StartsWith("_"))
            {
                return;
            }
#endif
            var lb = LB ( e ).Message ("{targetMame} {buildReason} {ParentTarget} {ProjectFile} {TargetFile}", e.TargetName, e.BuildReason, e.ParentTarget, e.ProjectFile, e.TargetFile );
            lb.Write ( ) ;
        }

        [ UsedImplicitly ]
        private void EventSourceOnMessageRaised ( object sender , BuildMessageEventArgs e )
        {
            if ( e.SenderName    == "RestoreTask"
                 && e.Importance == MessageImportance.Low )
            {
                return ;
            }

            var lb = LB ( e ).Level (e.Importance == MessageImportance.Low ? LogLevel.Trace : e.Importance == MessageImportance.High ? LogLevel.Warn : LogLevel.Debug).Message("{SenderName}: {Code}: {File}:{LineNumber}:{ColumnNumber}:{Subcategory}:{Message}", e.SenderName, e.Code, e.File, e.LineNumber, e.ColumnNumber, e.Subcategory, e.Message );
            
            switch ( e )
            {
                case CriticalBuildMessageEventArgs criticalBuildMessageEventArgs :
                    lb = lb.Level ( LogLevel.Warn ) ;
                    break ;
                case MetaprojectGeneratedEventArgs metaprojectGeneratedEventArgs :
                    lb = lb.Property (
                                      "metaprojcetXml"
                                    , metaprojectGeneratedEventArgs.metaprojectXml
                                     ) ;
                    break ;
                case ProjectImportedEventArgs projectImportedEventArgs : break ;
                case TargetSkippedEventArgs targetSkippedEventArgs :     break ;
                case TaskCommandLineEventArgs taskCommandLineEventArgs :
                    lb = lb.Message (
                                     "{SenderName}: {Code}: {File}:{LineNumber}:{ColumnNumber}:{Subcategory}:{Message},{TaskName},{Commandline}"
                                   , e.SenderName
                                   , e.Code
                                   , e.File
                                   , e.LineNumber
                                   , e.ColumnNumber
                                   , e.Subcategory
                                   , e.Message
                                   , taskCommandLineEventArgs.TaskName
                                   , taskCommandLineEventArgs.CommandLine
                                    ) ;
                    break ;
            }

            lb.Write ( ) ;
        }

        private static LogBuilder LB (
            LazyFormattedBuildEventArgs e
          , [ CallerMemberName ] string callerMemberName = ""
        )
        {
            var memberName = callerMemberName ;
            memberName = memberName.Replace ( "EventSourceOn" , "" ) ; 
            var name = e.GetType ( ).Name ;
            name = name.Replace ( "EventArgs" , "" ) ;
            var logBuilder = new LogBuilder ( Logger )
                            .Level ( LogLevel.Debug )
                            .LoggerName ( $"Build.{memberName}.{name}" )
                            .Property ( "BuildEventContext" , e.BuildEventContext )
                            .Message ( e.Message ) ;
            // .Property ( "EventArgs" , e ) ;

            return logBuilder ;
        }

        private void EventSourceOnBuildStarted ( object sender , BuildStartedEventArgs e )
        {
            LB ( e ).Level ( LogLevel.Warn ).Write ( ) ;
        }

        [ UsedImplicitly ]
        private void EventSourceOnTaskStarted ( object sender , TaskStartedEventArgs e )
        {
            LB ( e )
               .Message ( "{Message} {TaskName} {TaskFile}" , e.Message , e.TaskName , e.TaskFile )
               .Write ( ) ;
        }

        private void EventSourceOnStatusEventRaised ( object sender , BuildStatusEventArgs e )
        {
#if DEBUG
            LB (  e ).Level(LogLevel.Warn).Write ( ) ;
            // new LogBuilder ( Logger ).Level ( LogLevel.Warn )
#endif
            // .Message ( e.Message )
            // .Property ( "BuildEventContext" , e.BuildEventContext )
            // .Write ( ) ;
        }


        private void EventSourceOnCustomEventRaised ( object sender , CustomBuildEventArgs e )
        {
            LB(e).Level(LogLevel.Warn).Write();
            //Logger.Warn ( "{Message} {Context}" , e.Message , e.BuildEventContext ) ;
        }

        private void EventSourceOnBuildFinished ( object sender , BuildFinishedEventArgs e )
        {
            LB ( e ).Write ( ) ;
            if ( e.Succeeded )
            {
                Logger.Trace( "Build finished sucessfully." ) ;
            }
            else
            {
                Logger.Trace( "Build failed: {Message}" , e.Message ) ;
            }
        }


        private void EventSourceOnWarningRaised ( object sender , BuildWarningEventArgs e )
        {
            LB ( e ).Write ( ) ;
            //Logger.Warn ( "{projectFile} {message} {e}" , e.ProjectFile , e.Message , e ) ;
        }

        private void EventSourceOnErrorRaised ( object sender , BuildErrorEventArgs e )
        {
            LB ( e )
               .Level ( LogLevel.Debug)
               .Message ( "{projectFile} {Message} {e}" , e.ProjectFile , e.Message , e )
               .Write ( ) ;
        }

        private void EventSourceOnAnyEventRaised ( object sender , EventArgs e )
        {
            // var lb = new LogBuilder(Logger).LoggerName ( e.GetType (  ).FullName ).Level ( LogLevel.Error) ;
            // lb.Write ( ) ;
        }

        public void Shutdown ( ) { }

        public LoggerVerbosity Verbosity { get => _verbosity ; set => _verbosity = value ; }

        public string Parameters { get => _parameters ; set => _parameters = value ; }
        #endregion
    }
}