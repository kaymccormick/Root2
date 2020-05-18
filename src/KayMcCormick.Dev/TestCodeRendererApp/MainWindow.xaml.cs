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
using AnalysisAppLib;
using AnalysisControls;
using Microsoft.CodeAnalysis;

namespace TestCodeRendererApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var codeRenderer = new CodeRenderer();
            var codeAnalyseContext = AnalysisService.Parse(AnalysisControls.Properties.Resources.Program_Parse, "x");
            codeRenderer.BeginInit();
            codeRenderer.EndInit();
            CodeAnalysisProperties.SetCompilation(codeRenderer, (Compilation)codeAnalyseContext.Compilation);
            CodeAnalysisProperties.SetSyntaxTree((DependencyObject)codeRenderer, codeAnalyseContext.SyntaxTree);
            // codeRenderer.SyntaxTree = codeAnalyseContext.SyntaxTree;
            // codeRenderer.Compilation = (Compilation)codeAnalyseContext.Compilation;
            Grid.Children.Add(codeRenderer);
        }
    }
}
