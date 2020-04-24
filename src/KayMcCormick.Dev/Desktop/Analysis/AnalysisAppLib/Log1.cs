#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// Log1.cs
// 
// 2020-04-23-8:22 PM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using Buildalyzer ;
using Microsoft.Build.Framework ;
using Microsoft.Build.Logging.StructuredLogger ;
using ProjectEvaluationFinishedEventArgs = Microsoft.Build.Logging.StructuredLogger.ProjectEvaluationFinishedEventArgs ;
using ProjectEvaluationStartedEventArgs = Microsoft.Build.Logging.StructuredLogger.ProjectEvaluationStartedEventArgs ;
using ProjectImportedEventArgs = Microsoft.Build.Logging.StructuredLogger.ProjectImportedEventArgs ;
using TargetSkippedEventArgs = Microsoft.Build.Logging.StructuredLogger.TargetSkippedEventArgs ;

namespace AnalysisAppLib
{
    public class Log1 : Microsoft.Build.Framework.ILogger
    {
        private readonly Action < string >                     _outAct ;
        private readonly IProjectAnalyzer                      _value ;
        private readonly IEnumerable < Action < IEventMisc > > _misc ;

        public Log1 (
            Action < string >                     outAct
          , IProjectAnalyzer                      value
          , IEnumerable < Action < IEventMisc > > misc
        )
        {
            _outAct = outAct ;
            _value  = value ;
            _misc   = misc ;
        }

        private LoggerVerbosity _verbosity ;
        private string          _parameters ;
        #region Implementation of ILogger
        public void Initialize ( IEventSource eventSource )
        {
            
            eventSource.AnyEventRaised += EventSourceOnAnyEventRaised;
            eventSource.ErrorRaised    += EventSource_ErrorRaised;
        }

        private void EventSourceOnAnyEventRaised ( object sender , BuildEventArgs e )
        {
            bool? succeeded ;
            MiscLevel level = MiscLevel.DEBUG;
            int imp ;
            string file="" ;
            if ( e is BuildMessageEventArgs xx1 )
            {
                file = xx1.File ;
            }
            switch ( e )
            {
                
                case BuildFinishedEventArgs buildFinishedEventArgs :
                    level     = MiscLevel.INFO ;
                    succeeded = buildFinishedEventArgs.Succeeded ;break;
                case CriticalBuildMessageEventArgs criticalBuildMessageEventArgs :
                    level = MiscLevel.CRIT ;
                    imp   = 2 - ( int ) criticalBuildMessageEventArgs.Importance ;
                    break ;
                case MetaprojectGeneratedEventArgs metaprojectGeneratedEventArgs : break ;
                case ProjectImportedEventArgs projectImportedEventArgs :           break ;
                case TargetSkippedEventArgs targetSkippedEventArgs :               break ;
                case TaskCommandLineEventArgs taskCommandLineEventArgs :           break ;
                
                case BuildStartedEventArgs buildStartedEventArgs :
                    level = MiscLevel.INFO ;
                    break ;
                case ProjectEvaluationFinishedEventArgs projectEvaluationFinishedEventArgs : break ;
                
                case ProjectEvaluationStartedEventArgs projectEvaluationStartedEventArgs : break ;
                case ProjectFinishedEventArgs projectFinishedEventArgs : level = MiscLevel.INFO;
                    break ;
                case ProjectStartedEventArgs projectStartedEventArgs :
                    level = MiscLevel.INFO;
                    break ;
                case TargetFinishedEventArgs targetFinishedEventArgs : break ;
                case TargetStartedEventArgs2 targetStartedEventArgs2 : break ;
                case TargetStartedEventArgs targetStartedEventArgs :   break ;
                case TaskFinishedEventArgs taskFinishedEventArgs :     break ;
                case TaskStartedEventArgs taskStartedEventArgs :       break ;
                case BuildStatusEventArgs buildStatusEventArgs :
                    level = MiscLevel.DEBUG;
                    break ;
                case BuildWarningEventArgs buildWarningEventArgs :
                    level = MiscLevel.INFO;
                    break ;
                case ExternalProjectFinishedEventArgs externalProjectFinishedEventArgs : break ;
                case ExternalProjectStartedEventArgs externalProjectStartedEventArgs :   break ;
                case CustomBuildEventArgs customBuildEventArgs :
                    level = MiscLevel.INFO;
                    break ;
                case BuildMessageEventArgs buildMessageEventArgs:
                    break;
                case BuildErrorEventArgs buildErrorEventArgs :
                    level = MiscLevel.ERROR ;
                    break ;
                case LazyFormattedBuildEventArgs lazyFormattedBuildEventArgs : break ;
                case TelemetryEventArgs telemetryEventArgs :                   break ;
                default :                                                      throw new ArgumentOutOfRangeException ( nameof ( e ) ) ;
            }

            var msBuildEventMisc = new MSBuildEventMisc(e, level) ;
            msBuildEventMisc.File = file ;
            IEventMisc eventMisc = msBuildEventMisc;
            foreach (var action in _misc)
            {
                if(level != MiscLevel.DEBUG)
                    action(eventMisc);
            }
        }

        private void EventSource_ErrorRaised(object sender, LazyFormattedBuildEventArgs e)
        {
            var n1 = _value.ProjectInSolution.ProjectName ;
            // _outAct?.Invoke ( n1 + ":" +e.Message ) ;
            foreach ( var action in _misc )
            {
                // action ( new Misc ( e ) ) ;
            }
        }

        public void Shutdown ( ) { }

        public LoggerVerbosity Verbosity { get { return _verbosity ; } set { _verbosity = value ; } }

        public string Parameters { get { return _parameters ; } set { _parameters = value ; } }
        #endregion
    }
}