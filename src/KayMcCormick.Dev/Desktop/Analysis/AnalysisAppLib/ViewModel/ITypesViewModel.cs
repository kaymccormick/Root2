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

namespace AnalysisAppLib.ViewModel
{
    public interface ITypesViewModel : IViewModel
    {
        AppTypeInfo Root { get ; set ; }

        bool ShowBordersIsChecked { get ; set ; }

        uint[] HierarchyColors { get ; set ; }
    }
}