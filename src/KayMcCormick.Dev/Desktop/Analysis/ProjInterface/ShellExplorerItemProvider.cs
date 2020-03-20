#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// ShellExplorerItemProvider.cs
// 
// 2020-03-20-5:22 AM
// 
// ---
#endregion
using System.Collections.Generic ;
using Vanara.Windows.Shell ;

namespace ProjInterface
{
    public sealed class ShellExplorerItemProvider : IExplorerItemProvider
    {
        private ShellFolder _desktop ;
        private ShellExplorerItem _rootItem ;

        public ShellExplorerItemProvider ( )
        {
            _desktop = ShellFolder.Desktop ;
            _rootItem = new ShellExplorerItem(_desktop) ;
        }

        #region Implementation of IExplorerItemProvider
        public IEnumerable < AppExplorerItem > GetRootItems ( ) { yield return _rootItem; }
        #endregion
    }
}