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
using System.Diagnostics ;
using System.Drawing ;
using System.IO ;
using System.Linq ;
using System.Windows ;
using System.Windows.Interop ;
using System.Windows.Media ;
using System.Windows.Media.Imaging ;
using ExplorerCtrl ;
using Vanara.Windows.Shell ;

namespace ProjInterface
{
    public class ItemWrapper
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger ( ) ;

        public ItemWrapper ( ShellItem item )
        {
            Name        = item.Name ;
            ParsingName = item.ParsingName ;
            var store = item.Properties ;
            Attributes     = item.Attributes ;
            Props          = new Dictionary < object , object > ( ) ;
            FileInfo       = item.FileInfo ;
            FileSystemPath = item.FileSystemPath ;
            IsFileSystem   = item.IsFileSystem ;
            IsFolder       = item.IsFolder ;
            IShellItem     = item.IShellItem ;
            IsLink         = item.IsLink ;
            Parent         = item.Parent ;
            PIDL           = item.PIDL ;
            ToolTipText    = item.ToolTipText ;

            foreach ( var key in store.Keys )
            {
                var propertyDescription = store.Descriptions[ key ] ;
                try
                {
                    var propertyDescriptionList = item.GetPropertyDescriptionList ( key ) ;
                    foreach ( var v in propertyDescriptionList )
                    {
                        Logger.Debug ( $"{v}" ) ;
                    }

                    //.Where(description => description.PropertyKey == key).First();
                    //Props[propertyDescription.CanonicalName] = store[key];
                }
                catch ( Exception e )
                {
                    Logger.Warn ( e , $"{key} - {e.Message}" ) ;
                }
            }
        }

        public string ToolTipText { get ; set ; }

        public Vanara.PInvoke.Shell32.PIDL PIDL { get ; set ; }

        public bool IsLink { get ; set ; }

        public ShellFolder Parent { get ; set ; }

        public bool IsFolder { get ; set ; }

        public Vanara.PInvoke.Shell32.IShellItem IShellItem { get ; set ; }

        public bool IsFileSystem { get ; set ; }

        public string FileSystemPath { get ; set ; }

        public ShellFileInfo FileInfo { get ; set ; }

        public ShellItemAttribute Attributes { get ; set ; }

        public Dictionary < object , object > Props { get ; set ; }

        public string ParsingName { get ; set ; }

        public string Name { get ; set ; }
    }

    public class ShellExplorerItem : AppExplorerItemBase
    {
        private readonly object           _shellItem ;
        private          string           _name ;
        private          string           _fullName ;
        private          string           _link ;
        private          long             _size ;
        private          DateTime ?       _date ;
        private          ExplorerItemType _type ;
        private          ImageSource      _icon ;
        private          bool             _isDirectory ;
        private          bool             _hasChildren ;

        private List < ShellExplorerItem > _internalChildrenList =
            new List < ShellExplorerItem > ( ) ;

        private ItemWrapper _wrapper ;

        public ShellExplorerItem ( object shellItem )
        {
            _shellItem = shellItem ;
            _wrapper   = new ItemWrapper ( ( ShellItem ) _shellItem ) ;
            var icon = _wrapper.FileInfo.SmallIcon ; 

            Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();

            _icon = 
                Imaging.CreateBitmapSourceFromHBitmap(
                                                      hBitmap, IntPtr.Zero, Int32Rect.Empty,
                                                      BitmapSizeOptions.FromEmptyOptions());

        }

        #region Overrides of AppExplorerItemBase
        public override event EventHandler < RefreshEventArgs > Refresh ;

        public override string Name { get { return _wrapper.Name ; } }

        public override string FullName { get { return _wrapper.ParsingName ; } }

        public override string Link { get { return _link ; } }

        public override long Size { get { return _wrapper.FileInfo.Length ; } }

        public override DateTime ? Date { get { return _wrapper.FileInfo.LastWriteTime ; } }

        public override ExplorerItemType Type
        {
            get
            {
                return _wrapper.IsFolder ? ExplorerItemType.Directory :
                       _wrapper.IsLink   ? ExplorerItemType.Link : ExplorerItemType.File ;
            }
            
        }

        public override ImageSource Icon { get { return _icon ; } }

        public override bool IsDirectory => _wrapper.IsFolder ;
        
        public override bool HasChildren { get { return Children.Any ( ) ; } }

        public override IEnumerable < IExplorerItem > Children
        {
            get { return _internalChildrenList.Cast < IExplorerItem > ( ) ; }
        }

        public override void Push ( Stream stream , string path ) { }

        public override void Pull ( string path , Stream stream ) { }

        public override void CreateFolder ( string path ) { }
        #endregion
    }
}