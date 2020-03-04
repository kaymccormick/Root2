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
using AnalysisFramework ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;

namespace AnalysisControls
{
    class CompilationViewModel : ICompilationViewModel
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private CodeAnalyseContext  analyseContext ;
        private object              selectedItem ;
        private ControlFlowAnalysis currentControlFlowAnalysis ;
        private Exception currentControlFlowAnalysisException ;
        private ObservableCollection < ISymbol > valueStack = new ObservableCollection < ISymbol > ();
        #region Implementation of ICompilationViewModel
        public CompilationViewModel (  )
        {
            this.analyseContext = analyseContext ;
        }

        public CodeAnalyseContext AnalyseContext
        {
            get => analyseContext ;
            set
            {
                analyseContext = value ;
                OnPropertyChanged();
            }
        }

        public object SelectedItem
        {
            get => selectedItem ;
            set
            {
                selectedItem = value ;
                OnPropertyChanged();
            }
        }

        public void AnaylzeControlFlow ( object viewModelSelectedItem )
        {
            try
            {
                CurrentControlFlowAnalysisException = null ;
                var controlFlowAnalysis =
                    AnalyseContext.CurrentModel.AnalyzeControlFlow (
                                                                    viewModelSelectedItem as
                                                                        SyntaxNode
                                                                   ) ;
                CurrentControlFlowAnalysis = controlFlowAnalysis ;

            }
            catch ( Exception ex )
            {
                CurrentControlFlowAnalysisException = ex ;
                Logger.Error ( ex , ex.Message ) ;
            }

        }

        public void GetDeclaredSymbol ( object viewModelSelectedItem )
        {
            var symbol = AnalyseContext.CurrentModel.GetDeclaredSymbol ( viewModelSelectedItem as SyntaxNode ) ;
            ValueStack.Add ( symbol ) ;
            Logger.Debug ( "result i {symbmol} -{kind} " , symbol, symbol.Kind ) ;
        }

        public ObservableCollection < ISymbol > ValueStack { get => valueStack ; set => valueStack = value ; }

        public Exception CurrentControlFlowAnalysisException
        {
            get
            {
                return currentControlFlowAnalysisException ;
            }
            set
            {
                currentControlFlowAnalysisException = value ;
                OnPropertyChanged();
            }
        }

        public ControlFlowAnalysis CurrentControlFlowAnalysis
        {
            get
            {
                return currentControlFlowAnalysis ;
            }
            set
            {
                currentControlFlowAnalysis = value ;
                OnPropertyChanged();
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;  
        }
    }
}