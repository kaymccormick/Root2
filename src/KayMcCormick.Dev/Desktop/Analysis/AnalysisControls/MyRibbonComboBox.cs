using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using KayMcCormick.Dev;
using NLog;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MyRibbonComboBox : RibbonComboBox, IAppControl
    {
        static MyRibbonComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRibbonComboBox),
                new FrameworkPropertyMetadata(typeof(MyRibbonComboBox)));

        }
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
            object currentItem = this._currentItem;
            this._currentItem = (object)null;
            if (this.UsesItemContainerTemplate)
            {
                DataTemplate dataTemplate = this.ItemContainerTemplateSelector.SelectTemplate(currentItem, (ItemsControl)this);
                if (dataTemplate != null)
                {
                    object obj = (object)dataTemplate.LoadContent();
                    switch (obj)
                    {
                        case RibbonMenuItem _:
                        case RibbonGallery _:
                        case RibbonSeparator _:
                            return obj as DependencyObject;
                        default:
                            throw new AppInvalidOperationException("Invalid container");
                    }
                }
            }
            return (DependencyObject)new MyRibbonMenuItem();
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

        /// <inheritdoc />
        public Guid ControlId { get; }
    }
}