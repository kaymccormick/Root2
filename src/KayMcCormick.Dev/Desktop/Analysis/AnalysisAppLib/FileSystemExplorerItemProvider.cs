#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// FileSystemExplorerItemProvider.cs
// 
// 2020-03-20-5:55 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.IO ;

namespace AnalysisAppLib
{
    public class FileSystemExplorerItemProvider : IExplorerItemProvider
    {
        private readonly AppExplorerItem root ;

        public string DefaultInputPath { get ; } = Path.Combine (
                                                                 Environment.GetFolderPath (
                                                                                            Environment
                                                                                               .SpecialFolder
                                                                                               .UserProfile
                                                                                           )
                                                               , @"source\repos"
                                                                ) ;

        public FileSystemExplorerItemProvider ( ) {
            root = new FileSystemAppExplorerItem(DefaultInputPath);
        }

        #region Implementation of IExplorerItemProvider
        public IEnumerable < AppExplorerItem > GetRootItems ( ) { yield return root ; }
        #endregion
    }
}