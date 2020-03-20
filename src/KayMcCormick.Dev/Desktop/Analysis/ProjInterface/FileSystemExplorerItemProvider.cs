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
using System.Collections.Generic ;

namespace ProjInterface
{
    public class FileSystemExplorerItemProvider : IExplorerItemProvider
    {
        #region Implementation of IExplorerItemProvider
        public IEnumerable < AppExplorerItem > GetRootItems ( ) { yield break ; }
        #endregion
    }
}