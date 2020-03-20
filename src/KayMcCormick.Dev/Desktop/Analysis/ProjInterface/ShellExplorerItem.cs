#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// ShellExplorerItem.cs
// 
// 2020-03-20-5:07 AM
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
    public class ShellExplorerItem : AppExplorerItemBase
    {
        private string _name ;
        private string _fullName ;
        private string _link ;
        private long _size ;
        private DateTime ? _date ;
        private ExplorerItemType _type ;
        private ImageSource _icon ;
        private bool _isDirectory ;
        private bool _hasChildren ;
        private IEnumerable < IExplorerItem > _children ;
        #region Overrides of AppExplorerItemBase
        public override event EventHandler < RefreshEventArgs > Refresh ;

        public override string Name { get { return _name ; } set { _name = value ; } }

        public override string FullName { get { return _fullName ; } set { _fullName = value ; } }

        public override string Link { get { return _link ; } set { _link = value ; } }

        public override long Size { get { return _size ; } set { _size = value ; } }

        public override DateTime ? Date { get { return _date ; } set { _date = value ; } }

        public override ExplorerItemType Type { get { return _type ; } set { _type = value ; } }

        public override ImageSource Icon { get { return _icon ; } set { _icon = value ; } }

        public override bool IsDirectory { get { return _isDirectory ; } set { _isDirectory = value ; } }

        public override bool HasChildren { get { return _hasChildren ; } set { _hasChildren = value ; } }

        public override IEnumerable < IExplorerItem > Children { get { return _children ; } set { _children = value ; } }

        public override void Push ( Stream stream , string path ) { }

        public override void Pull ( string path , Stream stream ) { }

        public override void CreateFolder ( string path ) { }
        #endregion
    }
}