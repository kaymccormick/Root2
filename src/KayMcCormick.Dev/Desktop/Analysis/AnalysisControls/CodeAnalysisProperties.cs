using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class CodeAnalysisProperties
    {
        /// <summary>
        /// Provides access to the <see cref="Compilation"/> instance.
        /// </summary>
        public static readonly DependencyProperty CompilationProperty = DependencyProperty.RegisterAttached(
            // ReSharper disable once RedundantDelegateCreation
            "Compilation", typeof(Compilation), typeof(CodeAnalysisProperties), new PropertyMetadata(null, new PropertyChangedCallback(OnCompilationChanged)));
        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent CompilationChangedEvent =
            EventManager.RegisterRoutedEvent(
                "CompilationChanged"
                , RoutingStrategy.Direct
                , typeof(RoutedPropertyChangedEventHandler<Compilation>)
                , typeof(CodeAnalysisProperties)
            );

        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent SyntaxTreeChangedEvent =
            EventManager.RegisterRoutedEvent(
                "SyntaxTreeChanged"
                , RoutingStrategy.Direct
                , typeof(RoutedPropertyChangedEventHandler<SyntaxTree>)
                , typeof(CodeAnalysisProperties)
            );

        private static void OnCompilationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
                var evt = CompilationChangedEvent;
                var ev = new RoutedPropertyChangedEventArgs<Compilation>(
                    (Compilation)e
                        .OldValue
                    , (Compilation)e
                        .NewValue
                    , evt
                );
                switch (d)
                {
                    case UIElement uie:
                        uie.RaiseEvent(ev);
                        break;
                    case ContentElement ce:
                        ce.RaiseEvent(ev);
                        break;
                }
            }

        private static void OnSyntaxTreeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var evt = SyntaxTreeChangedEvent;
            var ev = new RoutedPropertyChangedEventArgs<SyntaxTree>(
                (SyntaxTree)e
                    .OldValue
                , (SyntaxTree)e
                    .NewValue
                , evt
            );
            switch (d)
            {
                case UIElement uie:
                    uie.RaiseEvent(ev);
                    break;
                case ContentElement ce:
                    ce.RaiseEvent(ev);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SyntaxNodeProperty = DependencyProperty.RegisterAttached(
            "SyntaxNode", typeof(SyntaxNode), typeof(CodeAnalysisProperties), new PropertyMetadata(default(SyntaxNode)));


        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static SyntaxNode GetSyntaxNode(DependencyObject o)
        {
            return (SyntaxNode) o.GetValue(SyntaxNodeProperty);
        }

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="node"></param>
        public static void SetSyntaxNode(DependencyObject o, SyntaxNode node)
        {
            o.SetValue(SyntaxNodeProperty, node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static Compilation GetCompilation(DependencyObject o)
        {
            return (Compilation) o.GetValue(CompilationProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="c"></param>
        public static void SetCompilation(DependencyObject o, Compilation c)
        {
            o.SetValue(CompilationProperty, c);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static SyntaxTree GetSyntaxTree(DependencyObject o)
        {
            return (SyntaxTree) o.GetValue(SyntaxTreeProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="syntaxTree"></param>
        public static void SetSyntaxTree(DependencyObject o, SyntaxTree syntaxTree)
        {
            o.SetValue(SyntaxTreeProperty, syntaxTree);
        }


        // public SyntaxNode SyntaxNode
        // {
            // get { return (SyntaxNode) GetValue(SyntaxNodeProperty); }
            // set { SetValue(SyntaxNodeProperty, value); }
        // }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SyntaxTreeProperty = DependencyProperty.RegisterAttached(
            // ReSharper disable once RedundantDelegateCreation
            "SyntaxTree", typeof(SyntaxTree), typeof(CodeAnalysisProperties), new PropertyMetadata(default(SyntaxTree), new PropertyChangedCallback(OnSyntaxTreeChanged)));

     
        // public SyntaxTree SyntaxTree
        // {
            // get { return (SyntaxTree) GetValue(SyntaxTreeProperty); }
            // set { SetValue(SyntaxTreeProperty, value); }
        // }

    
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public static readonly DependencyProperty SemanticModelProperty = DependencyProperty.RegisterAttached(
            "SemanticModel", typeof(SemanticModel), typeof(CodeAnalysisProperties), new PropertyMetadata(default(SemanticModel)));

        public FrameworkElement Content { get; set; }
  
        // public SemanticModel SemanticModel
        // {
            // get { return (SemanticModel) GetValue(SemanticModelProperty); }
            // set { SetValue(SemanticModelProperty, value); }
        // }
    }
}