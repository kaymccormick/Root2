﻿using System.Collections.ObjectModel;
using System.Windows.Shapes;

namespace AnalysisControls.RibbonM
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelGalleryCategory : RibbonModelItem
    {
        /// <summary>
        /// 
        /// </summary>
        public RibbonModelGalleryCategory()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<object> Items { get; } =
            new ObservableCollection<object>();

        /// <summary>
        /// 
        /// </summary>
        public object Content { get; set; }
        public string Header { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public RibbonModelGalleryItem CreateGalleryItem(object content)
        {
            var r = new RibbonModelGalleryItem(){Content=content};
            Items.Add(r);
            return r;
        }
    }
}