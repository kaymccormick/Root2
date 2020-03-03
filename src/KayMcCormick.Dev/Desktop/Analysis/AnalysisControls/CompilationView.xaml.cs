using System;
using System.Collections.Generic;
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
using ProjLib ;

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
        private ICompilationViewModel viewModel ;

        public CompilationView(ICompilationViewModel viewModel, CodeAnalyseContext codeAnalyseContext)
        {
            this.viewModel = viewModel ;
            viewModel.AnalyseContext = codeAnalyseContext ;
            InitializeComponent();

            this.SyntaxPanel.ViewModel.CompilationUnitSyntax = ViewModel.AnalyseContext.CurrentRoot ;
        }

        #region Implementation of IView<ICompilationViewModel>
        public ICompilationViewModel ViewModel { get => viewModel ; set => viewModel = value ; }
        #endregion

        private void tv_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var view = ( CollectionViewSource ) TryFindResource ( "Compilation" ) ;
            view.View.MoveCurrentTo ( e.NewValue ) ;
        }
    }

    public interface ICompilationViewModel : IViewModel
    {
        CodeAnalyseContext AnalyseContext { get ; set ; }
    }

    class CompilationViewModel : ICompilationViewModel
    {
        private CodeAnalyseContext analyseContext ;
        #region Implementation of ICompilationViewModel
        public CompilationViewModel (  )
        {
            this.analyseContext = analyseContext ;
        }

        public CodeAnalyseContext AnalyseContext { get => analyseContext ; set => analyseContext = value ; }
        #endregion
    }
}
