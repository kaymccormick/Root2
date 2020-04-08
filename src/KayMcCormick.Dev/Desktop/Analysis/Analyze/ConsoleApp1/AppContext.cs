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
using AnalysisAppLib.Project ;
using Autofac ;

namespace ConsoleApp1
{
    internal sealed class AppContext
    {
        public AppContext (
            ILifetimeScope           scope
      , IProjectBrowserViewModel projectBrowserViewModel
        )
        {
            Scope            = scope ;
            BrowserViewModel = projectBrowserViewModel ;
        }



        private IProjectBrowserViewModel _projectBrowserViewModel ;

        //public IEnumerable < Meta < Lazy < IAnalyzeCommand2 > > > AnalyzeCommands { get ; }

        public ILifetimeScope Scope { get ; }

        public IProjectBrowserViewModel BrowserViewModel
        {
            // ReSharper disable once UnusedMember.Global
            get { return _projectBrowserViewModel ; }
            set { _projectBrowserViewModel = value ; }
        }
    }
}