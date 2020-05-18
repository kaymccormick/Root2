using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace AnalysisControls
{
    [MarkupExtensionReturnType(typeof(SyntaxTree))]
    public class SyntaxTreeExtension : MarkupExtension
    {
        public IEnumerable<SyntaxTree> SyntaxTrees { get; set; }

        public int Index { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            try
            {
                if (SyntaxTrees != null) return SyntaxTrees.ElementAt(Index);

                var rootP = (IRootObjectProvider) serviceProvider.GetService(typeof(IRootObjectProvider));
                var w = (Window) rootP.RootObject;
                var st = w.GetValue(CodeProps.SourceTextProperty);
                if (st is SourceText x) return SyntaxFactory.ParseSyntaxTree(x, new CSharpParseOptions());

                return null;
            }
            catch (Exception ex)
            {

            }

            return null;
        } 
    }

    public class CompilationExtension : MarkupExtension
    {
        // public static readonly DependencyProperty SyntaxTreesProperty =
            // DependencyProperty.RegisterAttached("SyntaxTrees", typeof(IEnumerable<SyntaxTree>),
                // typeof(CompilationExtension));

        private IEnumerable<SyntaxTree> _syntaxTrees;

        // public static IEnumerable<SyntaxTree> GetSyntaxTrees(DependencyObject d)
        // {
            // return _syntaxTrees;
        // }

        // public static void SetSyntaxTrees(DependencyObject d, IEnumerable<SyntaxTree> trees)
        // {
            // _syntaxTrees = trees;
        // }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget t = (IProvideValueTarget) serviceProvider?.GetService(typeof(IProvideValueTarget));
            IEnumerable<SyntaxTree> trees = _syntaxTrees;
            // if (t != null)
            // {
                // var d = t.TargetObject as DependencyObject;
                // trees=(IEnumerable<SyntaxTree>) d.GetValue(CodeProps.SyntaxTreesProperty);
            // }
            if (!trees.Any())
            {
                throw new AppInvalidOperationException("No syntax trees");
            }

            IEnumerable<MetadataReference> mrs =
                new[] {typeof(object)}.Select(x => MetadataReference.CreateFromFile(x.Assembly.Location));
            var cSharpCompilation = CSharpCompilation.Create("x", trees, mrs, new CSharpCompilationOptions(OutputKind));
            DebugUtils.WriteLine(string.Join("\n",
                cSharpCompilation.GetDiagnostics().Where(xx => xx.Severity >= DiagnosticSeverity.Info)));

            return cSharpCompilation;
        }

        public OutputKind OutputKind { get; set; }

        public IEnumerable<SyntaxTree> SyntaxTrees
        {
            get { return _syntaxTrees; }
            set { _syntaxTrees = value; }
        }
    }


    public class SemanticModelExtension : MarkupExtension
    {
        #region Overrides of MarkupExtension

        [NotNull]
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var t = (IProvideValueTarget) serviceProvider.GetService(typeof(IProvideValueTarget));
            var o = t.TargetObject as BindingBase;
            if (o != null)
            {
                var value = o.ProvideValue(serviceProvider);
            }

            return "";
        }

        #endregion
    }
}