using System ;
using System.IO ;

namespace ProjLib
{
    static internal class FilePaths
    {
        private static readonly string _projectRootDir = Path.Combine (
                                                                       Environment.GetFolderPath (
                                                                                                  Environment
                                                                                                     .SpecialFolder
                                                                                                     .MyDocuments
                                                                                                 )
                                                                     , "ProjectLib"
                                                                      ) ;

        public static string ProjectRootDir => _projectRootDir ;
    }
}