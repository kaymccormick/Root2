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
        public WorkspaceMessage()
        {
        }
        public string ProjectName { get; set; }
        public string Message { get; set; }
        public object Source { get; set; }
    }
}