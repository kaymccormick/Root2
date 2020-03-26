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
#if USE_SHELL
using System ;
using System.Collections.Generic ;
using System.Drawing ;
using System.IO ;
using System.Linq ;
using System.Runtime.CompilerServices ;
using System.Runtime.InteropServices ;
using System.Text ;
using System.Windows ;
using System.Windows.Forms ;
using System.Windows.Interop ;
using System.Windows.Media ;
using System.Windows.Media.Imaging ;
using AnalysisAppLib ;
using ExplorerCtrl ;
using JetBrains.Annotations ;
using NLog ;
using NLog.Fluent ;
using Vanara.PInvoke ;
using Vanara.Windows.Shell ;
using Application = System.Windows.Application ;

namespace ProjInterface
{
    public class ItemWrapper
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly ShellItem _item ;

        // ReSharper disable once UnusedMember.Local
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        // ReSharper disable once NotAccessedField.Local
        private ShellFolder _folder ;
        private Image       _folderImage ;

        public ItemWrapper ( [ NotNull ] ShellItem item )
        {
            _item       = item ?? throw new ArgumentNullException ( nameof ( item ) ) ;
            Name        = item.Name ;
            ParsingName = item.ParsingName ;
            //var store = item.Properties ;
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

//                Logger.Info("{icon}", _folder.FileInfo.SmallIcon);

                // var view = _folder.GetViewObject < Shell32.IShellView > ( null ) ;

                // _folderImage = _folder.GetImage (
                // new Size ( 16 , 16 )
                // , ShellItemGetImageOptions.IconOnly
                // ) ;
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

        public string ToolTipText { get ; }

        public Shell32.PIDL PIDL { get ; }

        public bool IsLink { get ; }

        public ShellFolder Parent { get ; }

        public bool IsFolder { get ; }

        public Shell32.IShellItem IShellItem { get ; }

        public bool IsFileSystem { get ; }

        public string FileSystemPath { get ; }

        public ShellFileInfo FileInfo { get ; }

        public ShellItemAttribute Attributes { get ; }

        public Dictionary < object , object > Props { get ; }

        public string ParsingName { get ; }

        public string Name { get ; }

        // ReSharper disable once UnusedMember.Global
        public Image FolderImage { get { return _folderImage ; } set { _folderImage = value ; } }
    }

    public sealed class ShellExplorerItem : AppExplorerItem , IDisposable
    {
        private static readonly Logger      Logger = LogManager.GetCurrentClassLogger ( ) ;
        private readonly        ImageSource _fileInfoSmallIcon ;
        private readonly        object      _shellItem ;

        private string _link ;


        private List < ShellExplorerItem > _internalChildrenList ;


        private readonly ItemWrapper      _wrapper ;
        private          object           _extension ;
        private readonly ImageSource      icon1 ;
        private readonly User32.SafeHICON smallIcon ;
        private readonly User32.SafeHICON largeIcon ;
        private List<string> _pathSegments ;

        public ShellExplorerItem ( ShellExplorerItem parent , [ NotNull ] ShellItem shellItem )
        {
            
            _shellItem = shellItem ?? throw new ArgumentNullException ( nameof ( shellItem ) ) ;
            _wrapper   = new ItemWrapper ( ( ShellItem ) _shellItem ) ;
            if ( parent == null )
            {
                PathSegments = new List < string >
                               {
                                   shellItem.GetDisplayName ( ShellItemDisplayString.NormalDisplay )
                               } ;
            }
            else
            {
                PathSegments = parent.PathSegments
                                     .Append ( shellItem.GetDisplayName (ShellItemDisplayString.NormalDisplay ) ).ToList() ;
            }
        }

        public List<string> PathSegments { get { return _pathSegments ; } set { _pathSegments = value ; } }

        private ShellExplorerItem ( ShellExplorerItem parent, ShellItem shellItem , ImageSource fileInfoSmallIcon ) :
            this ( parent, shellItem )
        {
            _fileInfoSmallIcon = fileInfoSmallIcon ;
        }

        public ShellExplorerItem (
            ShellExplorerItem parent
          , ShellItem         shellItem
          , ImageSource       icon1
          , User32.SafeHICON  smallIcon
          , User32.SafeHICON  largeIcon
        ) : this (parent, shellItem )
        {
            this.icon1     = icon1 ;
            this.smallIcon = smallIcon ;
            this.largeIcon = largeIcon ;
        }

        private LogBuilder LB ( )
        {
            return new LogBuilder ( Logger ).LoggerName ( string.Join(".", PathSegments.Select(s => s.Replace(".", "_")))).Level ( LogLevel.Debug ) ;

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

        public override ImageSource Icon { get { return icon1 ?? _fileInfoSmallIcon ; } }

        public override bool IsDirectory { get { return _wrapper.IsFolder ; } }

        public override bool HasChildren
        {
            get
            {

                LB(  ).Message( nameof ( HasChildren ) ).Write();

                if ( _shellItem is ShellFolder f )
                {
                    return PopulateChildren ( f ) ;
                }
                else
                {
                    return false ;
                }
            }
        }

        public override IEnumerable < IExplorerItem > Children
        {
            get
            {

                LB(  ).Message(nameof( Children ) ).Write();

                if ( _internalChildrenList != null )
                {
                    return _internalChildrenList ;
                }

                if ( _shellItem is ShellFolder f )
                {
                    PopulateChildren ( f ) ;
                }
                else
                {
                    return Enumerable.Empty < IExplorerItem > ( ) ;
                }

                return _internalChildrenList ;
            }
        }

        private bool PopulateChildren (
            ShellFolder                 f
          , [ CallerFilePath ]   string callerFilePath   = null
          , [ CallerLineNumber ] int    callerLineNumber = 0
          , [ CallerMemberName ] string callerMemberName = null
        )
        {
            LB (  ).Message( "{method} {caller}:{line} ({callerMethod})"
                           , nameof ( PopulateChildren )
                           , callerFilePath
                           , callerLineNumber
                           , callerMemberName
                        ).Write() ;
            var win32Parent = new NativeWindow ( ) ;
            win32Parent.AssignHandle (
                                      new WindowInteropHelper ( Application.Current.MainWindow )
                                         .Handle
                                     ) ;
            if ( _internalChildrenList != null )
            {
                _internalChildrenList.Clear ( ) ;
            }
            else
            {
                _internalChildrenList = new List < ShellExplorerItem > ( ) ;
            }

            var children =
                f.EnumerateChildren ( FolderItemFilter.Folders | FolderItemFilter.NonFolders )
                 .ToArray ( ) ;

            Shell32.IExtractIcon view = null ;
            if ( children.Any ( ) )
            {
                try
                {
                    view = f.GetChildrenUIObjects < Shell32.IExtractIcon > (
                                                                            win32Parent
                                                                          , children
                                                                           ) ;
                }
                catch ( NotImplementedException )
                {
                }
                catch ( COMException )
                {
                }
                catch ( BadImageFormatException ) { }
                catch ( ArgumentException )
                {
                }

                foreach ( var child in children )
                {
                    _internalChildrenList.Add ( MakeItem (this, view , child ) ) ;
                }

                return _internalChildrenList.Any ( ) ;
            }
            else
            {
                return false ;
            }
        }

        public static ShellExplorerItem MakeItem (
            ShellExplorerItem    parent
          , Shell32.IExtractIcon view
          , ShellItem            child
          , ImageSource          fileInfoSmallIcon = null
        )
        {
            User32.SafeHICON smallIcon = null , largeIcon = null ;
            ImageSource icon1 = null ;
            try
            {
                if ( view != null )
                {
                    var sb = new StringBuilder ( 256 ) ;
                    var hr = view.GetIconLocation (
                                                   Shell32.GetIconLocationFlags.GIL_FORSHELL
                                                 , sb
                                                 , 256
                                                 , out var index2
                                                 , out var flags
                                                  ) ;

                    // Logger.Trace (
                                  // "Icon location is {file} {index} {flags:x}"
                                // , sb
                                // , index2
                                // , ( int ) flags
                                 // ) ;

                    if ( hr.Succeeded )
                    {
                        if ( ( flags & Shell32.GetIconLocationResultFlags.GIL_PERCLASS )
                             == Shell32.GetIconLocationResultFlags.GIL_PERCLASS )
                        {
                            var preexistingHandle =
                                ( ( child as ShellFolder )?.FileInfo?.SystemIcon
                                  ?? child.FileInfo?.SystemIcon )?.Handle
                                ?? IntPtr.Zero ;
                            BitmapSource icon ;

                            if ( preexistingHandle != IntPtr.Zero )
                            {
                                icon = GetIconBitmap (
                                                      new User32.SafeHICON ( preexistingHandle )
                                                     ) ;
                                return MakeItem (parent,  child , icon ) ;
                            }
                            else
                            {
                                return MakeItem ( parent, child ) ;
                            }
                        }

                        var hr2 = view.Extract (
                                                sb.ToString ( )
                                              , ( uint ) index2
                                              , out largeIcon
                                              , out smallIcon
                                              , 16 + 128 * 256
                                               ) ;
                        if ( hr2.Succeeded )
                        {
                            if ( smallIcon.IsInvalid )
                            {
                                throw new InvalidOperationException (
                                                                     $"Invalid icon for {child.Name}"
                                                                    ) ;
                            }

                            icon1 = GetIconBitmap ( smallIcon ) ;
                        }
                    }
                }
            }
            catch ( Exception ex )
            {
                new LogBuilder(Logger).Level(LogLevel.Error).Exception(ex).Message(ex.Message).Write();
            }

            if ( icon1 != null )
            {
                return new ShellExplorerItem (parent, child , icon1 , smallIcon , largeIcon ) ;
            }

            else
            {
                return new ShellExplorerItem ( parent, child ) ;
            }
        }

        public static ShellExplorerItem MakeItem (
            ShellExplorerItem parent,
            ShellItem   child
          , ImageSource fileInfoSmallIcon = null
        )
        {
            return new ShellExplorerItem ( parent, child , fileInfoSmallIcon ) ;
        }

        public static BitmapSource GetIconBitmap ( User32.SafeHICON smallIcon )
        {
            BitmapSource icon1 ;
            var bmp = smallIcon.ToBitmap ( ) ;
            var hbitmap = bmp.GetHbitmap ( ) ;
            icon1 = Imaging.CreateBitmapSourceFromHBitmap (
                                                           hbitmap
                                                         , IntPtr.Zero
                                                         , Int32Rect.Empty
                                                         , BitmapSizeOptions.FromEmptyOptions ( )
                                                          ) ;
            return icon1 ;
        }

        public override object Extension { get { return _extension ; } }

        public override void Push ( Stream stream , string path ) { }

        public override void Pull ( string path , Stream stream ) { }

        public override void CreateFolder ( string path ) { }
        #endregion

        // ReSharper disable once UnusedMember.Global
        public void AddChild ( ShellExplorerItem shellExplorerItem )
        {
            _internalChildrenList.Add ( shellExplorerItem ) ;
        }

        #region IDisposable
        public void Dispose ( )
        {
            smallIcon?.Dispose ( ) ;
            largeIcon?.Dispose ( ) ;
        }
        #endregion
    }
}
#endif