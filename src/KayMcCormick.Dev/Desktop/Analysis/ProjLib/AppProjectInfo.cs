#region header
// Kay McCormick (mccor)
// 
// Proj
// ProjLib
// AppProjectInfo.cs
// 
// 2020-02-27-10:02 AM
// 
// ---
#endregion
using ProjLib.Interfaces ;

namespace ProjLib
{
    // mru related
    public class AppProjectInfo : IProjectInfo
    {
        public string Name { get ; }

        public string InfoFilePath { get ; }

        public int DocumentsCount { get ; }

        public AppProjectInfo ( string name , string infoFilePath , int documentsCount )
        {
            Name           = name ;
            InfoFilePath   = infoFilePath ;
            DocumentsCount = documentsCount ;
        }
    }
}