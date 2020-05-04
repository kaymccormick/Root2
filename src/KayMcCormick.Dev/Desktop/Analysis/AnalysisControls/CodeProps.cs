using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace AnalysisControls
{
    public static class CodeProps
    {
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

        public static void SetSourceText(DependencyObject d, SourceText sourceText)
        {
                d.SetValue(SourceTextProperty, sourceText);

        }

        public static SourceText GetSourceText(DependencyObject d)
        {
            
                return (SourceText) d.GetValue(SourceTextProperty);
        }

        public static readonly DependencyProperty SyntaxTreesProperty = DependencyProperty.RegisterAttached(
            "SyntaxTrees", typeof(IEnumerable<SyntaxTree>), typeof(CompilationExtension), new PropertyMetadata(default(IEnumerable<SyntaxTree>)));

        public static IEnumerable<SyntaxTree> GetSyntaxTrees(DependencyObject d)
        {
            return (IEnumerable<SyntaxTree>) d.GetValue(SyntaxTreesProperty);
        }

        public static void SetSyntaxTrees(DependencyObject d, IEnumerable<SyntaxTree> syntaxTrees)
        {
            d.SetValue(SyntaxTreesProperty, syntaxTrees);
        }
    }
}
