using System.Windows.Controls;
using AnalysisFramework ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using ProjLib ;

namespace AnalysisControls
{
    /// <summary>
    /// Interaction logic for ComponentPage.xaml
    /// </summary>
    public partial class ComponentPage : Page
    {
        public IComponentViewModel ViewModel { get ; }

        public ComponentPage(IComponentViewModel viewModel)
        {
            ViewModel = viewModel ;
            InitializeComponent();
        }
    }

    public interface IComponentViewModel : IViewModel
    {
        SyntaxTree Tree { get ; }

        ICompilationUnitRootContext RootContext { get ; }
    }

    public class ComponentViewModel : IComponentViewModel
    {
        public SyntaxTree Tree { get ; }

        public ICompilationUnitRootContext RootContext { get ; }

        public ComponentViewModel ( ICompilationUnitRootContext rootContext , [ CanBeNull ] SyntaxTree syntaxTree  = null)
        {
            this.RootContext = rootContext ;
            this.Tree = syntaxTree ;
        }
    }
}
