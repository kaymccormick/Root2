#region header
// Kay McCormick (mccor)
// 
// ManagedProd
// AnalysisControls
// CompilationViewModel.cs
// 
// 2020-03-03-3:38 PM
// 
// ---
#endregion
using System ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Runtime.CompilerServices ;
using System.Runtime.Serialization ;
using AnalysisAppLib ;
using AnalysisControls.Interfaces ;
using AnalysisFramework ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using NLog ;

namespace AnalysisControls.ViewModels
{
    /// <summary></summary>
    /// <seealso cref="AnalysisControls.Interfaces.ICompilationViewModel" />
    public sealed class CompilationViewModel : ICompilationViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public ICompilationUnitRootContext CompilationUnitRootContext { get ; set ; }

        public ISyntaxTreeContext SyntaxTreeContext
        {
            get { return CompilationUnitRootContext ; }
        }

        private object              selectedItem ;
        private ControlFlowAnalysis currentControlFlowAnalysis ;
        private Exception           currentControlFlowAnalysisException ;

        private ObservableCollection < ISymbol > valueStack =
            new ObservableCollection < ISymbol > ( ) ;

        #region Implementation of ICompilationViewModel
        public ISemanticModelContext SemanticModelContext { get ; set ; }

        public object SelectedItem
        {
            get { return selectedItem ; }
            set
            {
                selectedItem = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public void AnaylzeControlFlow ( object viewModelSelectedItem , SemanticModel model )
        {
            try
            {
                CurrentControlFlowAnalysisException = null ;
                var controlFlowAnalysis =
                    model.AnalyzeControlFlow ( viewModelSelectedItem as SyntaxNode ?? throw new InvalidOperationException ( ) ) ;
                CurrentControlFlowAnalysis = controlFlowAnalysis ;
            }
            catch ( Exception ex )
            {
                CurrentControlFlowAnalysisException = ex ;
                Logger.Error ( ex , ex.Message ) ;
            }
        }

        public void GetDeclaredSymbol ( [ NotNull ] object viewModelSelectedItem , [ NotNull ] SemanticModel model )
        {
            if ( viewModelSelectedItem == null )
            {
                throw new ArgumentNullException ( nameof ( viewModelSelectedItem ) ) ;
            }

            var symbol = model.GetDeclaredSymbol ( viewModelSelectedItem as SyntaxNode ?? throw new InvalidOperationException ( ) ) ;
            ValueStack.Add ( symbol ) ;
            Logger.Debug ( "result i {symbmol} -{kind} " , symbol , symbol.Kind ) ;
        }

        public ObservableCollection < ISymbol > ValueStack
        {
            get { return valueStack ; }
            set { valueStack = value ; }
        }

        public Exception CurrentControlFlowAnalysisException
        {
            get { return currentControlFlowAnalysisException ; }
            set
            {
                currentControlFlowAnalysisException = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public ControlFlowAnalysis CurrentControlFlowAnalysis
        {
            get { return currentControlFlowAnalysis ; }
            set
            {
                currentControlFlowAnalysis = value ;
                OnPropertyChanged ( ) ;
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        #region Implementation of ISerializable
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }
}