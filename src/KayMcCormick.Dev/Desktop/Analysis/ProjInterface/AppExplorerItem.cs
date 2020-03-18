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
using System.Windows.Media ;
using ExplorerCtrl ;
using JetBrains.Annotations ;
using RefreshEventArgs = ExplorerCtrl.RefreshEventArgs ;

namespace ProjInterface
{
    public class AppExplorerItem : IExplorerItem, INotifyPropertyChanged
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
        private string _inputPath;
        private IDictionary _iconsResources ;
        private object _extension ;
        private IIconsSource _iconsSource ;

        public AppExplorerItem ( string inputPath , IIconsSource iconsSource )
        {
            this._inputPath = inputPath;
            _iconsSource = iconsSource ;
            FileAttributes f = File.GetAttributes ( inputPath ) ;
            FileInfo fi = new FileInfo(inputPath);
            if ( ( f & FileAttributes.Directory ) == FileAttributes.Directory )
            {
                IsDirectory = true ;
                Children = Directory.EnumerateFileSystemEntries( inputPath )
                                    .Select ( s => new AppExplorerItem ( s, _iconsSource ) ) ;
                HasChildren = Children.Any ( ) ;
                Type        = ExplorerItemType.Directory ;
            }
            else
            {
                Type = ExplorerItemType.File ;
                HasChildren = false ;
                Size = fi.Length;
                Extension = fi.Extension ;
            }

            Date = fi.LastWriteTime ;
            
            Name = fi.Name ;
            FullName = fi.FullName ;

        }
        #region Implementation of IExplorerItem
        public void Push ( Stream stream , string path ) { }

        public void Pull ( string path , Stream stream ) { }

        public void CreateFolder ( string path ) { }

        public string Name { get { return _name ; } set { _name = value ; } }

        public string FullName { get { return _fullName ; } set { _fullName = value ; } }

        public string Link { get { return _link ; } set { _link = value ; } }

        public long Size { get { return _size ; } set { _size = value ; } }

        public DateTime ? Date { get { return _date ; } set { _date = value ; } }

        public ExplorerItemType Type { get { return _type ; } set { _type = value ; } }

        public ImageSource Icon
        {
            get
            {
                if ( _icon == null )
                {
                    _icon = GetIconForExplorerItem ( this ) ;
                }
                return _icon;
            }
            set
            {
                _icon = value ;
            }
        }

        private ImageSource GetIconForExplorerItem ( AppExplorerItem appExplorerItem )
        {
            if ( appExplorerItem.IsDirectory )
            {
                var exts = new[] { new { ext = ".sln" }, new { ext = "csproj" } };

                var r =
                    from child in Children
                    let item = ( AppExplorerItem ) child
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
                return IconsSource.GetIconForFileExtension ( appExplorerItem.Extension ) ;
            }
        }

        public IIconsSource IconsSource { get { return _iconsSource ; } set { _iconsSource = value ; } }

        public object Extension { get { return _extension ; } set { _extension = value ; } }

        public bool IsDirectory { get { return _isDirectory ; } set { _isDirectory = value ; } }

        public bool HasChildren { get { return _hasChildren ; } set { _hasChildren = value ; } }

        public IEnumerable < IExplorerItem > Children { get { return _children ; } set { _children = value ; } }

        public IDictionary IconsResources
        {
            get { return _iconsResources ; }
            set
            {
                _iconsResources = value ;
            }
        }

        public event EventHandler < RefreshEventArgs > Refresh ;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}