using System;
using System.Windows;
using System.Windows.Controls.Ribbon;
using NLog;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MyRibbonGalleryCategory : RibbonGalleryCategory, IAppControl
    {
        private Logger Logger { get; }

        static MyRibbonGalleryCategory()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRibbonGalleryCategory),
                new FrameworkPropertyMetadata(typeof(MyRibbonGalleryCategory)));
        }

        /// <inheritdoc />
        protected override void OnItemTemplateChanged(DataTemplate oldItemTemplate, DataTemplate newItemTemplate)
        {
            Logger?.Info($"{nameof(OnItemTemplateChanged)} {newItemTemplate.DataTemplateKey} {newItemTemplate.DataType}");
            base.OnItemTemplateChanged(oldItemTemplate, newItemTemplate);
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return base.IsItemItsOwnContainerOverride(item);
        }

        /// <inheritdoc />
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MyRibbonGalleryItem();
        }

        /// <inheritdoc />
        public Guid ControlId { get; } = Guid.NewGuid();
    }
}