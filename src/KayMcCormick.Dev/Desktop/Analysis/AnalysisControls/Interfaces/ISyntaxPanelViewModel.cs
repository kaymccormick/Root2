#region header
// Kay McCormick (mccor)
// 
// ManagedProd
// AnalysisControls
// ISyntaxPanelViewModel.cs
// 
// 2020-03-03-2:17 PM
// 
// ---
#endregion
using System.ComponentModel ;
using KayMcCormick.Dev.Interfaces ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace AnalysisControls.Interfaces
{
    public interface ISyntaxPanelViewModel : IViewModel, INotifyPropertyChanged, INotifyPropertyChanging
    {
        CompilationUnitSyntax CompilationUnitSyntax { get ; set ; }

        object SelectedItem { get ; set ; }
    }
}