using System.Collections.Generic ;
using System.Threading.Tasks.Dataflow ;
using AnalysisFramework ;
using Microsoft.CodeAnalysis ;
using NLog ;

namespace ProjLib
{
    public class Pipeline
    {
        private static Logger                    Logger = LogManager.GetCurrentClassLogger ( ) ;
        private        ActionBlock < Workspace > _solutionDocumentsBlock ;

        public IPropagatorBlock < string , LogInvocation > PipelineInstance { get ; private set ; }

        private List < IDataflowBlock > _dataflowBlocks = new List < IDataflowBlock > ( ) ;

        private List < ISourceBlock < object > > sourceBlocks =
            new List < ISourceBlock < object > > ( ) ;

        public BufferBlock < LogInvocation > ResultBufferBlock { get ; }

        public Pipeline ( ITargetBlock < LogInvocation > act = null )
        {
            if ( act == null )
            {
                ResultBufferBlock =
                    new BufferBlock < LogInvocation > ( new DataflowBlockOptions ( ) { } ) ;
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

        private static ActionBlock < Workspace > SolutionDocumentsBlock (
            ITargetBlock < Document > takeDocument
        )
        {
            return new ActionBlock < Workspace > (
                                                  workspace => {
                                                      foreach ( var proj in workspace
                                                                           .CurrentSolution.Projects
                                                      )
                                                      {
                                                          foreach ( var projDocument in proj
                                                             .Documents )
                                                          {
                                                              takeDocument.SendAsync (
                                                                                      projDocument
                                                                                     ) ;
                                                          }
                                                      }
                                                  }
                                                 ) ;
        }

        public IPropagatorBlock < string , LogInvocation > BuildPipeline (
            ITargetBlock < LogInvocation > act
        )
        {
            var opt = new DataflowLinkOptions ( ) { PropagateCompletion = true } ;

            var onceBlock =
                new WriteOnceBlock < string > ( s => s , new ExecutionDataflowBlockOptions ( ) ) ;
            var transformBlock = repositoryTransformBlock ( ) ;
            onceBlock.LinkTo ( transformBlock , opt ) ;
            var buildTransformBlock =
                new TransformBlock < string , BuildResults > (
                                                              s => ProjLibUtils
                                                                 .BuildRepository ( s )
                                                             ) ;
            transformBlock.LinkTo ( buildTransformBlock , opt ) ;
            var findLogUsagesBlock = new FindLogUsagesBlock ( act ) ;
            _dataflowBlocks.Add ( findLogUsagesBlock ) ;
            var takeDocument = findLogUsagesBlock.Target ;

            var makeWs = new TransformBlock < BuildResults , Workspace > (
                                                                          results => ProjLibUtils
                                                                             .MakeWorkspaceAsync (
                                                                                                  results
                                                                                                 )
                                                                         ) ;
            buildTransformBlock.LinkTo ( makeWs , opt ) ;

            _solutionDocumentsBlock = SolutionDocumentsBlock ( takeDocument ) ;
            makeWs.LinkTo ( _solutionDocumentsBlock , opt ) ;
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
            _solutionDocumentsBlock.Completion.ContinueWith (
                                                             task => {
                                                                 Logger.Warn (
                                                                              "documents block complete"
                                                                             ) ;
                                                             }
                                                            ) ;
            PipelineInstance = DataflowBlock.Encapsulate ( onceBlock , findLogUsagesBlock.Source ) ;
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