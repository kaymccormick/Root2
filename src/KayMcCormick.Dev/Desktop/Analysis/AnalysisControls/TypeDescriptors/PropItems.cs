using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace AnalysisControls.TypeDescriptors
{
    public class PropItems : ItemsControl
    {
        
        /// <inheritdoc />
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
        }

        /// <inheritdoc />
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
        }

        /// <inheritdoc />
        protected override void OnItemsPanelChanged(ItemsPanelTemplate oldItemsPanel, ItemsPanelTemplate newItemsPanel)
        {
            base.OnItemsPanelChanged(oldItemsPanel, newItemsPanel);
        }

        /// <inheritdoc />
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            if (item is PropItem)
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new PropItem();
        }

        /// <inheritdoc />
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }

        /// <inheritdoc />
        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
        }

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        static PropItems()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridCellsPresenter), (PropertyMetadata)new FrameworkPropertyMetadata((object)typeof(PropItems)));
            ItemsControl.ItemsPanelProperty.OverrideMetadata(typeof(DataGridCellsPresenter), (PropertyMetadata)new FrameworkPropertyMetadata((object)new ItemsPanelTemplate(new FrameworkElementFactory(typeof(PropItemsPanel)))));
            UIElement.FocusableProperty.OverrideMetadata(typeof(DataGridCellsPresenter), (PropertyMetadata)new FrameworkPropertyMetadata((object)false));

        }
    }
}