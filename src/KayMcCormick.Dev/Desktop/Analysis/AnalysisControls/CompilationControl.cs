using System.Windows;
using System.Windows.Controls;
using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    public class CodeAnalysisProperties : DependencyObject
    {
        /// <summary>
        /// Provides access to the <see cref="Compilation"/> instance.
        /// </summary>
        public static readonly DependencyProperty CompilationProperty = DependencyProperty.RegisterAttached(
            "Compilation", typeof(Compilation), typeof(CodeAnalysisProperties), new FrameworkPropertyMetadata(default(Compilation), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty SyntaxNodeProperty = DependencyProperty.Register(
            "SyntaxNode", typeof(SyntaxNode), typeof(CodeAnalysisProperties), new PropertyMetadata(default(SyntaxNode)));

        public SyntaxNode SyntaxNode
        {
            get { return (SyntaxNode) GetValue(SyntaxNodeProperty); }
            set { SetValue(SyntaxNodeProperty, value); }
        }

        public static readonly DependencyProperty SyntaxTreeProperty = DependencyProperty.Register(
            "SyntaxTree", typeof(SyntaxTree), typeof(CodeAnalysisProperties), new PropertyMetadata(default(SyntaxTree)));

        public SyntaxTree SyntaxTree
        {
            get { return (SyntaxTree) GetValue(SyntaxTreeProperty); }
            set { SetValue(SyntaxTreeProperty, value); }
        }

        public static readonly DependencyProperty SemanticModelProperty = DependencyProperty.Register(
            "SemanticModel", typeof(SemanticModel), typeof(CodeAnalysisProperties), new PropertyMetadata(default(SemanticModel)));

        public SemanticModel SemanticModel
        {
            get { return (SemanticModel) GetValue(SemanticModelProperty); }
            set { SetValue(SemanticModelProperty, value); }
        }

    }
    
    /// <summary>
    /// 
    /// </summary>
    public abstract class CompilationControl : Control, IAppCustomControl
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty CompilationProperty = DependencyProperty.RegisterAttached(
            "Compilation", typeof(Compilation), typeof(SyntaxNodeControl), new FrameworkPropertyMetadata(default(Compilation), FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// 
        /// </summary>
        public Compilation Compilation
        {
            get { return (Compilation)GetValue(CompilationProperty); }
            set { SetValue(CompilationProperty, value); }
        }

    }
}