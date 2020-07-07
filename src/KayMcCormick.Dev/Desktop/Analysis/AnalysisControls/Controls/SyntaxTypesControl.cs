using System.Windows;
using System.Windows.Controls;
using AnalysisControls.ViewModel;

namespace AnalysisControls.Controls
{
    public class SyntaxTypesControl : Control
    {
        static SyntaxTypesControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SyntaxTypesControl), new FrameworkPropertyMetadata(typeof(SyntaxTypesControl)));
        }
        public TypesViewModel ViewModel { get; set; }
    }
}
