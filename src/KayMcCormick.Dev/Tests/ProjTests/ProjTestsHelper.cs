using System.Linq;
using System.Resources;
using System.Windows;
using System.Windows.Baml2006;
using System.Windows.Media;
using System.Xaml;
using AnalysisAppLib;
using AnalysisAppLib.Properties;
using AnalysisControls;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.VisualBasic;
using Remotion.FunctionalProgramming;
using CSharpExtensions = Microsoft.CodeAnalysis.CSharp.CSharpExtensions;
using SyntaxFactory = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using XamlReader = System.Windows.Markup.XamlReader;

namespace ProjTests
{
    static internal class ProjTestsHelper
    {
        public static CharRow Merge(CharacterCell ch, RegionInfo regionInfo, LineInfo li)
        {
            var chrow = new CharRow();
            chrow.Char = ch.Char;
            chrow.Row = ch.Row;
            chrow.Column = ch.Column;
            chrow.Bounds = ch.Bounds;
            chrow.RegionKey = regionInfo.Key;
            chrow.RegionOffset = regionInfo.Offset;
            if (regionInfo.SyntaxNode != null)
            {
                chrow.SyntaxNodeKind = CSharpExtensions.Kind(regionInfo.SyntaxNode).ToString();
                chrow.SyntaxNodeType = regionInfo.SyntaxNode.GetType().Name;
            } else if (regionInfo.SyntaxToken.HasValue)
            {
                var token = regionInfo.SyntaxToken.Value;
                chrow.SyntaxTokenKind = CSharpExtensions.Kind(token).ToString();
                chrow.SyntaxTokenText = token.Text;
                chrow.SyntaxTokenValueText = token.ValueText;
                chrow.SyntaxTokenValue = token.Value;
                chrow.SyntaxTokenRawKind = token.RawKind;
                

            }

            chrow.RegionLength = regionInfo.Length;
            chrow.LineNumber = li.LineNumber;
            chrow.LineOffset = li.Offset;
            chrow.LineText = li.Text;
            chrow.LineLength = li.Length;
            chrow.LineOriginX = li.Origin.X;
            chrow.LineOriginY = li.Origin.Y;
            chrow.LineWidth = li.Size.Width;
            chrow.LineHeight = li.Size.Height;
            return chrow;
        }

        public static void TestSyntaxControl(SyntaxNodeControl control)
        {
            var resources = ProjTestsHelper.ControlsResources();
            control.BorderThickness = new Thickness(3);
            control.BorderBrush = Brushes.Pink;
            control.VerticalAlignment = VerticalAlignment.Stretch;
            control.HorizontalAlignment = HorizontalAlignment.Stretch;
            var tree = ProjTestsHelper.SetupSyntaxParams(out var compilation);

            control.SyntaxTree = tree;
            control.Compilation = compilation;
            control.Model = compilation.GetSemanticModel(tree);
            var r  = new MyResourceDictionary();
            r.MergedDictionaries.Add(resources);
            var w = new Window {Content = control, ShowActivated = true, Resources = r};
            w.ShowDialog();
        }

        public static SyntaxTree SetupSyntaxParams(out CSharpCompilation compilation, string code = null)
        {
            if (code == null)
            {
                code = Resources.Program_Parse;
            }
            var unitSyntax = SyntaxNodeExtensions.NormalizeWhitespace<CompilationUnitSyntax>(SyntaxFactory.ParseCompilationUnit(code), "    ");
            var tree = SyntaxFactory.SyntaxTree(unitSyntax);

            compilation = AnalysisService.CreateCompilation("x", tree);
            return tree;
        }

        public static SyntaxTree SetupSyntaxParamsVb(out Compilation compilation, string code = null)
        {
            if (code == null)
            {
                code = "Console.Write(\"Press any key to continue...\")\r\nConsole.ReadKey(true)\r\n";
            }

            compilation = VisualBasicCompilation.Create("x", new[] {VisualBasicSyntaxTree.ParseText(code)});
            return compilation.SyntaxTrees.First();
        }

        public static ResourceDictionary ControlsResources()
        {
            var assembly = typeof(AnalysisControlsModule).Assembly;
            var x = new ResourceManager(
                "AnalysisControls.g"
                , assembly
            );

            var y = x.GetStream("templates.baml");
            // ReSharper disable once AssignNullToNotNullAttribute
            var b = new Baml2006Reader(y, new XamlReaderSettings());

            var oo = (ResourceDictionary) XamlReader.Load(b);
            return oo;
        }

        public static void TestSyntaxControlVb(FormattedTextControl control)
        {
            var tree = SetupSyntaxParamsVb(out var Compilation);

            var resources = ProjTestsHelper.ControlsResources();
            control.BorderThickness = new Thickness(3);
            control.BorderBrush = Brushes.Pink;
            control.VerticalAlignment = VerticalAlignment.Stretch;
            control.HorizontalAlignment = HorizontalAlignment.Stretch;

            control.SyntaxTree = tree;
            control.Compilation = Compilation;
            control.Model = Compilation.GetSemanticModel(tree);
            var r = new MyResourceDictionary();
            r.MergedDictionaries.Add(resources);
            var w = new Window { Content = control, ShowActivated = true, Resources = r };
            w.ShowDialog();
        }
    }

    internal class MyResourceDictionary: ResourceDictionary
    {
        public MyResourceDictionary()
        {
        }

        protected override void OnGettingValue(object key, ref object value, out bool canCache)
        {
            base.OnGettingValue(key, ref value, out canCache);
            DebugUtils.WriteLine($"{key}- {value}");
        }
    }
}