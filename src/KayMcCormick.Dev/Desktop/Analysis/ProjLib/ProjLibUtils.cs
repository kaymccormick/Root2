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
using LibGit2Sharp ;
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
using LogLevel = NLog.LogLevel ;

namespace ProjLib
{
    public static class ProjLibUtils
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private static string _projectRootDir = Path.Combine (
                                                              Environment.GetFolderPath (
                                                                                         Environment
                                                                                            .SpecialFolder
                                                                                            .MyDocuments
                                                                                        )
                                                            , "ProjectLib"
                                                             ) ;

        public static ProjectContext MakeProjectContextForSolutionPath ( string arg )
        {
            var dir = Path.GetDirectoryName ( arg ) ;
            var r = Repository.Discover ( dir ) ;
            var listRemoteReferences = Repository.ListRemoteReferences ( r ) ;
            foreach ( var listRemoteReference in listRemoteReferences )
            {
                Logger.Info (
                             "{x} {y} {z}"
                           , listRemoteReference.CanonicalName
                           , listRemoteReference.IsRemoteTrackingBranch
                           , listRemoteReference.TargetIdentifier
                            ) ;
            }

            var repos = new Repository ( r ) ;

            foreach ( var networkRemote in repos.Network.Remotes )
            {
                Logger.Info ( "{y} {x}" , networkRemote.Url , networkRemote.Name ) ;
            }

            foreach ( var reposRef in repos.Refs )
            {
            }

            return new ProjectContext ( arg ) ;
        }

        public static async Task < string > CloneProjectAsync ( string arg )
        {
            Logger.Info ( "Clone {arg}" , arg ) ;
            var workdirPath = Path.Combine ( ProjectRootDir , Path.GetRandomFileName ( ) ) ;

            var r = await Task.Run ( ( ) => Repository.Clone ( arg , workdirPath ) ) ;
            return workdirPath ;
        }

        public static string ProjectRootDir => _projectRootDir ;

        public static BuildResults BuildRepository ( string arg )
        {
            Logger.Info ( "BuildRepository {arg}" , arg ) ;
            try
            {
                if ( MSBuildLocator.CanRegister )
                {
                    MSBuildLocator.RegisterInstance (
                                                     MSBuildLocator
                                                        .QueryVisualStudioInstances (
                                                                                     new
                                                                                     VisualStudioInstanceQueryOptions ( )
                                                                                     {
                                                                                         DiscoveryTypes
                                                                                             = DiscoveryType
                                                                                                .VisualStudioSetup
                                                                                     }
                                                                                    )
                                                        .First (
                                                                instance
                                                                    => instance.Version.Major == 16
                                                                       && instance.Version.Minor
                                                                       == 4
                                                               )
                                                    ) ;
                }

                var b = ImmutableDictionary.CreateBuilder < string , string > ( ) ;
                b[ "Platform" ] = "x86" ;

                IDictionary < string , string > props = b.ToImmutable ( ) ;

                var projectCollection = new ProjectCollection ( ) ;

                List < string > files ;
                var projFilesList = Path.Combine ( arg , "projects.txt" ) ;
                Logger.Debug ( "Checking for existince of poject file {file}" , projFilesList ) ;
                if ( File.Exists ( projFilesList ) )
                {
                    files = File.ReadAllLines ( projFilesList ).ToList ( ) ;
                }
                else
                {
                    files = Directory
                           .EnumerateFiles ( arg , "*.csproj" , SearchOption.AllDirectories )
                           .ToList ( ) ;
                }

                List < string > solList ;
                var sol = Path.Combine ( arg , "solutions.txt" ) ;
                Logger.Debug ( "Checking for existince of poject file {file}" , projFilesList ) ;
                if ( File.Exists ( sol ) )
                {
                    solList = File.ReadAllLines ( sol ).ToList ( ) ;
                }
                else
                {
                    solList = Directory
                             .EnumerateFiles ( arg , "*.sln" , SearchOption.AllDirectories )
                             .ToList ( ) ;
                }

                var buildParameters = new BuildParameters ( projectCollection ) ;
                buildParameters.ProjectLoadSettings = ProjectLoadSettings.Default ;
                buildParameters.Interactive         = false ;
                buildParameters.Loggers             = new[] {new MyLogger ( ) } ;

                var buildResults = new BuildResults ( )
                                   {
                                       SourceDir = arg , SolutionsFilesList = solList
                                   } ;


                var buildFiles = new[] { solList.First ( ) } ;
                BuildManager.DefaultBuildManager.ResetCaches ( ) ;
                foreach ( var f in buildFiles )
                {
                    var realF = Path.Combine ( arg , f ) ;
                    Logger.Warn ( "Building {file}" , f ) ;
                    var buildRequest = new BuildRequestData (
                                                             realF
                                                           , props
                                                           , null
                                                           , new[] { "Restore" }
                                                           , new HostServices ( )
                                                           , BuildRequestDataFlags
                                                                .ProvideProjectStateAfterBuild
                                                            ) ;

                    var result =
                        BuildManager.DefaultBuildManager.Build ( buildParameters , buildRequest ) ;

                    if ( result.OverallResult == BuildResultCode.Failure )
                    {
                        // catch result ..
                    }

                    Logger.Info ( "Result: {buildResult}" , result.OverallResult ) ;
                }



                return buildResults ;
            }
            catch ( Exception ex )
            {
                Logger.Fatal ( ex , ex.ToString ( ) ) ;
            }

            return null ;
        }

        public static async Task < Workspace > MakeWorkspaceAsync ( BuildResults results )
        {
            var b = ImmutableDictionary.CreateBuilder < string , string > ( ) ;
            b[ "Platform" ] = "x86" ;

            IDictionary < string , string > props = b.ToImmutable ( ) ;

            MSBuildWorkspace workspace ;
            try
            {
                workspace = MSBuildWorkspace.Create ( props ) ;
            }
            catch ( Exception ex )
            {
                throw ;
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
            var solutionPath = Path.Combine (
                                             results.SourceDir
                                           , results.SolutionsFilesList.First ( )
                                            ) ;
            Debug.Assert ( solutionPath != null , nameof ( solutionPath ) + " != null" ) ;
            Logger.Debug ( $"Loading solution '{solutionPath}'" ) ;

            // Attach progress reporter so we print projects as they are loaded.
            //workspace.Services.GetService < IOptionService > ( ) ;
            //workspace.Options.WithChangedOption(new OptionKey(), ))
            await workspace.OpenSolutionAsync ( solutionPath ).ConfigureAwait ( true ) ;
            // , new Program.ConsoleProgressReporter()
            // );
            Logger.Debug ( $"Finished loading solution '{solutionPath}'" ) ;
            return workspace ;
        }
    }


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
                    Logger.Error ( ex , ex.ToString ( ) ) ;
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
                Logger.Error ( ex , ex.ToString ( ) ) ;
            }
        }

        public void Shutdown ( ) { Logger.Warn ( "Shutting down" ) ; }

        public LoggerVerbosity Verbosity { get => _verbosity ; set => _verbosity = value ; }

        public string Parameters { get => _parameters ; set => _parameters = value ; }
        #endregion
    }

    public class MyLogger : ILogger
    {
        public bool EnableAnyEvent { get ; set ; } = false ;

        private LoggerVerbosity _verbosity = LoggerVerbosity.Quiet ;

        private string          _parameters ;
        #region Implementation of ILogger
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

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

                eventSource.BuildStarted += EventSourceOnBuildStarted;
                eventSource.BuildFinished += EventSourceOnBuildFinished;
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
                Logger.Error ( ex , ex.ToString ( ) ) ;
            }
        }

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
            LB(e).Level (e.Succeeded ? LogLevel.Debug: LogLevel.Error).Write();
        }

        private void EventSourceOnTargetFinished ( object sender , TargetFinishedEventArgs e )
        {
            #if !DEBUG
            if ( e.TargetName.StartsWith ( "_" ) ) return ;
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
                lb = lb.Level ( LogLevel.Error ) ;
            }
        }

        private void EventSourceOnTargetStarted ( object sender , TargetStartedEventArgs e )
        {
#if !DEBUG
            if (e.TargetName.StartsWith("_")) return;
#endif
            var lb = LB ( e ).Message ("{targetMame} {buildReason} {ParentTarget} {ProjectFile} {TargetFile}", e.TargetName, e.BuildReason, e.ParentTarget, e.ProjectFile, e.TargetFile );
            lb.Write ( ) ;
        }

        private void EventSourceOnMessageRaised ( object sender , BuildMessageEventArgs e )
        {
            if ( e.SenderName    == "RestoreTask"
                 && e.Importance == MessageImportance.Low )
                return ;

            var lb = LB ( e ).Level (e.Importance == MessageImportance.Low ? LogLevel.Trace : e.Importance == MessageImportance.High ? LogLevel.Warn : LogLevel.Debug).Message("{SenderName}: {Code}: {File}:{LineNumber}:{ColumnNumber}:{Subcategory}:{Message}", e.SenderName, e.Code, e.File, e.LineNumber, e.ColumnNumber, e.Subcategory, e.Message );
            
            switch ( e )
            {
                case CriticalBuildMessageEventArgs criticalBuildMessageEventArgs :
                    lb = lb.Level ( LogLevel.Error ) ;
                    break ;
                case MetaprojectGeneratedEventArgs metaprojectGeneratedEventArgs :
                    lb = lb.Property (
                                      "metaprojcetXml"
                                    , metaprojectGeneratedEventArgs.metaprojectXml
                                     ) ;
                    break ;
                case ProjectImportedEventArgs projectImportedEventArgs :           break ;
                case TargetSkippedEventArgs targetSkippedEventArgs :               break ;
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
            LazyFormattedBuildEventArgs                   e
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
            LB ( e ).Level ( LogLevel.Warn ).Write ( ) ;
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
            LB ( e ).Write ( ) ;
            //Logger.Warn ( "{projectFile} {message} {e}" , e.ProjectFile , e.Message , e ) ;
        }

        private void EventSourceOnErrorRaised ( object sender , BuildErrorEventArgs e )
        {
            LB ( e )
               .Level ( LogLevel.Error )
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