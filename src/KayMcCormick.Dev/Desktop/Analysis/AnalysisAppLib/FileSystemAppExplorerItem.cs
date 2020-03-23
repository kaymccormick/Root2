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
using JetBrains.Annotations ;


namespace AnalysisAppLib
{
    // ReSharper disable once UnusedType.Global
    public sealed class FileSystemAppExplorerItem : AppExplorerItem , INotifyPropertyChanged
    {
        private string                        _name ;
        private string                        _fullName ;
        private string                        _link ;
        private long                          _size ;
        private DateTime ?                    _date ;
        
        
        private bool                          _isDirectory ;
        private bool                          _hasChildren ;
        private IEnumerable < FileSystemAppExplorerItem> _children ;
        private string                        _inputPath ;
        private IDictionary                   _iconsResources ;
        private object                        _extension ;
        
        
        private bool                          _isHidden ;
        private FileAttributes                _fileAttributes ;
        private object                        _extension1 ;
        private IEnumerable < AppExplorerItem > _children1 ;

        public FileSystemAppExplorerItem ( string inputPath  )
        {
            _inputPath      = inputPath ;
            _fileAttributes = File.GetAttributes ( inputPath ) ;
            var fi = new FileInfo ( inputPath ) ;
            if ( ( _fileAttributes & FileAttributes.Directory ) == FileAttributes.Directory )
            {
                _isDirectory = true ;
                _children = Directory.EnumerateFileSystemEntries ( inputPath )
                                     .Where ( s => ! s.StartsWith ( "." ) )
                                     .Select ( s => new FileSystemAppExplorerItem ( s  ) )
                                     .Where ( item => ! item.IsHidden ) ;
                _hasChildren = _children.Any ( ) ;
                // _type        = ExplorerItemType.Directory ;
            }
            else
            {
                // _type       = ExplorerItemType.File ;
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
        public override IEnumerable < AppExplorerItem > Children { get { return _children1 ; } }

        public override object Extension { get { return _extension1 ; } }

        public override void Push ( Stream stream , string path ) { }

        public override void Pull ( string path , Stream stream ) { }

        public override void CreateFolder ( string path ) { }


        public override string Name { get { return _name ; } }

        public override string FullName { get { return _fullName ; } }

        public override string Link { get { return _link ; } }

        public override long Size { get { return _size ; } }

        public override DateTime ? Date { get { return _date ; } }


        public override bool IsDirectory { get { return _isDirectory ; } }

        public override bool HasChildren { get { return _hasChildren ; } }
        
        public IDictionary IconsResources
        {
            get { return _iconsResources ; }
            set { _iconsResources = value ; }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMember.Local
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        public bool CanOpen { get ; } = true ;
        #region Implementation of INotifyPropertyChanged
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { }
            remove { }
        }
        #endregion
    }
}