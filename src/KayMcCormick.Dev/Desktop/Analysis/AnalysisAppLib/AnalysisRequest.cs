using AnalysisAppLib.Project ;

namespace AnalysisAppLib
{
    /// <summary>
    /// A request to the analysis service.
    /// </summary>
    public sealed class AnalysisRequest
    {
        private IProjectBrowserNode _projectInfo ;

        /// <summary>
        /// Related <see cref="IProjectBrowserNode"/>
        /// </summary>
        public IProjectBrowserNode Info
        {
            get { return _projectInfo ; }
            set { _projectInfo = value ; }
        }
    }

#if VERSIONCONTROL
    class PipelineRemoteSource : Pipeline
    {
        #region Overrides of PipeLine
        public override IPropagatorBlock < AnalysisRequest , ILogInvocation > BuildPipeline ( )
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
#endif
}