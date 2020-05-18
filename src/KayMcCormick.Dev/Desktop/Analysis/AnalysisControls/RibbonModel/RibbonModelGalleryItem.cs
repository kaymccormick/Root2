namespace AnalysisControls.RibbonModel
{
    public class RibbonModelGalleryItem : RibbonModelItem
    {
        public object Content { get; set; }

        public override ControlKind Kind => ControlKind.RibbonGalleryItem;
    }
}