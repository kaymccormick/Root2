namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectAddedEventArgs
    {
        private ProjectModel projectModel;

        /// <summary>
        /// 
        /// </summary>
        public ProjectModel Model
        {
            get { return projectModel; }
            set { projectModel = value; }
        }
    }
}