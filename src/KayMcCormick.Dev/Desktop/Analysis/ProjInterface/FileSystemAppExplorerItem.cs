#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// AppExplorerItem.cs
// 
// 2020-03-17-2:25 PM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.IO ;
using System.Linq ;
using System.Runtime.CompilerServices ;
using System.Windows.Controls ;
using System.Windows.Media ;
using ExplorerCtrl ;
using JetBrains.Annotations ;
using RefreshEventArgs = ExplorerCtrl.RefreshEventArgs ;

namespace ProjInterface
{
    public class FileSystemAppExplorerItem : AppExplorerItem , INotifyPropertyChanged
    {
        private string                        _name ;
        private string                        _fullName ;
        private string                        _link ;
        private long                          _size ;
        private DateTime ?                    _date ;
        private ExplorerItemType              _type ;
        private ImageSource                   _icon ;
        private bool                          _isDirectory ;
        private bool                          _hasChildren ;
        private IEnumerable < IExplorerItem > _children ;
        private string                        _inputPath ;
        private IDictionary                   _iconsResources ;
        private object                        _extension ;
        private IIconsSource                  _iconsSource ;
        private Image                         _iconImage ;
        private bool                          _isHidden ;
        private FileAttributes                _fileAttributes ;
        private object                        _extension1 ;

        public FileSystemAppExplorerItem ( string inputPath , IIconsSource iconsSource )
        {
            _inputPath      = inputPath ;
            _iconsSource    = iconsSource ;
            _fileAttributes = File.GetAttributes ( inputPath ) ;
            var fi = new FileInfo ( inputPath ) ;
            if ( ( _fileAttributes & FileAttributes.Directory ) == FileAttributes.Directory )
            {
                _isDirectory = true ;
                _children = Directory.EnumerateFileSystemEntries ( inputPath )
                                     .Where ( s => ! s.StartsWith ( "." ) )
                                     .Select ( s => new FileSystemAppExplorerItem ( s , _iconsSource ) )
                                     .Where ( item => ! item.IsHidden ) ;
                _hasChildren = Children.Any ( ) ;
                _type        = ExplorerItemType.Directory ;
            }
            else
            {
                _type       = ExplorerItemType.File ;
                _size       = fi.Length ;
                _extension1 = fi.Extension ;
            }

            _date = fi.LastWriteTime ;

            _name = fi.Name ;
            _fullName = fi.FullName ;
        }

        // ReSharper disable once ArrangeAccessorOwnerBody
        public bool IsHidden
            => ( _fileAttributes & FileAttributes.Hidden ) == FileAttributes.Hidden ;

        #region Implementation of IExplorerItem
        public override object Extension { get { return _extension1 ; } }

        public override void Push ( Stream stream , string path ) { }

        public override void Pull ( string path , Stream stream ) { }

        public override void CreateFolder ( string path ) { }

        public override event EventHandler < RefreshEventArgs > Refresh ;

        public override string Name { get { return _name ; } }

        public override string FullName { get { return _fullName ; } }

        public override string Link { get { return _link ; } }

        public override long Size { get { return _size ; } }

        public override DateTime ? Date { get { return _date ; } }

        public override ExplorerItemType Type { get { return _type ; } }

        public override ImageSource Icon
        {
            get
            {
                if ( _icon == null )
                {
                    _icon = GetIconImageSourceForExplorerItem ( this ) ;
                }

                return _icon ;
            }
        }

        private ImageSource GetIconImageSourceForExplorerItem ( FileSystemAppExplorerItem fileSystemAppExplorerItem )
        {
            if ( fileSystemAppExplorerItem.IsDirectory )
            {
                var exts = new[] { new { ext = ".sln" } , new { ext = "csproj" } } ;

                var r =
                    from child in fileSystemAppExplorerItem.Children
                    let item = ( FileSystemAppExplorerItem ) child
                    join x in exts on item.Extension equals x.ext
                    select item ;
                if ( r.Any ( ) )
                {
                    return IconsSource?.ProjectDirectoryIconImageSource ;
                }

                return IconsSource?.DirectoryIconImageSource ;
            }
            else
            {
                return IconsSource?.GetIconForFileExtension ( fileSystemAppExplorerItem.Extension ) ;
            }
        }

        public Image IconImage
        {
            get
            {
                if ( _iconImage == null )
                {
                    _iconImage = GetIconImageForExplorerItem ( this ) ;
                }

                return _iconImage ;
            }
            set { _iconImage = value ; }
        }

        private Image GetIconImageForExplorerItem ( FileSystemAppExplorerItem fileSystemAppExplorerItem )
        {
            if ( fileSystemAppExplorerItem.IsDirectory )
            {
                var exts = new[] { new { ext = ".sln" } , new { ext = "csproj" } } ;

                var r =
                    from child in fileSystemAppExplorerItem.Children
                    let item = ( FileSystemAppExplorerItem ) child
                    join x in exts on item.Extension equals x.ext
                    select item ;
                if ( r.Any ( ) )
                {
                    return IconsSource.ProjectDirectoryIcon ;
                }

                return IconsSource.DirectoryIcon ;
            }
            else
            {
                return new Image
                       {
                           Source = IconsSource.GetIconForFileExtension (
                                                                         fileSystemAppExplorerItem.Extension
                                                                        )
                       } ;
            }
        }

        public IIconsSource IconsSource
        {
            get { return _iconsSource ; }
            set { _iconsSource = value ; }
        }

        public override bool IsDirectory { get { return _isDirectory ; } }

        public override bool HasChildren { get { return _hasChildren ; } }

        public override IEnumerable < IExplorerItem > Children { get { return _children ; } }

        public IDictionary IconsResources
        {
            get { return _iconsResources ; }
            set { _iconsResources = value ; }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        #region Implementation of INotifyPropertyChanged
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { }
            remove { }
        }
        #endregion
    }
}