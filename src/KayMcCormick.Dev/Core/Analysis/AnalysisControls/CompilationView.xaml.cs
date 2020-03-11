using System.Windows;
using System.Windows.Input;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using MigraDoc.DocumentObjectModel ;
using NLog ;
using ProjLib ;
using Document = MigraDoc.DocumentObjectModel.Document ;
using Paragraph = MigraDoc.DocumentObjectModel.Paragraph ;
using Section = MigraDoc.DocumentObjectModel.Section ;

namespace AnalysisControls
{
    /// <summary>
    /// Interaction logic for CompilationView.xaml
    /// </summary>
    public partial class CompilationView : Window, IView <ICompilationViewModel>, IView1
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private ICompilationViewModel viewModel ;

        [UsedImplicitly]
        public CompilationView(ICompilationViewModel viewModel/*, ICodeAnalyseContext codeAnalyseContext*/)
        {
            this.viewModel = viewModel ;
            // viewModel.CompilationUnitRootContext = codeAnalyseContext ;
            // viewModel.SemanticModelContext = codeAnalyseContext ;
            InitializeComponent();

            
            if ( ViewModel.CompilationUnitRootContext != null )
            {
                Document doc = new Document();
                var section = new Section();
                var paragraph = new Paragraph();
                paragraph.Format.Font = new Font("Fira Code", 14.0);
                paragraph.AddText (
                                   ViewModel.CompilationUnitRootContext.CompilationUnit
                                            .ToFullString ( )
                                  ) ;
                section.Add ( paragraph ) ;
                doc.Add ( section ) ;
                var s =
                    new MigraDoc.RtfRendering.RtfDocumentRenderer ( ).RenderToString (
                                                                                      doc
                                                                                    , @"C:\temp"
                                                                                     ) ;
                rtfCode.Text = s ;

                this.SyntaxPanel.ViewModel.CompilationUnitSyntax =
                    ViewModel.CompilationUnitRootContext.CompilationUnit ;
            }

            this.SyntaxPanel.ViewModel.PropertyChanged += ( sender , args ) => {
                Logger.Debug ( "PropertyChanged: {prop}" , args.PropertyName ) ;
                if ( args.PropertyName == "SelectedItem" )
                {
                    var modelSelectedItem = this.SyntaxPanel.ViewModel.SelectedItem ;
                    Logger.Debug("Selected item is {item}", modelSelectedItem);
                    var viewModelSelectedItem = modelSelectedItem as SyntaxNode ;
                    Logger.Debug("node is {item}", viewModelSelectedItem);
                    Logger.Debug("setting viewModel AnalyseContext Node to {item}", viewModelSelectedItem);
                    ViewModel.SelectedItem =
                        viewModelSelectedItem ;
                }
            } ;


        }

        #region Implementation of IView<ICompilationViewModel>
        public ICompilationViewModel ViewModel { get => viewModel ; set => viewModel = value ; }
        #endregion

        private void tv_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // var view = ( CollectionViewSource ) TryFindResource ( "Compilation" ) ;
            // view.View.MoveCurrentTo ( e.NewValue ) ;
        }

        private void CommandBinding_OnExecuted ( object sender , ExecutedRoutedEventArgs e )
        {
            ICompilationViewModel tempQualifier = ViewModel ;
            ViewModel.AnaylzeControlFlow ( ViewModel.SelectedItem , tempQualifier.SemanticModelContext.CurrentModel ) ;
            TabControl.SelectedItem = SemanticModelTab ;
        }

        private void CommandBinding_OnExecuted2 ( object sender , ExecutedRoutedEventArgs e )
        {
            ICompilationViewModel tempQualifier = ViewModel ;
            ViewModel.GetDeclaredSymbol ( ViewModel.SelectedItem , tempQualifier.SemanticModelContext.CurrentModel ) ;
        }

        #region Implementation of IView1
        public string ViewTitle => "Compilation View";
        #endregion
    }
}
