#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisControls
// IComponentViewModel.cs
// 
// 2020-03-11-7:21 PM
// 
// ---
#endregion
using Microsoft.CodeAnalysis ;

namespace AnalysisAppLib.ViewModel
{
    public interface IComponentViewModel : IViewModel
    {
        SyntaxTree Tree { get ; }

        ICompilationUnitRootContext RootContext { get ; }
    }
}