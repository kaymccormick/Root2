using System.Windows;
using System.Windows.Controls;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class SemanticControl1 : SyntaxNodeControl
    {
        private TreeView _treeView;

        static SemanticControl1()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SemanticControl1),
                new FrameworkPropertyMetadata(typeof(SemanticControl1)));
        }

        public SemanticControl1()
        {
        }

        public override void OnApplyTemplate()
        {
            _treeView = (TreeView) GetTemplateChild("TreeView");
            if (_treeView != null)
                if (Compilation != null)
                    _treeView.ItemsSource = Compilation.GlobalNamespace.GetMembers();
                else
                    DebugUtils.WriteLine("Compilation is unavailable.");
        }
    }
}