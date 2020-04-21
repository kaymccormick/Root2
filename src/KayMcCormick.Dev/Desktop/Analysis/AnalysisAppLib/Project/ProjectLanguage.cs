#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// ProjectLanguage.cs
// 
// 2020-04-21-2:36 PM
// 
// ---
#endregion
namespace AnalysisAppLib.Project
{
    /// <summary>
    /// Project Language
    /// </summary>
    public class ProjectLanguage
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int Id { get ; set ; }
        /// <summary>
        /// Name of project
        /// </summary>
        public string ProjectName { get ; set ; }
        /// <summary>
        /// Solution path
        /// </summary>
        public string SolutionPath { get ; set ; }
    }
}