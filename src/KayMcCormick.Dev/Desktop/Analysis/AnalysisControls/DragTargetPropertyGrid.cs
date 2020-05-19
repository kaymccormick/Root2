using System.Windows.Forms;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class DragTargetPropertyGrid : PropertyGrid
    {
        /// <inheritdoc />
        public override bool AllowDrop => true;

        /// <inheritdoc />
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
            SelectedObject = drgevent.Data.GetData(drgevent.Data.GetFormats()[0]);
        }

    }
}