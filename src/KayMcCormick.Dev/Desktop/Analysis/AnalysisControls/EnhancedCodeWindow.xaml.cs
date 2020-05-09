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
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AnalysisControls
{
    /// <summary>
    /// Interaction logic for EnhancedCodeWindow.xaml
    /// </summary>
    public partial class EnhancedCodeWindow : Window
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty CompilationProperty = DependencyProperty.Register(
            "Compilation", typeof(CSharpCompilation), typeof(EnhancedCodeWindow), new FrameworkPropertyMetadata(default(CSharpCompilation)));

        /// <summary>
        /// 
        /// </summary>
        public CSharpCompilation Compilation
        {
            get { return (CSharpCompilation)GetValue(CompilationProperty); }
            set { SetValue(CompilationProperty, value); }
        }

        /// </summary>
        public static readonly DependencyProperty IsSelectingProperty = DependencyProperty.Register(
            "IsSelecting", typeof(bool), typeof(EnhancedCodeWindow), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 
        /// </summary>
        public bool IsSelecting
        {
            get { return (bool)GetValue(IsSelectingProperty); }
            set { SetValue(IsSelectingProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectionEnabledProperty = DependencyProperty.Register(
            "SelectionEnabled", typeof(bool), typeof(EnhancedCodeWindow), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 
        /// </summary>
        public bool SelectionEnabled
        {
            get { return (bool)GetValue(SelectionEnabledProperty); }
            set { SetValue(SelectionEnabledProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty NodeProperty = DependencyProperty.Register(
            "Node", typeof(SyntaxNode), typeof(EnhancedCodeWindow), new PropertyMetadata(default(SyntaxNode)));

        /// <summary>
        /// 
        /// </summary>
        public SyntaxNode Node
        {
            get { return (SyntaxNode)GetValue(NodeProperty); }
            set { SetValue(NodeProperty, value); }
        }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SyntaxTreeProperty = DependencyProperty.Register(
            "SyntaxTree", typeof(SyntaxTree), typeof(EnhancedCodeWindow), new PropertyMetadata(default(SyntaxTree), SyntaxTreeUpdated));

        private static void SyntaxTreeUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EnhancedCodeWindow x = (EnhancedCodeWindow)d;
            x.Node = ((SyntaxTree)e.NewValue)?.GetRoot();
        }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxTree SyntaxTree
        {
            get { return (SyntaxTree)GetValue(SyntaxTreeProperty); }
            set { SetValue(SyntaxTreeProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(
            "Model", typeof(SemanticModel), typeof(EnhancedCodeWindow), new PropertyMetadata(default(SemanticModel)));

        /// <summary>
        /// 
        /// </summary>
        public SemanticModel Model
        {
            get { return (SemanticModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }
        private EnhancedCodeControl _codeControl;

        public EnhancedCodeWindow()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _codeControl = (EnhancedCodeControl) _codeControl;
            _codeControl?.SetBinding(CompilationControl.CompilationProperty, new Binding("Compilation") { Source = this });
            _codeControl?.SetBinding(SyntaxNodeControl.SyntaxTreeProperty, new Binding("SyntaxTree") { Source = this });
            _codeControl?.SetBinding(SyntaxNodeControl.ModelProperty, new Binding("Model") { Source = this });

        }
    }
}
