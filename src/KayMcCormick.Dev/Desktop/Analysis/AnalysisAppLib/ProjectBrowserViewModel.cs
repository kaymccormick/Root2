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
using System ;
using System.Runtime.Serialization ;
using AnalysisAppLib.ViewModel ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectBrowserViewModel : IProjectBrowserViewModel
    {
        private readonly IBrowserNodeCollection _rootCollection ;

        /// <summary>
        /// 
        /// </summary>
        public ProjectBrowserViewModel ( )
        {
            var browserNodeCollection = new BrowserNodeCollection ( ) ;
            _rootCollection = browserNodeCollection ;
            var projectBrowserNode = new ProjectBrowserNode
                                     {
                                         Name          = "LogTest"
                                       , RepositoryUrl = new Uri ( "none:" )
                                       , SolutionPath =
                                             @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v2\LogTest\LogTest.sln"
                                     } ;
            browserNodeCollection.Add ( projectBrowserNode ) ;
            var projectBrowserNode2 = new ProjectBrowserNode
                                      {
                                          Name = "My Project (root2)"
                                        , RepositoryUrl =
                                              new Uri (
                                                       "https://kaymccormick@dev.azure.com/kaymccormick/KayMcCormick.Dev/_git/KayMcCormick.Dev"
                                                      )
                                        , SolutionPath =
                                              @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\Root2\src\KayMcCormick.Dev\ManagedProd.sln"
                                        , Platform = "x86"
                                      } ;
            browserNodeCollection.Add ( projectBrowserNode2 ) ;
            var projectBrowserNode3 = new ProjectBrowserNode
                                      {
                                          Name = "My Project (root)"
                                        , RepositoryUrl =
                                              new Uri (
                                                       "https://kaymccormick@dev.azure.com/kaymccormick/KayMcCormick.Dev/_git/KayMcCormick.Dev"
                                                      )
                                        , SolutionPath =
                                              @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\work2\src\KayMcCormick.Dev\ManagedProd.sln"
                                        , Platform = "x86"
                                      } ;
            browserNodeCollection.Add ( projectBrowserNode3 ) ;
        }

        #region Implementation of IProjectBrowserViewModoel
        /// <summary>
        /// 
        /// </summary>
        public IBrowserNodeCollection RootCollection { get { return _rootCollection ; } }
        #endregion
        #region Implementation of ISerializable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }
}