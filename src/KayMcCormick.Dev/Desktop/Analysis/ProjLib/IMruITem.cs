using System ;
using System.IO ;
using System.Linq ;
using NLog ;

namespace ProjLib
{
    public interface IMruItem
    {
        string FilePath { get ; }

        string Name { get ; }

        string Location { get  ; }

        FileInfo FileInf { get ; }

        bool Exists { get ; }

        string VerbatimPath { get ; }
    }

    public class MruItem : IMruItem
    {
        public static MruItem CreateInstance ( string filePath , string name )
        {
            return new MruItem ( filePath , name ) ;
        }

        public string FilePath { get ; set ; }

        public string Name { get ; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        private MruItem (  string filePath, string name)
        {
            try
            {
                VerbatimPath = filePath ;
                filePath = Environment.ExpandEnvironmentVariables ( filePath ) ;
                FileInf = new FileInfo ( filePath ) ;
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
            catch ( IOException ex )
            {
                FileInf = null ;
            }
            if (!FileInf.Exists)
            {
                FileInf = null;
            }

            Name = name ;

        }

        public string VerbatimPath { get ;  }

        public string Location { get ; set ; }

        public FileInfo FileInf { get ;  }

        public bool Exists => FileInf?.Exists ?? false ;
    }
}