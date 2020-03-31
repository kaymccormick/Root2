#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// IProjectBrowserViewModoel.cs
// 
// 2020-03-01-6:32 PM
// 
// ---
#endregion
using KayMcCormick.Dev ;

namespace AnalysisAppLib.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProjectBrowserViewModel : IViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        IBrowserNodeCollection RootCollection { get ; }
    }
}