using System.Collections.ObjectModel ;
using System.IO ;

namespace ProjLib.Interfaces
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
}