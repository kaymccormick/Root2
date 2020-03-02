using System ;
using System.Collections.Generic ;
using System.Collections.Immutable ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Reflection ;
using System.Runtime.CompilerServices ;
using System.ServiceModel.Channels ;
using System.Text.RegularExpressions ;
using System.Threading ;
using System.Threading.Tasks ;

using Microsoft.Build.Evaluation ;
using Microsoft.Build.Execution ;
using Microsoft.Build.Framework ;
using Microsoft.Build.Locator ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.Host.Mef ;
using Microsoft.CodeAnalysis.MSBuild ;
using Microsoft.CodeAnalysis.Options ;
using NLog ;
using NLog.Fluent ;
using NLog.LogReceiverService ;
using ILogger = Microsoft.Build.Framework.ILogger ;

namespace ProjLib
{
    public static class ProjLibUtils
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public static async Task < Workspace > LoadSolutionInstanceAsync (
            IProgress < ProjectLoadProgress > progress
          , string                            solutionPath
          , VisualStudioInstance              instance
        )

        {
            Logger.Info (
                         "Load solution {threadid} {solution}"
                       , Thread.CurrentThread.ManagedThreadId
                       , solutionPath
                        ) ;
            if ( MSBuildLocator.IsRegistered )
            {
                // MSBuildLocator.Unregister ( ) ;
            }

            if ( MSBuildLocator.CanRegister )
            {
                MSBuildLocator.RegisterInstance ( instance ) ;
            }
            else
            {
                // throw new Exception ( "Unable to register msbuildlocator" ) ;
            }

            var b = ImmutableDictionary.CreateBuilder<string, string>();
            b["Platform"] = "x86";

            IDictionary<string, string> props = b.ToImmutable();
            IDictionary<string, string> props2 = b.ToImmutable();

            MSBuildWorkspace workspace ;
            try
            {
                workspace           = MSBuildWorkspace.Create ( props ) ;
            }
            catch ( Exception ex )
            {
                throw ;
            }

            
            var projectCollection = new ProjectCollection ( ) ;
            var dir = Path.GetDirectoryName ( solutionPath ) ;
List<string> files;
var projFilesList = Path.Combine(dir, "projects.txt");
 if(File.Exists(projFilesList)) {
files = File.ReadAllLines(projFilesList).ToList();
} else {
            files = Directory.EnumerateFiles ( dir , "*.csproj" , SearchOption.AllDirectories ).ToList();
}
            Logger.Debug ( "{projects}" , projectCollection.LoadedProjects ) ;

            var buildParameters = new BuildParameters(projectCollection);
            buildParameters.ProjectLoadSettings = ProjectLoadSettings.Default;
            buildParameters.Interactive         = false;
            buildParameters.Loggers = new[] { new MyLogger ( ) } ;

            BuildManager.DefaultBuildManager.ResetCaches();
            foreach ( string f in files)
            {
                string realF = Path.Combine ( dir , f ) ;
                var buildRequest = new BuildRequestData (
                                                         realF
                                                       , props2
                                                       , null
                                                       , new[] { "Restore" }
                                                       , new HostServices ( )
                                                       , BuildRequestDataFlags
                                                            .ProvideProjectStateAfterBuild
                                                        ) ;
                
                var buildResult =
                    BuildManager.DefaultBuildManager.Build ( buildParameters, buildRequest ) ;
                if ( buildResult.OverallResult == BuildResultCode.Failure )
                {
                    // catch result ..
                }

                Logger.Info ( "Result: {buildResult}" , buildResult.OverallResult ) ;
            }


            Logger.Info (
                         "SkipUnrecognizedProjects is {SkipUnrecognizedProjects}"
                       , workspace.SkipUnrecognizedProjects
                        ) ;
            workspace.SkipUnrecognizedProjects = true ;
            workspace.DocumentOpened += ( sender , args )
                => Logger.Debug (
                                 "{eventName}: {document}"
                               , nameof ( workspace.DocumentOpened )
                               , args.Document
                                ) ;
            
            // Print message for WorkspaceFailed event to help diagnosing project load failures.

            workspace.WorkspaceFailed += ( o , e ) => {
                try
                {
                    var m = Regex.Match (
                                         e.Diagnostic.Message
                                       , "Msbuild failed when processing the file '(.*)' with message:(.*)"
                                        ) ;
                    if ( m.Success )
                    {
                        var file1 = m.Groups[ 1 ].Captures[ 0 ] ;
                        var message = m.Groups[ 2 ].Captures[ 0 ] ;
                        Logger.Warn ( "Workspace Error: {file}, {msg}" , file1 , message ) ;
                    }
                }
                catch ( Exception ex )
                {
                    Logger.Warn ( ex , ex.ToString ( ) ) ;
                }

                // "Msbuild failed when processing the file 'C:\\Users\\mccor.LAPTOP-T6T0BN1K\\source\\repos\\V3\\Root\\src\\KayMcCormick.Dev\\Desktop\\App1\\App1.csproj' with message: C:\\WINDOWS\\Microsoft.NET\\Framework\\v4.0.30319\\Microsoft.WinFx.targets: (268, 9): Unknown build error, 'Object reference not set to an instance of an object.' "
                Logger.Fatal ( "Load workspac failed: {}" , e.Diagnostic.Message ) ;
            } ;

            // ReSharper disable once LocalizableElement
            Debug.Assert ( solutionPath != null , nameof ( solutionPath ) + " != null" ) ;
            Logger.Debug ( $"Loading solution '{solutionPath}'" ) ;

            // Attach progress reporter so we print projects as they are loaded.
            //workspace.Services.GetService < IOptionService > ( ) ;
            //workspace.Options.WithChangedOption(new OptionKey(), ))
            await workspace.OpenSolutionAsync ( solutionPath , progress ).ConfigureAwait ( false ) ;
            // , new Program.ConsoleProgressReporter()
            // );
            Logger.Debug ( $"Finished loading solution '{solutionPath}'" ) ;
            return workspace ;
        }

        public static ProjectContext MakeProjectContextForSolutionPath ( string arg )
        {
            var dir = Path.GetDirectoryName ( arg ) ;
            var r = LibGit2Sharp.Repository.Discover ( dir ) ;
            var listRemoteReferences = LibGit2Sharp.Repository.ListRemoteReferences ( r ) ;
            foreach ( var listRemoteReference in listRemoteReferences )
            {
                Logger.Info ( "{x} {y} {z}" , listRemoteReference.CanonicalName, listRemoteReference.IsRemoteTrackingBranch, listRemoteReference.TargetIdentifier ) ;
            }

            var repos = new LibGit2Sharp.Repository ( r ) ;

            foreach ( var networkRemote in repos.Network.Remotes )
            {
                Logger.Info ( "{y} {x}" , networkRemote.Url , networkRemote.Name ) ;
            }

            foreach ( var reposRef in repos.Refs )
            {

            }

            return new ProjectContext ( arg ) ;
        }
    }


    public class MyLogger2 : ILogger
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private LoggerVerbosity _verbosity = LoggerVerbosity.Diagnostic ;
        private string _parameters ;
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
                                                                                          parameterInfo.Name
                                                                                        , parameterInfo
                                                                                             .ParameterType
                                                                                         )
                                                                    )
                                                )
                                        .ToList ( ) ;
                    Logger.Info ( "cdata: {cdata}" , cdata ) ;
                    eventInfo.AddEventHandler ( this , new EventHandler < BuildEventArgs > ( Handle ) ) ;//new Handler ( Handle ))) ;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, ex.ToString());
                }
            }
        }

        private void Handle ( object sender , BuildEventArgs e )
        {
            Logger.Info ( e.Message ) ;
            try
            {
                List < Tuple < string , object > > l = new List < Tuple < string , object > > ( ) ;
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
            } catch(Exception ex)
            {
                Logger.Error ( ex , ex.ToString ( ) ) ;
            }
        }

        public void Shutdown ( )
        {
            Logger.Warn ( "Shutting down" ) ;

        }

        public LoggerVerbosity Verbosity { get => _verbosity ; set => _verbosity = value ; }

        public string Parameters { get => _parameters ; set => _parameters = value ; }
        #endregion
    }

    public class MyLogger : ILogger
    {
        private LoggerVerbosity _verbosity ;
        private string _parameters ;
        #region Implementation of ILogger
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        public void Initialize ( IEventSource eventSource )
        {
            try
            {
                eventSource.AnyEventRaised    += EventSourceOnAnyEventRaised ;
                eventSource.ErrorRaised       += EventSourceOnErrorRaised ;
                eventSource.WarningRaised     += EventSourceOnWarningRaised ;
                eventSource.BuildFinished     += EventSourceOnBuildFinished ;
                eventSource.CustomEventRaised += EventSourceOnCustomEventRaised ;
                eventSource.StatusEventRaised += EventSourceOnStatusEventRaised ;
                eventSource.TaskStarted       += EventSourceOnTaskStarted ;
                eventSource.BuildStarted      += EventSourceOnBuildStarted ;
                eventSource.MessageRaised     += EventSourceOnMessageRaised ;
                eventSource.TargetStarted     += EventSourceOnTargetStarted ;
            }catch(Exception ex)
            {
                Logger.Error ( ex , ex.ToString ( ) ) ;
            }
        }

        private void EventSourceOnTargetStarted ( object sender , TargetStartedEventArgs e )
        {
            var lb = LB ( e ) ;
            lb.Write();
        }

        private void EventSourceOnMessageRaised ( object sender , BuildMessageEventArgs e )
        {
            var lb = LB ( e ) ;
            switch ( e )
            {
                case CriticalBuildMessageEventArgs criticalBuildMessageEventArgs :
                    lb = lb.Message ( 
                        criticalBuildMessageEventArgs.Message ) ;
                    break ;
                case MetaprojectGeneratedEventArgs metaprojectGeneratedEventArgs :
                    break ;
                case ProjectImportedEventArgs projectImportedEventArgs :
                    break ;
                case TargetSkippedEventArgs targetSkippedEventArgs : break ;
                case TaskCommandLineEventArgs taskCommandLineEventArgs : break ;
            }
            lb.Write();
        }

        private static LogBuilder LB ( EventArgs e, [CallerMemberName] string callerMemberName= "" )
        {
            return new LogBuilder ( Logger ).LoggerName($"{callerMemberName}.{e.GetType (  ).Name}")
                  .Property ( "EventArgs" , e )
                  .Message ( "{message}" , (e is BuildMessageEventArgs be) ? (be.Message ?? "null message") : "no message") ;
        }

        private void EventSourceOnBuildStarted ( object sender , BuildStartedEventArgs e )
        {
            Logger.Info ( e.Message ) ;
        }

        private void EventSourceOnTaskStarted ( object sender , TaskStartedEventArgs e )
        {
            Logger.Info ( "{0} {1} {2}" , e.Message , e.TaskName , e.TaskFile ) ;
        }

        private void EventSourceOnStatusEventRaised ( object sender , BuildStatusEventArgs e )
        {
            new LogBuilder ( Logger ).Level ( LogLevel.Warn )
                                     .Message ( e.Message )
                                     .Property ( "BuildEventContext" , e.BuildEventContext )
                                     .Write ( ) ;
        }


        private void EventSourceOnCustomEventRaised ( object sender , CustomBuildEventArgs e )
        {
            Logger.Warn ( "{Message} {Context}" , e.Message , e.BuildEventContext ) ;
        }

        private void EventSourceOnBuildFinished ( object sender , BuildFinishedEventArgs e )
        {
            if ( e.Succeeded )
            {

                Logger.Info ( "Build finished sucessfully." ) ;
            }
            else
            {
                Logger.Fatal ( "Build failed: {Message}" , e.Message ) ;
            }
        }
    

        private void EventSourceOnWarningRaised ( object sender , BuildWarningEventArgs e )
        {
            Logger.Warn ( "{projectFile} {message} {e}" , e.ProjectFile , e.Message , e ) ;

        }

        private void EventSourceOnErrorRaised ( object sender , BuildErrorEventArgs e )
        {
            Logger.Error ( "{projectFile} {Message} {e}", e.ProjectFile , e.Message , e ) ;
        }

        private void EventSourceOnAnyEventRaised ( object sender , EventArgs e)
        {
            var lb = LB ( e ).Level(LogLevel.Debug).LoggerName(string.Join(".", nameof(EventSourceOnAnyEventRaised), e.GetType().Name));
            lb.Write();
        }

        public void Shutdown ( ) { }

        public LoggerVerbosity Verbosity { get => _verbosity ; set => _verbosity = value ; }

        public string Parameters { get => _parameters ; set => _parameters = value ; }
        #endregion
    }
}
