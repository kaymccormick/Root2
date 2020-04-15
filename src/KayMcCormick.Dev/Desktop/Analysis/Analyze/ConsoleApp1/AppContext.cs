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
using AnalysisAppLib.XmlDoc.Project ;
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
        private Options _options ;

        //public IEnumerable < Meta < Lazy < IAnalyzeCommand2 > > > AnalyzeCommands { get ; }

        public ILifetimeScope Scope { get ; }

        public IProjectBrowserViewModel BrowserViewModel
        {
            // ReSharper disable once UnusedMember.Global
            get { return _projectBrowserViewModel ; }
            set { _projectBrowserViewModel = value ; }
        }

        public Options Options { get { return _options ; } set { _options = value ; } }
    }
}