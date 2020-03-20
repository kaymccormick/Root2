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
using System ;
using System.Collections.Generic ;
using Vanara.PInvoke ;
using Vanara.Windows.Shell ;

namespace ProjInterface
{
    public sealed class ShellExplorerItemProvider : IExplorerItemProvider , ITakesHwnd
    {
        private ShellFolder       _desktop ;
        private ShellExplorerItem _rootItem ;
        private IntPtr            _hWnd ;

        public ShellExplorerItemProvider ( )
        {
            _desktop = ShellFolder.Desktop ;
            // var view =
            //     _desktop.Parent.GetChildrenUIObjects < Shell32.IExtractIcon > (
            //                                                                    null
            //                                                                  , new[] { _desktop }
            //                                                                   ) ;
            var iconBItmap = ShellExplorerItem.GetIconBitmap (
                                             new User32.SafeHICON (
                                                                   _desktop.FileInfo.SmallIcon
                                                                           .Handle
                                                                  )
                                            ) ;
            _rootItem = ShellExplorerItem.MakeItem ( null, _desktop, iconBItmap) ;
        }

        #region Implementation of IExplorerItemProvider
        public IEnumerable < AppExplorerItem > GetRootItems ( ) { yield return _rootItem ; }
        #endregion
        #region Implementation of ITakesHwnd
        public void SetHwnd ( IntPtr hWnd ) { _hWnd = hWnd ; }
        #endregion
    }
}