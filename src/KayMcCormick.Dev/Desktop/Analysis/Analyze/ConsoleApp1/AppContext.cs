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
#if TERMUI
        private readonly TermUi _termUi ;
        public           TermUi Ui { get { return _termUi ; } }
        public AppContext (
            ILifetimeScope           scope
          , IProjectBrowserViewModel projectBrowserViewModel
          , TermUi                   termUi
        )
        {
            _termUi          = termUi ;
            Scope            = scope ;
            BrowserViewModel = projectBrowserViewModel ;
        }
#else
        public AppContext (
            ILifetimeScope           scope
      , IProjectBrowserViewModel projectBrowserViewModel
        )
        {
            Scope            = scope ;
            BrowserViewModel = projectBrowserViewModel ;
        }


#endif

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