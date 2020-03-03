using System.Collections.Generic ;
using System.Threading.Tasks.Dataflow ;
using AnalysisFramework ;
using Microsoft.CodeAnalysis ;

namespace ProjLib
{
    public class Pipeline
    {
        private ActionBlock < Workspace > _solutionDocumentsBlock ;

        public IPropagatorBlock < string , LogInvocation > PipelineInstance { get ; private set ; }

        private List<IDataflowBlock> _dataflowBlocks = new List < IDataflowBlock > ();
        private List<ISourceBlock <object>> sourceBlocks = new List < ISourceBlock < object > > ();

        public BufferBlock < LogInvocation > ResultBufferBlock { get ; }

        public Pipeline ( ITargetBlock < LogInvocation > act = null )
        {
            if ( act == null )
            {
                ResultBufferBlock = new BufferBlock < LogInvocation > (new DataflowBlockOptions() { }) ;
                act = ResultBufferBlock;
            }
            _ = BuildPipeline ( act ) ;
        }

        private static IPropagatorBlock <string, ProjectContext> ProjectContextBlock()
        {
            return new TransformBlock < string , ProjectContext > (
                                                                   ProjLibUtils
                                                                      .MakeProjectContextForSolutionPath
                                                                  ) ;
        }
        private static TransformBlock < string , string> repositoryTransformBlock( )
        {
            var t0 = new TransformBlock < string , string > ( ProjLibUtils.CloneProjectAsync) ;
                                                               //
                                                               //  s => ProjLibUtils
                                                               //     .LoadSolutionInstanceAsync (
                                                               //                                 null
                                                               //                               , s
                                                               //                               , null
                                                               //                                )
                                                               // ) ;
            return t0 ;
        }

        private static ActionBlock < Workspace > SolutionDocumentsBlock ( ITargetBlock < Document > takeDocument )
        {
            return new ActionBlock < Workspace > (
                                                  workspace => {
                                                      foreach ( var proj in workspace
                                                                           .CurrentSolution.Projects )
                                                      {
                                                          foreach ( var projDocument in proj.Documents )
                                                          {
                                                              takeDocument.SendAsync ( projDocument ) ;
                                                          }
                                                      }
                                                      takeDocument.Complete();
                                                  }
                                                 ) ;
        }

        public IPropagatorBlock < string , LogInvocation > BuildPipeline (
            ITargetBlock < LogInvocation > act
        )
        {
            DataflowLinkOptions opt = new DataflowLinkOptions() { PropagateCompletion = true };

            var transformBlock = Pipeline.repositoryTransformBlock( ) ;
            var buildTransformBlock =
                new TransformBlock < string , BuildResults > (
                                                              s => ProjLibUtils
                                                                 .BuildRepository ( s )
                                                             ) ;
transformBlock.LinkTo (
                                   buildTransformBlock
                                  ) ;
            var findLogUsagesBlock = new FindLogUsagesBlock (act ) ;
            _dataflowBlocks.Add(findLogUsagesBlock);
            ITargetBlock < Document > takeDocument = findLogUsagesBlock.Target ;

            var makeWs = new TransformBlock < BuildResults , Workspace > (
                                                                               results
                                                                                   => ProjLibUtils.MakeWorkspaceAsync(results)
                                                                              ) ;
            buildTransformBlock.LinkTo (
                                        makeWs
                                      , opt ) ;
            
            _solutionDocumentsBlock = Pipeline.SolutionDocumentsBlock ( takeDocument ) ;
            makeWs.LinkTo ( _solutionDocumentsBlock, opt ) ;
            //var workspace = new TransformBlock<BuildResults, Workspace> (Transform);
            //build.LinkTo ( workspace, opt);
            //workspace.LinkTo ( _solutionDocumentsBlock , opt ) ;
PipelineInstance = DataflowBlock.Encapsulate < string , LogInvocation > ( transformBlock, findLogUsagesBlock.Source ) ;
            return PipelineInstance ;
        }

        private Workspace Transform ( BuildResults arg ) { return null ; }
    }

    public class BuildResults
    {
        private string sourceDir ;

        public List < string > SolutionsFilesList { get ; set ; } = new List < string > ( ) ;

        public string SourceDir { get { return sourceDir ; } set { sourceDir = value ; } }
    }

    public class ProjectContext
    {
        public ProjectContext ( string s ) { }
    }
}