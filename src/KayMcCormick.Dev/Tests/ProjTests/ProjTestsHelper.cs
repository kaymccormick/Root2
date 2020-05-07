using System.Windows;
using System.Windows.Media;
using AnalysisAppLib;
using AnalysisControls;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

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
                chrow.SyntaxNodeKind = regionInfo.SyntaxNode.Kind().ToString();
                chrow.SyntaxNodeType = regionInfo.SyntaxNode.GetType().Name;
            } else if (regionInfo.SyntaxToken.HasValue)
            {
                var token = regionInfo.SyntaxToken.Value;
                chrow.SyntaxTokenKind = token.Kind().ToString();
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
            control.BorderThickness = new Thickness(3);
            control.BorderBrush = Brushes.Pink;
            control.VerticalAlignment = VerticalAlignment.Stretch;
            control.HorizontalAlignment = HorizontalAlignment.Stretch;
            var tree = ProjTestsHelper.SetupSyntaxParams(out var compilation);

            control.SyntaxTree = tree;
            control.Compilation = compilation;
            var w = new Window {Content = control, ShowActivated = true};
            w.ShowDialog();
        }

        public static SyntaxTree SetupSyntaxParams(out CSharpCompilation compilation)
        {
            var unitSyntax = SyntaxFactory.ParseCompilationUnit(AnalysisAppLib.Properties.Resources.Program_Parse)
                .NormalizeWhitespace("    ");
            var tree = SyntaxFactory.SyntaxTree(unitSyntax);

            compilation = AnalysisService.CreateCompilation("x", tree);
            return tree;
        }
    }
}