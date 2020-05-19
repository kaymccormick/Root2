using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls.Ribbon;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    public class PrimaryRibbonModel
    {
        /// <summary>
        /// 
        /// </summary>
        public RibbonModelApplicationMenu AppMenu { get; set; } = new RibbonModelApplicationMenu();

        /// <summary>
        /// 
        /// </summary>
        public PrimaryRibbonModel()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static object CreateGallery()
        {
            //return new MyRibbonGallery();
            return new RibbonModelGallery();
        }


        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ObservableCollection<RibbonModelTab> RibbonItems { get; } = new ObservableCollection<RibbonModelTab>();

        /// <summary>
        /// 
        /// </summary>
        public RibbonModelQuickAccessToolBar QuickAccessToolBar { get; set; } = new RibbonModelQuickAccessToolBar();

        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ObservableCollection<RibbonModelContextualTabGroup> ContextualTabGroups
        {
            get;
        } = new ObservableCollection<RibbonModelContextualTabGroup>();

	public object HelpPaneContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public static object CreateGalleryCategory(string header)
        {
            return new RibbonModelGalleryCategory() { Header = header};
            //return new MyRibbonGalleryCategory() {Header = header};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gallery"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static object CreateGalleryCategory(object gallery, string header)
        {
            var cat = PrimaryRibbonModel.CreateGalleryCategory(header);
            if (gallery is RibbonGallery g1)
            {
                g1.Items.Add(cat);
            } else if (gallery is RibbonModelGallery g2)
            {
                g2.Items.Add(cat);
            }

            return cat;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cat1"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static object CreateGalleryItem(object cat1, object content)
        {
            //var ribbonGalleryItem = new MyRibbonGalleryItem() { Content = content };
            var ribbonGalleryItem = new RibbonModelGalleryItem() {Content = content};
        
            if (cat1 is RibbonModelGalleryCategory c)
            {
                c.Items.Add(ribbonGalleryItem);
            }
            else if (cat1 is RibbonGalleryCategory cat)
            {
                cat.Items.Add(ribbonGalleryItem);
            }

            return ribbonGalleryItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static RibbonModelGallery CreateModelGallery()
        {
            return new RibbonModelGallery();
        }
    }
}
