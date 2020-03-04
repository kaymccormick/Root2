using System;
using System.Collections.Generic;
using System.Collections.Specialized ;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AnalysisFramework ;
using Autofac ;
using Microsoft.CodeAnalysis ;
using MigraDoc.DocumentObjectModel ;
using NLog ;
using ProjLib ;
using Document = MigraDoc.DocumentObjectModel.Document ;
using Paragraph = MigraDoc.DocumentObjectModel.Paragraph ;
using Section = MigraDoc.DocumentObjectModel.Section ;

namespace AnalysisControls
{
    public class AnalysisControlsModule : Module {
        #region Overrides of Module
        protected override void Load ( ContainerBuilder builder )
        {
            builder.RegisterType < CompilationView > ( ).AsSelf ( ) ;
            builder.RegisterType < CompilationViewModel > ( ).As < ICompilationViewModel > ( ) ;
        }
        #endregion
    }
    /// <summary>
    /// Interaction logic for CompilationView.xaml
    /// </summary>
    public partial class CompilationView : Window, IView <ICompilationViewModel>, IView1
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private ICompilationViewModel viewModel ;

        public CompilationView(ICompilationViewModel viewModel, CodeAnalyseContext codeAnalyseContext)
        {
            this.viewModel = viewModel ;
            viewModel.AnalyseContext = codeAnalyseContext ;
            InitializeComponent();

            MigraDoc.DocumentObjectModel.Document doc = new Document();
            var section = new Section() ;
            var paragraph = new Paragraph() ;
            paragraph.AddText ( ViewModel.AnalyseContext.CurrentRoot.ToFullString ( ) ) ;
            section.Add(paragraph);
            doc.Add (section );
            var s = new MigraDoc.RtfRendering.RtfDocumentRenderer ( ).RenderToString ( doc , @"C:\temp" ) ;
            rtfCode.Text = s ;

            this.SyntaxPanel.ViewModel.CompilationUnitSyntax = ViewModel.AnalyseContext.CurrentRoot ;
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
            ViewModel.AnaylzeControlFlow ( ViewModel.SelectedItem ) ;
            TabControl.SelectedItem = SemanticModelTab ;
        }

        private void CommandBinding_OnExecuted2 ( object sender , ExecutedRoutedEventArgs e )
        {
            ViewModel.GetDeclaredSymbol ( ViewModel.SelectedItem ) ;
        }
    }
}
