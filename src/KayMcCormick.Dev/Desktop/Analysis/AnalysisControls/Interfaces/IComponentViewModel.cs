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
using AnalysisFramework ;
using KayMcCormick.Dev ;
using Microsoft.CodeAnalysis ;

namespace AnalysisControls.Interfaces
{
    public interface IComponentViewModel : IViewModel
    {
        SyntaxTree Tree { get ; }

        ICompilationUnitRootContext RootContext { get ; }
    }
}