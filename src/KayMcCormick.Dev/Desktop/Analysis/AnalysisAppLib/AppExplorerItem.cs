#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// AppExplorerItemBase.cs
// 
// 2020-03-20-5:06 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.IO ;

namespace AnalysisAppLib
{
    public abstract class AppExplorerItem
    {
        public abstract string Name { get ; }

        public abstract string FullName { get ; }

        public abstract string Link { get ; }

        public abstract long Size { get ; }

        public abstract DateTime ? Date { get ; }

        public abstract bool IsDirectory { get ; }

        public abstract bool HasChildren { get ; }

        public abstract IEnumerable < AppExplorerItem > Children { get ; }

        public abstract object Extension { get ; }

        public abstract void Push ( Stream         stream , string path ) ;
        public abstract void Pull ( string         path ,   Stream stream ) ;
        public abstract void CreateFolder ( string path ) ;
    }
}