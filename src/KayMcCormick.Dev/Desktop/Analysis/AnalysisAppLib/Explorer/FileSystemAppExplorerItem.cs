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

namespace AnalysisAppLib.Explorer
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FileSystemAppExplorerItem : AppExplorerItem , INotifyPropertyChanged
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly IEnumerable < FileSystemAppExplorerItem > _children ;
#pragma warning disable 649
        private          IEnumerable < AppExplorerItem >           _children1 ;
#pragma warning restore 649
        private readonly DateTime ?                                _date ;
#pragma warning disable 169
        private          object                                    _extension ;
#pragma warning restore 169
        private readonly object                                    _extension1 ;
        private readonly FileAttributes                            _fileAttributes ;
        private readonly string                                    _fullName ;
        private readonly bool                                      _hasChildren ;
        private          IDictionary                               _iconsResources ;
        // ReSharper disable once NotAccessedField.Local
        private          string                                    _inputPath ;


        private readonly bool _isDirectory ;


#pragma warning disable 169
        private          bool   _isHidden ;
#pragma warning restore 169
#pragma warning disable 649
        private          string _link ;
#pragma warning restore 649
        private readonly string _name ;
        private readonly long   _size ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputPath"></param>
        // ReSharper disable once AnnotateNotNullParameter
        public FileSystemAppExplorerItem ( string inputPath )
        {
            _inputPath      = inputPath ;
            _fileAttributes = File.GetAttributes ( inputPath ) ;
            var fi = new FileInfo ( inputPath ) ;
            if ( ( _fileAttributes & FileAttributes.Directory ) == FileAttributes.Directory )
            {
                _isDirectory = true ;
                _children = Directory.EnumerateFileSystemEntries ( inputPath )
                                     .Where ( s => ! s.StartsWith ( "." , StringComparison.Ordinal ) )
                                     .Select ( s => new FileSystemAppExplorerItem ( s ) )
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

            _name     = fi.Name ;
            _fullName = fi.FullName ;
        }


        /// <summary>
        /// 
        /// </summary>
        public bool IsHidden
        {
            get { return ( _fileAttributes & FileAttributes.Hidden ) == FileAttributes.Hidden ; }
        }

        private const bool CanOpen = true ;

        /// <summary>
        /// 
        /// </summary>
        public bool GetCanOpen()
        {
            return CanOpen;
        }

        #region Implementation of INotifyPropertyChanged
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { }
            remove { }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once EventNeverSubscribedTo.Global
        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        // ReSharper disable once UnusedMember.Local
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        #region Implementation of IExplorerItem
        /// <summary>
        /// 
        /// </summary>
        public override IEnumerable < AppExplorerItem > Children { get { return _children1 ; } }

        /// <summary>
        /// 
        /// </summary>
        public override object Extension { get { return _extension1 ; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="path"></param>
        public override void Push ( Stream stream , string path ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="stream"></param>
        public override void Pull ( string path , Stream stream ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public override void CreateFolder ( string path ) { }


        /// <summary>
        /// 
        /// </summary>
        public override string Name { get { return _name ; } }

        /// <summary>
        /// 
        /// </summary>
        public override string FullName { get { return _fullName ; } }

        /// <summary>
        /// 
        /// </summary>
        public override string Link { get { return _link ; } }

        /// <summary>
        /// 
        /// </summary>
        public override long Size { get { return _size ; } }

        /// <summary>
        /// 
        /// </summary>
        public override DateTime ? Date { get { return _date ; } }


        /// <summary>
        /// 
        /// </summary>
        public override bool IsDirectory { get { return _isDirectory ; } }

        /// <summary>
        /// 
        /// </summary>
        public override bool HasChildren { get { return _hasChildren ; } }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public IDictionary IconsResources
        {
            get { return _iconsResources ; }
            set { _iconsResources = value ; }
        }
        #endregion
    }
}