using System ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.IO ;
using System.Linq ;
using System.Runtime.CompilerServices ;
using JetBrains.Annotations ;

namespace ProjLib
{
    public interface IMruItem
    {
        string FilePath { get ; }

        string Name { get ; }

        string Location { get ; }

        FileInfo FileInf { get ; }

        bool Exists { get ; }

        string VerbatimPath { get ; }

        ObservableCollection< IProjectInfo > ProjectCollection { get ; }
    }

    public interface IProjectInfo : IItemInfo
    {
    }

    public interface IItemInfo
    {
        string Name { get ; }
    }

    public class MruItem : IMruItem , INotifyPropertyChanged
    {
        private ObservableCollection< IProjectInfo > _projectCollection = new ObservableCollection<IProjectInfo>();

        public string FilePath { get ; set ; }

        public string Name { get ; }

        
        public MruItem ( string filePath , string name )
        {
            try
            {
                VerbatimPath = filePath ;
                filePath     = Environment.ExpandEnvironmentVariables ( filePath ) ;
                FileInf      = new FileInfo ( filePath ) ;
                if ( FileInf != null )
                {
                    var strings = FileInf.Directory.FullName.Split ( Path.DirectorySeparatorChar ) ;
                    var p = strings.ToList ( )
                                   .GetRange (
                                              strings.Length < 3 ? 0 : strings.Length - 3
                                            , strings.Length >= 3 ? 3 : strings.Length
                                             ) ;
                    Location = string.Join ( Path.DirectorySeparatorChar.ToString ( ) , p ) ;
                }

                FilePath = filePath ;
            }
            catch ( IOException )
            {
                FileInf = null ;
            }

            if ( ! FileInf.Exists )
            {
                FileInf = null ;
            }

            Name = name ;
        }

        public string VerbatimPath { get ; }

        
        public string Location { get ; set ; }

        public FileInfo FileInf { get ; }

        public bool Exists => FileInf?.Exists ?? false ;

        public ObservableCollection < IProjectInfo > ProjectCollection
        {
            get => _projectCollection ;
            private set => _projectCollection = value ;
        }

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}