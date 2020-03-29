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

namespace AnalysisAppLib
{
    public interface IExplorerItemProvider
    {
        IEnumerable < AppExplorerItem > GetRootItems ( ) ;
    }
}