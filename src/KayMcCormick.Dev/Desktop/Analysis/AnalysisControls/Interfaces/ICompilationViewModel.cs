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
using AnalysisAppLib ;
using AnalysisFramework ;
using KayMcCormick.Dev ;
using Microsoft.CodeAnalysis ;

namespace AnalysisControls.Interfaces
{
    public interface ICompilationViewModel : IViewModel, INotifyPropertyChanged
    {
        ISemanticModelContext SemanticModelContext { get ; set ; }

        object SelectedItem { get ; set ; }

        ControlFlowAnalysis CurrentControlFlowAnalysis { get ; set ; }

        Exception CurrentControlFlowAnalysisException { get ; set ; }

        void AnaylzeControlFlow ( object viewModelSelectedItem , SemanticModel model ) ;
        void GetDeclaredSymbol ( object viewModelSelectedItem , SemanticModel model ) ;
        ObservableCollection < ISymbol > ValueStack { get ; set ; }

        ISyntaxTreeContext SyntaxTreeContext { get ; }

        ICompilationUnitRootContext CompilationUnitRootContext { get ; set ; }
    }
}