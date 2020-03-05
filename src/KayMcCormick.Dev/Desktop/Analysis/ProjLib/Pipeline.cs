using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using AnalysisFramework ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;
using NLog.Fluent ;

namespace ProjLib
{
    public class Pipeline
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public IPropagatorBlock < string , ILogInvocation > PipelineInstance { get ; private set ; }

        private List < IDataflowBlock > _dataflowBlocks = new List < IDataflowBlock > ( ) ;

        public BufferBlock < ILogInvocation > ResultBufferBlock { get ; }

        public Pipeline ( )
        {
            ResultBufferBlock =
                new BufferBlock < ILogInvocation > ( new DataflowBlockOptions ( ) { } ) ;
            _ = BuildPipeline ( ) ;
        }

        public IPropagatorBlock < string , ILogInvocation > BuildPipeline ( )
        {
            var opt = new DataflowLinkOptions { PropagateCompletion = true } ;

            var input = new WriteOnceBlock < string > ( s => s ) ;
            var clone = DataflowBlocks.ClonseSource ( ) ;
            input.LinkTo ( clone , opt ) ;
            var build = DataflowBlocks.PackagesRestore ( ) ;
            clone.LinkTo ( build , opt ) ;
            var initWorkspace = DataflowBlocks.InitializeWorkspace ( ) ;
            build.LinkTo ( initWorkspace , opt ) ;

            var toDocuments = Workspaces.SolutionDocumentsBlock ( ) ;
            initWorkspace.LinkTo ( toDocuments , opt ) ;
            var findLogUsages = DataflowBlocks.FindLogUsages();
            toDocuments.LinkTo ( findLogUsages , opt ) ;
            findLogUsages.LinkTo ( ResultBufferBlock , opt ) ;

            Continuation ( input ,             nameof ( input ) ) ;
            Continuation ( clone ,             nameof ( clone ) ) ;
            Continuation ( build ,             nameof ( build ) ) ;
            Continuation ( initWorkspace ,     nameof ( initWorkspace ) ) ;
            Continuation ( toDocuments ,       nameof ( toDocuments ) ) ;
            Continuation ( findLogUsages ,     nameof ( findLogUsages ) ) ;
            Continuation ( ResultBufferBlock , nameof ( ResultBufferBlock ) ) ;

            PipelineInstance = DataflowBlock.Encapsulate ( input , ResultBufferBlock ) ;
            return PipelineInstance ;
        }

        private static void Continuation ( IDataflowBlock block , string writeOnceBlockName )
        {
            block.Completion.ContinueWith (
                                           task => ContinuationFunction (
                                                                         task
                                                                       , writeOnceBlockName
                                                                        )
                                          ) ;
        }

        private static void ContinuationFunction ( Task task , string logName )
        {
            if ( task.IsFaulted )
            {
                new LogBuilder ( Logger )
                   .LoggerName ( $"{Logger.Name}.{logName}" )
                   .Level ( LogLevel.Debug )
                   .Exception ( task.Exception )
                   .Message ( "fault is {ex}" , task.Exception )
                   .Write ( ) ;
            }
            else { Logger.Debug ( $"{logName} complete - not faulted" ) ; }
        }
    }
}