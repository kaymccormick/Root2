using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MyRibbonMenuButton : RibbonMenuButton, IAppControl
    {
        static MyRibbonMenuButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRibbonMenuButton),
                new FrameworkPropertyMetadata(typeof(MyRibbonMenuButton)));

        }

        /// <inheritdoc />
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            RibbonDebugUtils.WriteLine($"{this} OnItemsChanged");
            base.OnItemsChanged(e);
        }

        private object _currentItem;

        /// <inheritdoc />
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
                        case RibbonMenuButton _:
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

        public Guid ControlId { get; } = Guid.NewGuid();
    }
}