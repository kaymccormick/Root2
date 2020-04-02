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
using KayMcCormick.Dev ;

namespace AnalysisControls
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