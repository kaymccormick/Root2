using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls.Ribbon;
using NLog;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MyRibbonComboBox : RibbonComboBox, IAppControl
    {
        private object _currentItem;
        /// <summary>
        /// 
        /// </summary>
        protected Logger Logger { get; }

        /// <summary>
        /// 
        /// </summary>
        public MyRibbonComboBox()
        {
            ControlId = Guid.NewGuid();
            Logger = LogManager.GetLogger($"{nameof(MyRibbonComboBox)}.{ControlId}");
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
        }

        /// <inheritdoc />
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            int num;
            switch (item)
            {
                case RibbonMenuItem _:
                case RibbonSeparator _:
                    num = 1;
                    break;
                default:
                    num = item is RibbonGallery ? 1 : 0;
                    break;
            }
            bool flag = num != 0;
            if (!flag)
                this._currentItem = item;
            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            var currentItem = _currentItem;
            _currentItem = (object) null;
            Logger.Info($"Current item is {currentItem}");
            Logger.Info($"UsesItemContainerTEmplate is {UsesItemContainerTemplate}");
            if (UsesItemContainerTemplate)
            {
            }

            var containerForItemOverride = base.GetContainerForItemOverride();
            Logger.Info($"container is {containerForItemOverride}");
            return containerForItemOverride;
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }

        public Guid ControlId { get; }
    }
}