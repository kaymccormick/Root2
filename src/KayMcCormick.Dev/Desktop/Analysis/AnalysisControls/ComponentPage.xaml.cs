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
using System.Windows.Navigation;
using System.Windows.Shapes;
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
