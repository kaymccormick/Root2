using System.Collections.Generic;
using System.Windows;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public static class CodeProps
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SourceTextProperty = DependencyProperty.RegisterAttached(
            "SourceText", typeof(SourceText), typeof(CodeProps), new PropertyMetadata(default(SourceText), SourceTextUpdated, CoerceSourceText));

        private static object CoerceSourceText(DependencyObject d, object basevalue)
        {
            if (basevalue is SourceText)
            {
                return basevalue;
            }

            return SourceText.From(basevalue.ToString());
        }

        private static void SourceTextUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="sourceText"></param>
        public static void SetSourceText(DependencyObject d, SourceText sourceText)
        {
                d.SetValue(SourceTextProperty, sourceText);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static SourceText GetSourceText(DependencyObject d)
        {
            
                return (SourceText) d.GetValue(SourceTextProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SyntaxTreesProperty = DependencyProperty.RegisterAttached(
            "SyntaxTrees", typeof(IEnumerable<SyntaxTree>), typeof(CompilationExtension), new PropertyMetadata(default(IEnumerable<SyntaxTree>)));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        [UsedImplicitly]
        public static IEnumerable<SyntaxTree> GetSyntaxTrees(DependencyObject d)
        {
            return (IEnumerable<SyntaxTree>) d.GetValue(SyntaxTreesProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="syntaxTrees"></param>
        [UsedImplicitly]
        public static void SetSyntaxTrees(DependencyObject d, IEnumerable<SyntaxTree> syntaxTrees)
        {
            d.SetValue(SyntaxTreesProperty, syntaxTrees);
        }
    }
}
