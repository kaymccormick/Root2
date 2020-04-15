#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// IExplorerItemProvider.cs
// 
// 2020-03-20-5:21 AM
// 
// ---
#endregion
using System.Collections.Generic ;
using AnalysisAppLib.XmlDoc ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public interface IExplorerItemProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable < AppExplorerItem > GetRootItems ( ) ;
    }
}