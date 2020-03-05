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
        private readonly IComponentViewModel _viewModel ;

        public ComponentPage(IComponentViewModel viewModel)
        {
            _viewModel = viewModel ;
            InitializeComponent();
        }
    }

    public interface IComponentViewModel : IViewModel
    {

    }

    public class ComponentViewModel : IComponentViewModel
    {
        private SyntaxTree syntaxTree ;

        public ComponentViewModel ( [ CanBeNull ] SyntaxTree syntaxTree  = null) { this.syntaxTree = syntaxTree ; }
    }
}
