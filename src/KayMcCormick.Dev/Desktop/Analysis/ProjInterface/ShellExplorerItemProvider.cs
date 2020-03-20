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
using Vanara.Windows.Shell ;

namespace ProjInterface
{
    public sealed class ShellExplorerItemProvider : IExplorerItemProvider
    {
        private ShellFolder _desktop ;

        public ShellExplorerItemProvider ( )
        {
            _desktop = ShellFolder.Desktop ;
            ShellExplorerItem item = new ShellExplorerItem(_desktop);
            foreach ( var shellItem in _desktop.EnumerateChildren (
                                                                   FolderItemFilter.Folders
                                                                   | FolderItemFilter.NonFolders
                                                                  ) )
            {
                
            }
        }
    }
}