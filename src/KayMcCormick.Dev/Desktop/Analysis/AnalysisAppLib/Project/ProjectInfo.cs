#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// ProjectInfo.cs
// 
// 2020-04-21-2:34 PM
// 
// ---
#endregion
using System ;

namespace AnalysisAppLib.Project
{
    /// <summary>
    /// Project representation
    /// </summary>
    public class ProjectInfo
    {
        public int Id { get ; set ; }
        /// <summary>
        /// Project language
        /// </summary>
        public ProjectLanguage Language ;

        private Uri _repositoryUrl ;
        private string _platform ;

        /// <summary>
        /// PRoject name
        /// </summary>
        public string Name { get ; set ; }
        /// <summary>
        /// Solution file
        /// </summary>
        public string SolutionPath { get ; set ; }

        /// <summary>
        /// Repository url
        /// </summary>
        public Uri RepositoryUrl { get { return _repositoryUrl ; } set { _repositoryUrl = value ; } }

        /// <summary>
        /// Platform
        /// 
        /// </summary>
        public string Platform { get { return _platform ; } set { _platform = value ; } }
    }
}