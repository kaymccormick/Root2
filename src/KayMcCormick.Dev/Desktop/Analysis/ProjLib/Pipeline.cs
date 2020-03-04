using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Threading.Tasks.Dataflow ;
using AnalysisFramework ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;

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

        public Pipeline ( ITargetBlock < ILogInvocation > act = null )
        {
            if ( act == null )
            {
                ResultBufferBlock =
                    new BufferBlock < ILogInvocation > ( new DataflowBlockOptions ( ) { } ) ;
                act = ResultBufferBlock ;
            }

            _ = BuildPipeline ( act ) ;
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

        public IPropagatorBlock < string , ILogInvocation > BuildPipeline (
            ITargetBlock < ILogInvocation > act
        )
        {
            var opt = new DataflowLinkOptions ( ) { PropagateCompletion = true } ;

            var onceBlock =
                new WriteOnceBlock < string > ( s => s , new ExecutionDataflowBlockOptions ( ) ) ;
            var transformBlock = repositoryTransformBlock ( ) ;
            onceBlock.LinkTo ( transformBlock , opt ) ;
            var buildTransformBlock = DataflowBlocks.BuildBlock ( ) ;
            transformBlock.LinkTo ( buildTransformBlock , opt ) ;
            var makeWs = DataflowBlocks.WorkspaceBlock ( ) ;

            buildTransformBlock.LinkTo ( makeWs , opt ) ;



            var findLogUsagesBlock = DataflowBlocks.FindLogUsagesBlock ( ) ;

            _solutionDocumentsBlock = DataflowBlocks.SolutionDocumentsBlock ( ) ;
            makeWs.LinkTo ( _solutionDocumentsBlock , opt ) ;
            _solutionDocumentsBlock.LinkTo ( findLogUsagesBlock , opt ) ;
            findLogUsagesBlock.LinkTo ( act , opt ) ;
            //var workspace = new TransformBlock<BuildResults, Workspace> (Transform);
            //build.LinkTo ( workspace, opt);
            //workspace.LinkTo ( _solutionDocumentsBlock , opt ) ;
            onceBlock.Completion.ContinueWith (
                                               ( task ) => {
                                                   Logger.Warn ( "onceblock complete" ) ;
                                               }
                                              ) ;
            transformBlock.Completion.ContinueWith (
                                                    task => {
                                                        Logger.Warn ( "clone block complete" ) ;
                                                    }
                                                   ) ;
            buildTransformBlock.Completion.ContinueWith (
                                                         task => {
                                                             Logger.Warn (
                                                                          "build block complete"
                                                                         ) ;
                                                         }
                                                        ) ;
            makeWs.Completion.ContinueWith (
                                            task => {
                                                Logger.Warn ( "make workspace block complete" ) ;
                                            }
                                           ) ;
            _solutionDocumentsBlock.Completion.ContinueWith (
                                                             task => {
                                                                 Logger.Warn (
                                                                              "documents block complete"
                                                                             ) ;
                                                             }
                                                            ) ;
            findLogUsagesBlock.Completion.ContinueWith (
                                                        task => {
                                                            Logger.Warn (
                                                                         "find usages block complete"
                                                                        ) ;
                                                        }
                                                       ) ;
            PipelineInstance = DataflowBlock.Encapsulate ( onceBlock , findLogUsagesBlock ) ;
            return PipelineInstance ;
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