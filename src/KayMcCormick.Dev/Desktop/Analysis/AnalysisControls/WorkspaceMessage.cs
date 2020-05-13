namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkspaceMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public WorkspaceMessageSeverity Severity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object Source { get; set; }
    }
}