﻿using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AnalysisControls.RibbonModel
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