﻿using System.Linq;
using System.Windows;
using AnalysisControls.ViewModel;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    public static class RoslynProperties
    {
        public static readonly DependencyProperty DocumentProperty = DependencyProperty.RegisterAttached(
            "Document", typeof(Document), typeof(RoslynProperties),
            new PropertyMetadata(default(Document), OnDocumentChanged));

        public static readonly DependencyProperty SyntaxNodeProperty = DependencyProperty.RegisterAttached(
            "SyntaxNode", typeof(SyntaxNode), typeof(RoslynProperties),
            new PropertyMetadata(default(SyntaxNode)));

        public static readonly DependencyProperty SemanticModelProperty = DependencyProperty.RegisterAttached(
            "SemanticModel", typeof(SemanticModel), typeof(RoslynProperties),
            new PropertyMetadata(default(SemanticModel)));

        public static readonly DependencyProperty SyntaxTreeProperty = DependencyProperty.RegisterAttached(
            "SyntaxTree", typeof(SyntaxTree), typeof(RoslynProperties),
            new FrameworkPropertyMetadata(default(SyntaxTree), FrameworkPropertyMetadataOptions.None,
                SyntaxTreeUpdated));

        public static readonly DependencyProperty CompilationProperty = DependencyProperty.RegisterAttached(
            "Compilation", typeof(Compilation), typeof(RoslynProperties),
            new FrameworkPropertyMetadata(default(Compilation), FrameworkPropertyMetadataOptions.None));

        public static readonly DependencyProperty SourceTextProperty = DependencyProperty.RegisterAttached(
            "SourceText", typeof(string), typeof(RoslynProperties), new PropertyMetadata(null));

        public static Document get_Document(DependencyObject d)
        {
            return (Document) d.GetValue(DocumentProperty);
        }

        public static void set_Document(DependencyObject d, Document document)
        {
            d.SetValue(DocumentProperty, document);
        }

        public static SyntaxNode get_SyntaxNode(DependencyObject d)
        {
            return (SyntaxNode) d.GetValue(SyntaxNodeProperty);
        }

        public static void set_SyntaxNode(DependencyObject d, SyntaxNode syntaxNode)
        {
            d.SetValue(SyntaxNodeProperty, syntaxNode);
        }

        private static void SyntaxTreeUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DebugUtils.WriteLine("Syntax tree updated");
            DebugUtils.WriteLine("Resetting model and compilation to null");
            set_SemanticModel(d, null);
            set_Compilation(d, null);
            DebugUtils.WriteLine("setting node to syntax root");
            set_SyntaxNode(d, ((SyntaxTree) e.NewValue)?.GetRoot());
        }

        private static Compilation get_Compilation(DependencyObject d)
        {
            return (Compilation) d.GetValue(CompilationProperty);
        }

        private static void set_Compilation(DependencyObject d, Compilation compilation)
        {
            d.SetValue(CompilationProperty, compilation);
        }

        private static SemanticModel get_SemanticModel(DependencyObject d)
        {
            return (SemanticModel) d.GetValue(SemanticModelProperty);
        }

        private static void set_SemanticModel(DependencyObject d, SemanticModel model)
        {
            d.SetValue(SemanticModelProperty, model);
        }

        private static string get_SourceText(DependencyObject d)
        {
            return (string) d.GetValue(SourceTextProperty);
        }

        private static void set_SourceText(DependencyObject d, string sourceText)
        {
            d.SetValue(SourceTextProperty, sourceText);
        }

        private static async void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newValue = (Document) e.NewValue;
            if (newValue != null && newValue.SupportsSyntaxTree)
            {
                var t = await newValue.GetSyntaxTreeAsync().ConfigureAwait(true);
                d.SetValue(SyntaxTreeProperty, t);
            }

            if (newValue != null && newValue.SupportsSemanticModel)
            {
                var model = await newValue.GetSemanticModelAsync().ConfigureAwait(true);
                set_SemanticModel(d, model);
            }
        }
    }
}