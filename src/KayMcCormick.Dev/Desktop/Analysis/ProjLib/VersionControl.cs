using System.IO ;
using System.Threading.Tasks ;
using LibGit2Sharp ;
using NLog ;

namespace ProjLib
{
    static internal class VersionControl
    {
    
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static async Task < string > CloneProjectAsync ( string arg )
        {
            Logger.Info ( "Clone {arg}" , arg ) ;
            var workdirPath = Path.Combine ( FilePaths.ProjectRootDir , Path.GetRandomFileName ( ) ) ;

            var r = await Task.Run ( ( ) => Repository.Clone ( arg , workdirPath ) ) ;
            return workdirPath ;
        }
    }
}