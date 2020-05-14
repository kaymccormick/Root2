using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using NLog;

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

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return base.IsItemItsOwnContainerOverride(item);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MyRibbonGalleryCategory();
        }
    }

    public class MyRibbonGalleryCategory : RibbonGalleryCategory, IAppControl
    {
        private Logger Logger { get; } 
    
        /// <summary>
        /// 
        /// </summary>
        public MyRibbonGalleryCategory()
        {
            // Logger = LogManager.GetLogger(
                // $"{nameof(MyRibbonGalleryCategory)}.{ControlId.ToString().ToUpperInvariant()}");
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

    public class MyRibbonGalleryItem : RibbonGalleryItem
    {
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
        }

        protected override void OnContentTemplateChanged(DataTemplate oldContentTemplate, DataTemplate newContentTemplate)
        {
            base.OnContentTemplateChanged(oldContentTemplate, newContentTemplate);
        }

        protected override void OnContentTemplateSelectorChanged(DataTemplateSelector oldContentTemplateSelector,
            DataTemplateSelector newContentTemplateSelector)
        {
            base.OnContentTemplateSelectorChanged(oldContentTemplateSelector, newContentTemplateSelector);
        }

        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            base.OnTemplateChanged(oldTemplate, newTemplate);
        }
    }
}