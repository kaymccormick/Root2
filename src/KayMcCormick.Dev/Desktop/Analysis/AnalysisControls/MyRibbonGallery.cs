using System;
using System.Windows;
using System.Windows.Controls.Ribbon;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MyRibbonGallery : RibbonGallery, IAppControl
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid ControlId { get; } = Guid.NewGuid();

        static MyRibbonGallery()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRibbonGallery),
                new FrameworkPropertyMetadata(typeof(MyRibbonGallery)));
        }
    
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return base.IsItemItsOwnContainerOverride(item);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MyRibbonGalleryCategory();
        }
    }
}