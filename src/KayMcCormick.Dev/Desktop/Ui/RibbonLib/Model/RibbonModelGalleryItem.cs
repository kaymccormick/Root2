namespace RibbonLib.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelGalleryItem : RibbonModelItem
    {
        /// <summary>
        /// 
        /// </summary>
        public object Content { get; set; }

        /// <inheritdoc />
        public override ControlKind Kind => ControlKind.RibbonGalleryItem;
    }
}