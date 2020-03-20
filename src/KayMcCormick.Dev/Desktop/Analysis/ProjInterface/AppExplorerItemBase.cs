#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// AppExplorerItemBase.cs
// 
// 2020-03-20-5:06 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.IO ;
using System.Windows.Media ;
using ExplorerCtrl ;

namespace ProjInterface
{
    public abstract class AppExplorerItemBase : IExplorerItem
    {
        #region Implementation of IExplorerItem
        public abstract event EventHandler < RefreshEventArgs > Refresh ;

        public abstract string Name { get ; }

        public abstract string FullName { get ; }

        public abstract string Link { get ; }

        public abstract long Size { get ; }

        public abstract DateTime ? Date { get ; }

        public abstract ExplorerItemType Type { get ; }

        public abstract ImageSource Icon { get ; }

        public abstract bool IsDirectory { get ; }

        public abstract bool HasChildren { get ; }

        public abstract IEnumerable < IExplorerItem > Children { get ; }

        public abstract void Push ( Stream stream , string path ) ;
        public abstract void Pull ( string path , Stream stream ) ;
        public abstract void CreateFolder ( string path ) ;
        #endregion
    }
}