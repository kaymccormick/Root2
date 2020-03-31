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
    /// <summary>
    /// 
    /// </summary>
    public abstract class AppExplorerItem
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract string Name { get ; }

        /// <summary>
        /// 
        /// </summary>
        public abstract string FullName { get ; }

        /// <summary>
        /// 
        /// </summary>
        public abstract string Link { get ; }

        /// <summary>
        /// 
        /// </summary>
        public abstract long Size { get ; }

        /// <summary>
        /// 
        /// </summary>
        public abstract DateTime ? Date { get ; }

        /// <summary>
        /// 
        /// </summary>
        public abstract bool IsDirectory { get ; }

        /// <summary>
        /// 
        /// </summary>
        public abstract bool HasChildren { get ; }

        /// <summary>
        /// 
        /// </summary>
        public abstract IEnumerable < AppExplorerItem > Children { get ; }

        /// <summary>
        /// 
        /// </summary>
        public abstract object Extension { get ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="path"></param>
        public abstract void Push ( Stream         stream , string path ) ;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="stream"></param>
        public abstract void Pull ( string         path ,   Stream stream ) ;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public abstract void CreateFolder ( string path ) ;
    }
}