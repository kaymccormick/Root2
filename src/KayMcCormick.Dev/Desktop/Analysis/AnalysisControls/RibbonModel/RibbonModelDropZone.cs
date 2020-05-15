using System.Windows;
using System.Windows.Media;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelDropZone : RibbonModelItem
    {
        public override ControlKind Kind => ControlKind.DropZone;

        /// <summary>
        /// 
        /// </summary>
        public Brush Fill { get; set; }

        public virtual DragDropEffects OnDrop(IDataObject eData)
        {
            return DragDropEffects.None;
        }
    }
}