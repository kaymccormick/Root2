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
using KayMcCormick.Dev ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace AnalysisAppLib.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISyntaxPanelViewModel : IViewModel, INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <summary>
        /// 
        /// </summary>
        CompilationUnitSyntax CompilationUnitSyntax { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        object SelectedItem { get ; set ; }
    }
}