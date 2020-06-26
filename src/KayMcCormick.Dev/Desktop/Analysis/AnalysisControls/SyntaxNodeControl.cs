using System.Linq;
using System.Windows;
using AnalysisAppLib;
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
        public static readonly DependencyProperty SourceTextProperty = DependencyProperty.Register(
            "SourceText", typeof(string), typeof(SyntaxNodeControl), new PropertyMetadata("", OnSourceTextUpdated));

        /// <summary>
        /// 
        /// </summary>
        public SemanticModel Model
        {
            get { return (SemanticModel) GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SourceText
        {
            get { return (string) GetValue(SourceTextProperty); }
            set { SetValue(SourceTextProperty, value); }
        }

        protected bool UpdatingSourceText { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ChangingText { get; set; }

        private static void OnSourceTextUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
            var f = (SyntaxNodeControl)d;

            f.OnSourceTextChanged1((string) e.NewValue, (string)e.OldValue);

            return;

            var c = (FormattedTextControl3) d;

            var eNewValue = (string) e.NewValue;
            if (e.OldValue == null || c.SyntaxTree == null)
            {
                //    var tree = CSharpSyntaxTree.ParseText(eNewValue, new CSharpParseOptions(LanguageVersion.CSharp7_3));
                //    c.SyntaxTree = tree;
            }
            else
            {
                var newTree = c.SyntaxTree.WithChangedText(Microsoft.CodeAnalysis.Text.SourceText.From(eNewValue));
                c.SyntaxTree = newTree;
#if false
                foreach (var textChange in newTree.GetChanges(c.SyntaxTree))
                {
		DebugUtils.WriteLine($"{textChange.Span}", DebugCategory.TextFormatting);
                    var i = textChange.Span.Start;
                    LineInfo theLine = null;
                    for (var line = c.LineInfos.FirstOrDefault(); line != null; line = line.NextLine)
                    {
                        
                        if (line.Offset + line.Length >= i)
                        {
                            theLine = line;
                            break;
                        }
                    }

                    if (theLine != null)
                    {
                        var region = theLine.Regions[0];
                        while (region != null)
                        {
                            if (region.Offset + region.Length >= i)
                            {
                                break;
                            }
                            region = region.NextRegion;
                        }

                        if (region != null)
                        {
                            var chi = i - region.Offset;

                        }
                    }
                    
                }
#endif
                foreach (var changedSpan in c.SyntaxTree.GetChangedSpans(newTree))
                {
                }
            }


            if (!string.IsNullOrWhiteSpace(eNewValue))
            {
                var ctx = AnalysisService.Parse(eNewValue, "x");
                c.SyntaxTree = ctx.SyntaxTree;
                c.Model = ctx.CurrentModel;
                c.Compilation = ctx.Compilation;
            }
        }

        protected virtual void OnSourceTextChanged1(string newValue, string eOldValue)
        {
            if (ChangingText)
                return;
            if (newValue  != null)
            {
                UpdatingSourceText = true;
                Compilation = CSharpCompilation.Create(
                    "test",
                    new[]
                    {
                        SyntaxFactory.ParseSyntaxTree(
                            newValue)
                    }, new[] {MetadataReference.CreateFromFile(typeof(object).Assembly.Location)});
                SyntaxTree = Compilation.SyntaxTrees.First();
                Node = SyntaxTree.GetRoot();
                Model = Compilation?.GetSemanticModel(SyntaxTree);
                UpdatingSourceText = false;
                
                
            }

            
        }
    }
}