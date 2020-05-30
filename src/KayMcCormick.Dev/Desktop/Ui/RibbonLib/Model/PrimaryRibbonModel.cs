using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using JetBrains.Annotations;

namespace RibbonLib.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class PrimaryRibbonModel : INotifyPropertyChanged
    {
        private Brush _background;
        private object _selectedItem;
        private Brush _borderBrush;
        private object _activeContent;

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


        public Brush BorderBrush
        {
            get { return _borderBrush; }
            set
            {
                if (Equals(value, _borderBrush)) return;
                _borderBrush = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object SelectedItem
        
        {
            get { return _selectedItem; }
            set
            {
                if (Equals(value, _selectedItem)) return;
                _selectedItem = value;
                if (_selectedItem is RibbonModelTab tab)
                {
                    tab.OnActiveContentChanged(this, new ActiveContentChangedEventArgs(ActiveContent));
                }
                OnPropertyChanged();
            }
        }

        public object ActiveContent
        {
            get { return _activeContent; }
            set
            {
                if (Equals(value, _activeContent)) return;
                _activeContent = value;
                (SelectedItem as RibbonModelTab)?.OnActiveContentChanged(this, new ActiveContentChangedEventArgs(value));
                OnPropertyChanged();
            }
        }

        public Brush Background
        {
            get { return _background; }
            set
            {
                if (Equals(value, _background)) return;
                _background = value;
                OnPropertyChanged();
            }
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

	/// <summary>
	/// 
	/// </summary>
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            RibbonDebugUtils.OnPropertyChanged(this, propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
