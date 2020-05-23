using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    public class MyRibbonApplicationMenu : RibbonApplicationMenu
    {
        private object _currentItem;

        public MyRibbonApplicationMenu()
        {
            Background = Brushes.Green;
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


        static MyRibbonApplicationMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRibbonApplicationMenu),
                new FrameworkPropertyMetadata(typeof(MyRibbonApplicationMenu)));

        }

        protected override void OnStyleChanged(Style oldStyle, Style newStyle)
        {
            DebugUtils.WriteLine($"{this} {newStyle}");
            base.OnStyleChanged(oldStyle, newStyle);
            DebugUtils.WriteLine($"!{this} {newStyle}");
        }

        /// <inheritdoc />
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            int num;
            switch (item)
            {
                case RibbonApplicationMenuItem _:
                case RibbonApplicationSplitMenuItem _:
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
                        case RibbonApplicationMenuItem _:
                        case RibbonApplicationSplitMenuItem _:
                        case RibbonSeparator _:
                        case RibbonGallery _:
                            return obj as DependencyObject;
                        default:
                            throw new AppInvalidOperationException("Invalid container");
                    }
                }
            }
            return (DependencyObject)new MyRibbonApplicationMenuItem();
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