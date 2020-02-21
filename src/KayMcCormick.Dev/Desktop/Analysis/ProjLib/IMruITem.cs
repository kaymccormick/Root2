using System.IO ;
using System.Linq ;

namespace ProjLib
{
    public interface IMruItem
    {
        string FilePath { get ; }

        string Name { get ; }

        string Location { get ; set ; }

        FileInfo FileInf { get ; }
    }

    public class MruItem : IMruItem
    {
        public string FilePath { get ; set ; }

        public string Name { get ; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public MruItem (  string filePath, string name)
        {
            FileInf = new FileInfo(filePath);
            if ( FileInf != null )
            {
                var strings = FileInf.Directory.FullName.Split ( Path.PathSeparator ) ;
                var p = strings.ToList ( ).GetRange ( strings.Length - 4  , 3) ;
                Location = string.Join ( Path.PathSeparator.ToString(), p);
            }
            FilePath = filePath ;
            Name = name ;
        }

        public string Location { get ; set ; }

        public FileInfo FileInf { get ;  }
    }
}