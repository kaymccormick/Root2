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
using System.Runtime.InteropServices ;
using System.Windows ;
using System.Windows.Interop ;
using System.Windows.Media ;
using System.Windows.Media.Imaging ;
using ExplorerCtrl ;
using Vanara.PInvoke ;
using Vanara.Windows.Shell ;
using Color = System.Drawing.Color ;
using Size = System.Drawing.Size ;

namespace ProjInterface
{
    public class ItemWrapper
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger ( ) ;
        private ShellFolder _folder ;
        private Image _folderImage ;

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
            try
            {
                ToolTipText = item.ToolTipText ;
            }
            catch ( COMException )
            {

            }

            if ( item is ShellFolder folder )
            {
                _folder = folder ;
                _folderImage = _folder.GetImage ( new Size ( 16 , 16 ) , ShellItemGetImageOptions.ResizeToFit ) ;
            }
#if false
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
#endif
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

        public Image FolderImage { get { return _folderImage ; } set { _folderImage = value ; } }
    }

    public class ShellExplorerItem : AppExplorerItem
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

        private List < ShellExplorerItem > _internalChildrenList ;
            

        private ItemWrapper _wrapper ;
        private object _extension ;

        public ShellExplorerItem ( object shellItem )
        {
            _shellItem = shellItem ;
            _wrapper   = new ItemWrapper ( ( ShellItem ) _shellItem ) ;
            if ( ! _wrapper.IsFolder
                 && _wrapper.FileInfo != null )
            {
                var icon = _wrapper.FileInfo.SmallIcon ;

                Bitmap bitmap = icon.ToBitmap ( ) ;
                IntPtr hBitmap = bitmap.GetHbitmap ( ) ;

                _icon = Imaging.CreateBitmapSourceFromHBitmap (
                                                               hBitmap
                                                             , IntPtr.Zero
                                                             , Int32Rect.Empty
                                                             , BitmapSizeOptions
                                                                  .FromEmptyOptions ( )
                                                              ) ;
            }
            else
            {
                var image = _wrapper.FolderImage ;
                if ( image is Bitmap bmp )
                {
                    var hbitmap = bmp.GetHbitmap ( Color.White ) ;
                    _icon = Imaging.CreateBitmapSourceFromHBitmap (
                                                                   hbitmap
                                                                 , IntPtr.Zero
                                                                 , Int32Rect.Empty
                                                                 , BitmapSizeOptions
                                                                      .FromEmptyOptions ( )
                                                                  ) ;
                }
            }

        }

        #region Overrides of AppExplorerItemBase
        public override event EventHandler < RefreshEventArgs > Refresh ;

        public override string Name { get { return _wrapper.Name ; } }

        public override string FullName { get { return _wrapper.ParsingName ; } }

        public override string Link { get { return _link ; } }

        public override long Size
        {
            get { return IsDirectory ? 0 : _wrapper.FileInfo?.Length ?? 0 ; }
        }

        public override DateTime ? Date { get { return _wrapper.FileInfo?.LastWriteTime ; } }

        public override ExplorerItemType Type
        {
            get
            {
                return _wrapper.IsFolder ? ExplorerItemType.Directory :
                       _wrapper.IsLink   ? ExplorerItemType.Link : ExplorerItemType.File ;
            }
            
        }

        public override ImageSource Icon { get { return _icon ; } }

        public override bool IsDirectory
        {
            get { return _wrapper.IsFolder ; }
        }

        public override bool HasChildren { get { return Children.Any ( ) ; } }

        public override IEnumerable < IExplorerItem > Children
        {
            get
            {
                if ( _internalChildrenList == null )
                {
                    if ( _shellItem is ShellFolder f )
                    {
                        _internalChildrenList = new List < ShellExplorerItem > ( ) ;
                        foreach ( var child in f.EnumerateChildren (
                                                                    FolderItemFilter.Folders
                                                                    | FolderItemFilter.NonFolders
                                                                   ) )
                        {
                            _internalChildrenList.Add ( new ShellExplorerItem ( child ) ) ;
                        }
                    }
                    else
                    {
                        return Enumerable.Empty < IExplorerItem > ( ) ;
                    }
                }

                return _internalChildrenList.Cast < IExplorerItem > ( ) ;
            }
        }

        public override object Extension { get { return _extension ; } }

        public override void Push ( Stream stream , string path ) { }

        public override void Pull ( string path , Stream stream ) { }

        public override void CreateFolder ( string path ) { }
#endregion

public void AddChild ( ShellExplorerItem shellExplorerItem )
{
    _internalChildrenList.Add ( shellExplorerItem ) ;
}
    }
}