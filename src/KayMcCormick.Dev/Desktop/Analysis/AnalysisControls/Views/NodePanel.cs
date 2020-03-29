#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisControls
// NodePanel.cs
// 
// 2020-03-11-7:23 PM
// 
// ---
#endregion
using System.Windows.Controls ;
using AnalysisControls.Interfaces ;

namespace AnalysisControls.Views
{
    public partial class NodePanel : UserControl , INodePanel
    {
        public NodePanel ( ) { InitializeComponent ( ) ; }
    }
}