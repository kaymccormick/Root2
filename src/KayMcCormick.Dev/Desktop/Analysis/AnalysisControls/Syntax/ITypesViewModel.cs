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
using KayMcCormick.Dev.Interfaces ;

namespace AnalysisControls.Syntax
{
    public interface ITypesViewModel : IViewModel
    {
        AppTypeInfo Root { get ; set ; }
    }
}