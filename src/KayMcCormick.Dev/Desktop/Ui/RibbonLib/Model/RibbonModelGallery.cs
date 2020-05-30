using System.Collections.ObjectModel;

namespace RibbonLib.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelGallery : RibbonModelItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string Header { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<object> Items { get; } = new ObservableCollection<object>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public RibbonModelGalleryCategory CreateGalleryCategory(string header)
        {
            var cat = new RibbonModelGalleryCategory(){Header=header};
            Items.Add(cat);
            return cat;
        }

        public override ControlKind Kind => ControlKind.RibbonGallery;

    }
}