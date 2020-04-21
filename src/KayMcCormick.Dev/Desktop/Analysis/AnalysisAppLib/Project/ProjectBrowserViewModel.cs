#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// ProjectBrowserViewModel.cs
// 
// 2020-03-01-6:32 PM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Runtime.Serialization ;

namespace AnalysisAppLib.Project
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
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
            // ReSharper disable once CollectionNeverQueried.Local
            List <ProjectInfo> projects = new List < ProjectInfo > ();
            var projectBrowserNode = new ProjectInfo
                                     {
                                         Name          = "LogTest"
                                       , RepositoryUrl = new Uri ( "none:" )
                                       , SolutionPath =
                                             // ReSharper disable once StringLiteralTypo
                                             @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v2\LogTest\LogTest.sln"
                                     } ;
            projects.Add ( projectBrowserNode ) ;
            //browserNodeCollection.Add ( projectBrowserNode ) ;
            var projectBrowserNode2 = new ProjectInfo
                                      {
                                          Name = "My Project (root2)"
                                        , RepositoryUrl =
                                              new Uri (
                                                       "https://kaymccormick@dev.azure.com/kaymccormick/KayMcCormick.Dev/_git/KayMcCormick.Dev"
                                                      )
                                        , SolutionPath =
                                              // ReSharper disable once StringLiteralTypo

                                              @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\Root2\src\KayMcCormick.Dev\ManagedProd.sln"
                                        , Platform = "x86"
                                      } ;
            //browserNodeCollection.Add ( projectBrowserNode2 ) ;
            projects.Add ( projectBrowserNode2 ) ;
            var projectBrowserNode3 = new ProjectInfo
                                      {
                                          Name = "My Project (root)"
                                        , RepositoryUrl =
                                              new Uri (
                                                       "https://kaymccormick@dev.azure.com/kaymccormick/KayMcCormick.Dev/_git/KayMcCormick.Dev"
                                                      )
                                        , SolutionPath =
                                              // ReSharper disable once StringLiteralTypo

                                              @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\work2\src\KayMcCormick.Dev\ManagedProd.sln"
                                        , Platform = "x86"
                                      } ;
            //browserNodeCollection.Add ( projectBrowserNode3 ) ;
            projects.Add (projectBrowserNode3  );
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
        public void GetObjectData ( SerializationInfo info , StreamingContext context )
        {

            info.AddValue ( "RootCollection" , _rootCollection.Select(node => node.Name).ToArray()) ;
        }
        #endregion
    }
}