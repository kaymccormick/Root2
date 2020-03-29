#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// AdaptedExplorerItem.cs
// 
// 2020-03-25-12:47 PM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.IO ;
using System.Linq ;
using System.Windows.Media ;
using AnalysisAppLib ;
using ExplorerCtrl ;
using JetBrains.Annotations ;

namespace ProjInterface
{
    public sealed class AdaptedExplorerItem : IExplorerItem
    {
        private readonly AppExplorerItem _itemImpl ;
        public AdaptedExplorerItem ( AppExplorerItem itemImpl ) { _itemImpl = itemImpl ; }
        #region Implementation of IExplorerItem
        public event EventHandler < RefreshEventArgs > Refresh ;

        public string Name { get { return _itemImpl.Name ; } }

        public string FullName { get { return _itemImpl.FullName ; } }

        public string Link { get { return _itemImpl.Link ; } }

        public long Size { get { return _itemImpl.Size ; } }

        public DateTime ? Date { get { return _itemImpl.Date ; } }

        public ExplorerItemType Type { get { return GetExplorerItemType ( _itemImpl ) ; } }

        private static ExplorerItemType GetExplorerItemType ( [ NotNull ] AppExplorerItem itemImpl )
        {
            if ( itemImpl.IsDirectory )
            {
                return ExplorerItemType.Directory ;
            }

            return ExplorerItemType.File ;
        }

        [ CanBeNull ] public ImageSource Icon { get { return GetItemImageSourceIcon ( _itemImpl ) ; } }

        [ CanBeNull ]
        private static ImageSource GetItemImageSourceIcon ( AppExplorerItem itemImpl )
        {
            return null ;
        }

        public bool IsDirectory { get { return _itemImpl.IsDirectory ; } }

        public bool HasChildren { get { return _itemImpl.HasChildren ; } }

        [ NotNull ] public IEnumerable < IExplorerItem > Children
        {
            get { return _itemImpl.Children.Select ( item => new AdaptedExplorerItem ( item ) ) ; }
        }

        public void Push ( Stream stream , string path ) { }

        public void Pull ( string path , Stream stream ) { }

        public void CreateFolder ( string path ) { }
        #endregion
    }
}