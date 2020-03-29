#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisControls
// ITypesViewModel.cs
// 
// 2020-03-11-7:06 PM
// 
// ---
#endregion
using AnalysisAppLib.Syntax ;
using KayMcCormick.Dev ;

namespace AnalysisAppLib.ViewModel
{
    /// <summary>
    /// </summary>
    public interface ITypesViewModel : IViewModel
    {
        /// <summary>
        /// </summary>
        AppTypeInfo Root { get ; set ; }

        /// <summary>
        /// </summary>
        bool ShowBordersIsChecked { get ; set ; }

        /// <summary>
        /// </summary>
        uint[] HierarchyColors { get ; set ; }
    }
}