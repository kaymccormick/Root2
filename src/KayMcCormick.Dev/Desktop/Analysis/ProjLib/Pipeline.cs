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
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public IPropagatorBlock < string , ILogInvocation > PipelineInstance { get ; private set ; }

        private List < IDataflowBlock > _dataflowBlocks = new List < IDataflowBlock > ( ) ;
        private DataflowLinkOptions _linkOptions = new DataflowLinkOptions { PropagateCompletion = true } ;
        private IPropagatorBlock < string , string > _currentBlock ;

        public BufferBlock < ILogInvocation > ResultBufferBlock { get ; }

        public DataflowLinkOptions LinkOptions
        {
            get => _linkOptions ;
            set => _linkOptions = value ;
        }

        public IPropagatorBlock< string ,string> CurrentBlock { get { return _currentBlock ; } set { _currentBlock = value ; } }

        public Pipeline ( )
        {
            ResultBufferBlock =
                new BufferBlock < ILogInvocation > ( new DataflowBlockOptions ( ) { } ) ;
            _ = BuildPipeline ( ) ;
        }

        public virtual IPropagatorBlock < string , ILogInvocation > BuildPipeline ( )
        {
            var initWorkspace = Register ( DataflowBlocks.InitializeWorkspace ( ) ) ;

            IPropagatorBlock<string, string > cur = CurrentBlock ?? Register ( ConfigureInput ( ) ) ;
            cur.LinkTo ( initWorkspace , LinkOptions ) ;

            var toDocuments = Register ( Workspaces.SolutionDocumentsBlock ( ) ) ;
            
            initWorkspace.LinkTo ( toDocuments , LinkOptions ) ;
            var findLogUsages = Register(DataflowBlocks.FindLogUsages());
            toDocuments.LinkTo ( findLogUsages , LinkOptions ) ;
            findLogUsages.LinkTo ( Register ( ResultBufferBlock ), LinkOptions ) ;

            // Continuation ( clone ,             nameof ( clone ) ) ;
            // Continuation ( build ,             nameof ( build ) ) ; 
            // Continuation ( initWorkspace ,     nameof ( initWorkspace ) ) ;
            // Continuation ( toDocuments ,       nameof ( toDocuments ) ) ;
            // Continuation ( findLogUsages ,     nameof ( findLogUsages ) ) ;
            // Continuation ( ResultBufferBlock , nameof ( ResultBufferBlock ) ) ;

            PipelineInstance = DataflowBlock.Encapsulate ( Head , ResultBufferBlock ) ;
            return PipelineInstance ;
        }

        private T Register <T>( T block ) where T:IDataflowBlock
        {
            _dataflowBlocks.Add ( ( block ) ) ;
            Continuation ( block , block.ToString ( ) ) ;
            return block ;
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

        protected virtual IPropagatorBlock < string, string > ConfigureInput ( )
        {
            
            var input = new WriteOnceBlock < string > ( s => s ) ;
            Head = input ;
            return input ;
        }
        
        public ITargetBlock<string> Head { get ; set ; }
    }

    class PipelineRemoteSource : Pipeline
    {
        #region Overrides of PipeLine
        public override IPropagatorBlock < string , ILogInvocation > BuildPipeline ( )
        {
            var opt = LinkOptions ;

            var input = ConfigureInput ( ) ;
            var clone = DataflowBlocks.CloneSource();
            input.LinkTo(clone, opt);

            #if NUGET
            var build = DataflowBlocks.PackagesRestore();
            clone.LinkTo(build, opt);

            CurrentBlock = build ;
#endif

            return base.BuildPipeline ( ) ;
        }
        #endregion
    }
}
