using System.Windows;

namespace ProjTests
{
    internal struct CharRow
    {
        public char Char { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public Rect Bounds { get; set; }
        public string RegionKey { get; set; }
        public string SyntaxNodeKind { get; set; }
        public string SyntaxNodeType { get; set; }
        public string SyntaxTokenKind { get; set; }
        public string SyntaxTokenText { get; set; }
        public string SyntaxTokenValueText { get; set; }
        public object SyntaxTokenValue { get; set; }
        public int SyntaxTokenRawKind { get; set; }

        public int RegionOffset { get; set; }
        public int LineOffset { get; set; }
        public int LineNumber { get; set; }
        public int RegionLength { get; set; }
        public string LineText { get; set; }
        public int LineLength { get; set; }
        public double LineOriginX { get; set; }
        public double LineOriginY { get; set; }
        public double LineHeight { get; set; }
        public double LineWidth { get; set; }
    }
}