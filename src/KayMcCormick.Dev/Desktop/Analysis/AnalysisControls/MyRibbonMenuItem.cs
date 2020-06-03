using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using KayMcCormick.Dev;
using RibbonLib;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MyRibbonMenuItem : RibbonMenuItem
    {
        private object _currentItem;

        static MyRibbonMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRibbonMenuItem),
                new FrameworkPropertyMetadata(typeof(MyRibbonMenuItem)));

        }
        public override void OnApplyTemplate()
        {
            DebugUtils.WriteLine($"{this}.{nameof(OnApplyTemplate)}");
            base.OnApplyTemplate();
        }

        protected override void OnItemTemplateChanged(DataTemplate oldItemTemplate, DataTemplate newItemTemplate)
        {
            DebugUtils.WriteLine($"{this}.{nameof(OnItemTemplateChanged)}");
            base.OnItemTemplateChanged(oldItemTemplate, newItemTemplate);
        }

        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            DebugUtils.WriteLine($"{this}.{nameof(OnTemplateChanged)}");
            base.OnTemplateChanged(oldTemplate, newTemplate);
        }

        /// <inheritdoc />
        public MyRibbonMenuItem()
        {
            DependencyPropertyDescriptor dpd =
                DependencyPropertyDescriptor.FromProperty(RoleProperty, typeof(MyRibbonMenuItem));
            dpd.AddValueChanged(this, RoleChanged);
        }

        private void RoleChanged(object sender, EventArgs e)
        {
            RibbonDebugUtils.WriteLine($"Role changed: {Role}");
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }

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

        /// <inheritdoc />
        protected override DependencyObject GetContainerForItemOverride()
        {
            object currentItem = this._currentItem;
            this._currentItem = (object)null;
            if (this.UsesItemContainerTemplate)
            {
                DataTemplate dataTemplate =
                    this.ItemContainerTemplateSelector.SelectTemplate(currentItem, (ItemsControl) this);
                if (dataTemplate != null)
                {
                    object obj = (object) dataTemplate.LoadContent();
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
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            DebugUtils.WriteLine($"{this} {e.Property.Name} {e.NewValue}");
            base.OnPropertyChanged(e);
            RibbonDebugUtils.OnPropertyChanged(this.ToString(), this, e);

        }
        protected override Size MeasureOverride(Size constraint)
        {
            DebugUtils.WriteLine($"{this}.{nameof(MeasureOverride)}");
            return base.MeasureOverride(constraint);
        }

    }
}