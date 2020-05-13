#region header
// Kay McCormick (mccor)
// 
// Analysis
// ConsoleApp1
// AppContext.cs
// 
// 2020-04-08-5:32 AM
// 
// ---
#endregion

using AnalysisAppLib.Project;
using Autofac;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AppContext
    {
        private readonly AppDbContextHelper _helper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="projectBrowserViewModel"></param>
        /// <param name="helper"></param>
        public AppContext (
            ILifetimeScope           scope
      , IProjectBrowserViewModel projectBrowserViewModel,
            AppDbContextHelper helper
        )
        {
            _helper = helper;
            Scope            = scope ;
            BrowserViewModel = projectBrowserViewModel ;
        }



        private IProjectBrowserViewModel _projectBrowserViewModel ;
        //private Options _options ;

        //public IEnumerable < Meta < Lazy < IAnalyzeCommand2 > > > AnalyzeCommands { get ; }

        /// <summary>
        /// 
        /// </summary>
        public ILifetimeScope Scope { get ; }

        public IProjectBrowserViewModel BrowserViewModel
        {
            // ReSharper disable once UnusedMember.Global
            get { return _projectBrowserViewModel ; }
            set { _projectBrowserViewModel = value ; }
        }

        //public Options Options { get { return _options ; } set { _options = value ; } }
    }
}