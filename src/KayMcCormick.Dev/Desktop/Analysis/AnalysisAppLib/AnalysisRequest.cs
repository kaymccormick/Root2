namespace AnalysisAppLib
{
    /// <summary>
    /// </summary>
    public sealed class AnalysisRequest
    {
        private IProjectBrowserNode projectInfo ;

        /// <summary>
        /// </summary>
        public IProjectBrowserNode Info
        {
            get { return projectInfo ; }
            set { projectInfo = value ; }
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