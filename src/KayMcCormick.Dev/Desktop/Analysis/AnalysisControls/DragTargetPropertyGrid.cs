using System.Windows.Forms;

namespace AnalysisControls
{
    public class DragTargetPropertyGrid : PropertyGrid
    {
        public override bool AllowDrop => true;
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
            SelectedObject = drgevent.Data.GetData(drgevent.Data.GetFormats()[0]);
        }

    }
}