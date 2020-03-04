#region header
// Kay McCormick (mccor)
// 
// ManagedProd
// AnalysisControls
// ICompilationViewModel.cs
// 
// 2020-03-03-3:39 PM
// 
// ---
#endregion
using System ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using AnalysisFramework ;
using Microsoft.CodeAnalysis ;
using ProjLib ;

namespace AnalysisControls
{
    public interface ICompilationViewModel : IViewModel, INotifyPropertyChanged
    {
        CodeAnalyseContext AnalyseContext { get ; set ; }

        object SelectedItem { get ; set ; }

        ControlFlowAnalysis CurrentControlFlowAnalysis { get ; set ; }

        Exception CurrentControlFlowAnalysisException { get ; set ; }

        void AnaylzeControlFlow ( object viewModelSelectedItem ) ;
        void GetDeclaredSymbol ( object viewModelSelectedItem ) ;

        ObservableCollection < ISymbol > ValueStack { get ; set ; }
    }
}