#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// NodePanel.cs
// 
// 2020-02-27-1:20 AM
// 
// ---
#endregion
using System.Windows.Controls ;

namespace AnalysisControls
{
    public partial class NodePanel : UserControl, INodePanel
    {
        public NodePanel ( ) {
            InitializeComponent();
        }
    }

    public interface INodePanel
    {
    }
}