using System.Collections.Generic ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using System.Windows.Documents ;
using CodeAnalysisApp1 ;
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

        private static TransformBlock < string , Workspace > WorkspaceTransformBlock ( )
        {
            var t0 = new TransformBlock < string , Workspace > (
                                                                s => ProjLibUtils
                                                                   .LoadSolutionInstanceAsync (
                                                                                               null
                                                                                             , s
                                                                                             , null
                                                                                              )
                                                               ) ;
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
            var workspaceTransformBlock = Pipeline.WorkspaceTransformBlock ( ) ;
            _dataflowBlocks.Add(workspaceTransformBlock);
            DataflowLinkOptions opt = new DataflowLinkOptions ( ) { PropagateCompletion = true } ;
            var findLogUsagesBlock = new FindLogUsagesBlock (act ) ;
            _dataflowBlocks.Add(findLogUsagesBlock);
            ITargetBlock < Document > takeDocument = findLogUsagesBlock.Target ;

            _solutionDocumentsBlock = Pipeline.SolutionDocumentsBlock ( takeDocument ) ;
            workspaceTransformBlock.LinkTo (
                                            _solutionDocumentsBlock, opt
                                           ) ;
            PipelineInstance = DataflowBlock.Encapsulate < string , LogInvocation > ( workspaceTransformBlock , findLogUsagesBlock.Source ) ;
            return PipelineInstance ;
        }
    }
}