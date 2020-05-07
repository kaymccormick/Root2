using System.Windows;
using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class SyntaxNodeDepth
    {
        public Thickness Margin
        {
            get { return new Thickness(Depth * 10, 0, 0, 0); }
        }

        public int Depth { get; set; }
        public SyntaxNode SyntaxNode { get; set; }
    }
}