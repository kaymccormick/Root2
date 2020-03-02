#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// ProjectBrowserViewModoel.cs
// 
// 2020-03-01-6:32 PM
// 
// ---
#endregion
namespace ProjLib
{
    public class ProjectBrowserViewModoel : IProjectBrowserViewModoel
    {
        public ProjectBrowserViewModoel ( ) {
            var browserNodeCollection = new BrowserNodeCollection() ;
            _rootCollection = browserNodeCollection;
            var projectBrowserNode = new ProjectBrowserNode ( )
                                     {
                                         Name = "My Project",
                                         RepositoryUrl = "https://kaymccormick@dev.azure.com/kaymccormick/KayMcCormick.Dev/_git/KayMcCormick.Dev"
            } ;
            browserNodeCollection.Add ( projectBrowserNode ) ;
        }

        private IBrowserNodeCollection _rootCollection ;
        #region Implementation of IProjectBrowserViewModoel
        public IBrowserNodeCollection RootCollection { get => _rootCollection ; set => _rootCollection = value ; }
        #endregion
    }
}