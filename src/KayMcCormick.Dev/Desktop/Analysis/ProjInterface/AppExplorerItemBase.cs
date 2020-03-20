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

        public abstract string Name { get ; set ; }

        public abstract string FullName { get ; set ; }

        public abstract string Link { get ; set ; }

        public abstract long Size { get ; set ; }

        public abstract DateTime ? Date { get ; set ; }

        public abstract ExplorerItemType Type { get ; set ; }

        public abstract ImageSource Icon { get ; set ; }

        public abstract bool IsDirectory { get ; set ; }

        public abstract bool HasChildren { get ; set ; }

        public abstract IEnumerable < IExplorerItem > Children { get ; set ; }

        public abstract void Push ( Stream stream , string path ) ;
        public abstract void Pull ( string path , Stream stream ) ;
        public abstract void CreateFolder ( string path ) ;
        #endregion
    }
}