using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Shapes;
using JetBrains.Annotations;
using KayMcCormick.Dev;

namespace AnalysisControls.RibbonM
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModel
    {
        /// <summary>
        /// 
        /// </summary>
        public RibbonModelApplicationMenu AppMenu { get; set; } = new RibbonModelApplicationMenu();

        /// <summary>
        /// 
        /// </summary>
        public RibbonModel()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static object CreateGallery()
        {
            return new RibbonGallery();
        }

        private RibbonModelTab CreateTab(string header
        )
        {
            var tab = new RibbonModelTab {Header = header};
            RibbonItems.Add(tab);
            return tab;
        }


        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<RibbonModelTab> RibbonItems { get; } = new ObservableCollection<RibbonModelTab>();

        public ObservableCollection<RibbonModelContextualTabGroup> ContextualTabGroups
        {
            get;
            set;
        } = new ObservableCollection<RibbonModelContextualTabGroup>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public static object CreateGalleryCategory(string header)
        {
            return new RibbonGalleryCategory() {Header = header};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gallery"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static object CreateGalleryCategory(object gallery, string header)
        {
            var cat = RibbonModel.CreateGalleryCategory(header);
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
            var ribbonGalleryItem = new RibbonGalleryItem() { Content = content };
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

    public class RibbonModelContextualTabGroup : INotifyPropertyChanged
    {
        private Visibility _visibility = Visibility.Visible;
        public string Header { get; set; }

        public Visibility Visibility
        {
            get
            {
                DebugUtils.WriteLine("requested visiblity");
                return _visibility;
            }
            set
            {
                if (value == _visibility) return;
                DebugUtils.WriteLine($"Setting visibility to {value}");
                _visibility = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            DebugUtils.WriteLine($"{propertyName}");
        }
    }
}