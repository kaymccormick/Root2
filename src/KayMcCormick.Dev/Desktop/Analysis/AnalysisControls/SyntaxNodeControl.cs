using System.Windows;
using System.Windows.Controls;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class SyntaxNodeControl : Control
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsSelectingProperty = DependencyProperty.Register(
            "IsSelecting", typeof(bool), typeof(SyntaxNodeControl), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 
        /// </summary>
        public bool IsSelecting
        {
            get { return (bool) GetValue(IsSelectingProperty); }
            set { SetValue(IsSelectingProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectionEnabledProperty = DependencyProperty.Register(
            "SelectionEnabled", typeof(bool), typeof(SyntaxNodeControl), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 
        /// </summary>
        public bool SelectionEnabled
        {
            get { return (bool) GetValue(SelectionEnabledProperty); }
            set { SetValue(SelectionEnabledProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty NodeProperty = DependencyProperty.Register(
            "Node", typeof(SyntaxNode), typeof(SyntaxNodeControl), new PropertyMetadata(default(SyntaxNode)));

        /// <summary>
        /// 
        /// </summary>
        public SyntaxNode Node
        {
            get { return (SyntaxNode) GetValue(NodeProperty); }
            set { SetValue(NodeProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty CompilationProperty = DependencyProperty.Register(
            "Compilation", typeof(CSharpCompilation), typeof(SyntaxNodeControl), new PropertyMetadata(default(CSharpCompilation)));

        /// <summary>
        /// 
        /// </summary>
        public CSharpCompilation Compilation
        {
            get { return (CSharpCompilation) GetValue(CompilationProperty); }
            set { SetValue(CompilationProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SyntaxTreeProperty = DependencyProperty.Register(
            "SyntaxTree", typeof(SyntaxTree), typeof(SyntaxNodeControl), new PropertyMetadata(default(SyntaxTree), SyntaxTreeUpdated));

        private static void SyntaxTreeUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SyntaxNodeControl x = (SyntaxNodeControl) d;
            x.Node = ((SyntaxTree) e.NewValue)?.GetRoot();
        }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxTree SyntaxTree
        {
            get { return (SyntaxTree) GetValue(SyntaxTreeProperty); }
            set { SetValue(SyntaxTreeProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(
            "Model", typeof(SemanticModel), typeof(SyntaxNodeControl), new PropertyMetadata(default(SemanticModel)));

        /// <summary>
        /// 
        /// </summary>
        public SemanticModel Model
        {
            get { return (SemanticModel) GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }
    }
}