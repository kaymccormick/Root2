using System.Windows;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SyntaxNodeControl : CompilationControl
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
            "SelectionEnabled", typeof(bool), typeof(SyntaxNodeControl), new PropertyMetadata(true));

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
            "Node", typeof(SyntaxNode), typeof(SyntaxNodeControl), new PropertyMetadata(default(SyntaxNode), OnNodeUpdated));

        private static void OnNodeUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SyntaxNodeControl ss = (SyntaxNodeControl)d;
            ss.OnNodeUpdated();
        }

        protected virtual void OnNodeUpdated()
        {

        }

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
        public static readonly DependencyProperty SyntaxTreeProperty = DependencyProperty.Register(
            "SyntaxTree", typeof(SyntaxTree), typeof(SyntaxNodeControl), new FrameworkPropertyMetadata(default(SyntaxTree),FrameworkPropertyMetadataOptions.Inherits, SyntaxTreeUpdated, CoerceValueCallback));

        private static object CoerceValueCallback(DependencyObject d, object basevalue)
        {
            return basevalue;
        }

        private static void SyntaxTreeUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SyntaxNodeControl ss = (SyntaxNodeControl)d;
            ss.OnSyntaxTreeUpdated((SyntaxTree)e.NewValue);
        }

        protected virtual void OnSyntaxTreeUpdated(SyntaxTree newValue)
        {
	    DebugUtils.WriteLine("Syntax tree updated");
	DebugUtils.WriteLine("Resetting model and compilation to null");
            Model = null;
            Compilation = null;
	DebugUtils.WriteLine("setting node to syntax root");
            Node = newValue?.GetRoot();
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