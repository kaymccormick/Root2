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

namespace AnalysisAppLib.Explorer
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class 
        // ReSharper disable once UnusedType.Global
        FileSystemExplorerItemProvider : IExplorerItemProvider
    {
        private readonly AppExplorerItem root ;

        /// <summary>
        /// 
        /// </summary>
        public FileSystemExplorerItemProvider ( )
        {
            root = new FileSystemAppExplorerItem ( DefaultInputPath ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public string DefaultInputPath { get ; } = Path.Combine (
                                                                 Environment.GetFolderPath (
                                                                                            Environment
                                                                                               .SpecialFolder
                                                                                               .UserProfile
                                                                                           )
                                                               , @"source\repos"
                                                                ) ;

        #region Implementation of IExplorerItemProvider
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable < AppExplorerItem > GetRootItems ( ) { yield return root ; }
        #endregion
    }
}