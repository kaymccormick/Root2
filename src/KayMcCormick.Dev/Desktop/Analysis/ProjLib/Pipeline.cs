using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using AnalysisFramework ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;
using NLog.Fluent ;

namespace ProjLib
{
    public class Pipeline
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private TransformManyBlock < Workspace , Document > _solutionDocumentsBlock ;

        public IPropagatorBlock < string , ILogInvocation > PipelineInstance { get ; private set ; }

        private List < IDataflowBlock > _dataflowBlocks = new List < IDataflowBlock > ( ) ;

        private List < ISourceBlock < object > > sourceBlocks =
            new List < ISourceBlock < object > > ( ) ;

        public BufferBlock < ILogInvocation > ResultBufferBlock { get ; }

        public Pipeline ( )
        {
            ResultBufferBlock =
                new BufferBlock < ILogInvocation > ( new DataflowBlockOptions ( ) { } ) ;
            _ = BuildPipeline ( ) ;
        }

        private static IPropagatorBlock < string , ProjectContext > ProjectContextBlock ( )
        {
            return new TransformBlock < string , ProjectContext > (
                                                                   ProjLibUtils
                                                                      .MakeProjectContextForSolutionPath
                                                                  ) ;
        }

        private static TransformBlock < string , string > repositoryTransformBlock ( )
        {
            return new TransformBlock < string , string > ( ProjLibUtils.CloneProjectAsync ) ;
        }

        public IPropagatorBlock < string , ILogInvocation > BuildPipeline ( )
        {
            var opt = new DataflowLinkOptions ( ) { PropagateCompletion = true } ;

            var input = new WriteOnceBlock < string > ( s => s ) ;
            var clone = repositoryTransformBlock ( ) ;
            input.LinkTo ( clone , opt ) ;
            var build = DataflowBlocks.BuildBlock ( ) ;
            clone.LinkTo ( build , opt ) ;
            var initWorkspace = DataflowBlocks.WorkspaceBlock ( ) ;
            build.LinkTo ( initWorkspace , opt ) ;

            var findLogUsages = DataflowBlocks.FindLogUsagesBlock ( ) ;
            
            var toDocuments = _solutionDocumentsBlock = DataflowBlocks.SolutionDocumentsBlock();
            initWorkspace.LinkTo ( toDocuments , opt ) ;
            toDocuments.LinkTo ( findLogUsages , opt ) ;
            findLogUsages.LinkTo ( ResultBufferBlock , opt ) ;

            Continuation ( input ,                  nameof ( input ) ) ;
            Continuation ( clone ,                  nameof ( clone ) ) ;
            Continuation ( build ,                  nameof ( build ) ) ;
            Continuation ( initWorkspace ,        nameof ( initWorkspace ) ) ;
            Continuation ( toDocuments , nameof ( toDocuments ) ) ;
            Continuation ( findLogUsages ,     nameof ( findLogUsages ) ) ;
            Continuation ( ResultBufferBlock ,      nameof ( ResultBufferBlock ) ) ;
            PipelineInstance = DataflowBlock.Encapsulate ( input , ResultBufferBlock ) ;
            return PipelineInstance ;
        }

        private void Continuation ( IDataflowBlock writeOnceBlock , string writeOnceBlockName )
        {
            writeOnceBlock.Completion.ContinueWith (
                                                    task => ContinuationFunction (
                                                                                  task
                                                                                , writeOnceBlockName
                                                                                 )
                                                   ) ;
        }

        private void ContinuationFunction ( Task task , string onceBlockName )
        {
            if ( task.IsFaulted )
            {
                new LogBuilder ( Logger )
                   .LoggerName ( $"{Logger.Name}.{onceBlockName}" )
                   .Level ( LogLevel.Debug )
                   .Exception ( task.Exception )
                   .Message ( "fault is {ex}" , task.Exception.ToString ( ) )
                   .Write ( ) ;
            }
            else { Logger.Debug ( $"{onceBlockName} complete - not faulted" ) ; }
        }

        private Workspace Transform ( BuildResults arg ) { return null ; }
    }

    public class BuildResults
    {
        private string sourceDir ;

        public List < string > SolutionsFilesList { get ; set ; } = new List < string > ( ) ;

        public string SourceDir { get => sourceDir ; set => sourceDir = value ; }
    }

    public class ProjectContext
    {
        public ProjectContext ( string s ) { }
    }
}
