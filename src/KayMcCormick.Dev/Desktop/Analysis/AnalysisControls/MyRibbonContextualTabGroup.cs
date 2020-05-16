using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MyRibbonContextualTabGroup : RibbonContextualTabGroup, IAppControl
    {
        private TabsEnumerable _tabs;

        static MyRibbonContextualTabGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRibbonContextualTabGroup),
                new FrameworkPropertyMetadata(typeof(MyRibbonContextualTabGroup)));

            UIElement.VisibilityProperty.OverrideMetadata(typeof(MyRibbonContextualTabGroup), (PropertyMetadata)new FrameworkPropertyMetadata((object)Visibility.Collapsed, new PropertyChangedCallback(OnVisibilityChanged), new CoerceValueCallback(CoerceVisibility)));
        }

        public MyRibbonContextualTabGroup()
        {
        }

        internal IEnumerable<MyRibbonTab> Tabs
        {
            get
            {
                if (this._tabs == null)
                    this._tabs = new MyRibbonContextualTabGroup.TabsEnumerable(this);
                return (IEnumerable<MyRibbonTab>)this._tabs;
            }
        }
        private class TabsEnumerable : IEnumerable<MyRibbonTab>, IEnumerable
        {
            public TabsEnumerable(MyRibbonContextualTabGroup tabGroup)
            {
                this.ContextualTabGroup = tabGroup;
            }

            private MyRibbonContextualTabGroup ContextualTabGroup { get; set; }

            public IEnumerator<MyRibbonTab> GetEnumerator()
            {
                MyRibbon ribbon = ContextualTabGroup.MyRibbon;
                if (ribbon != null)
                {
                    int itemCount = ribbon.Items.Count;
                    for (int i = 0; i < itemCount; ++i)
                    {
                        if (ribbon.ItemContainerGenerator.ContainerFromIndex(i) is MyRibbonTab ribbonTab && ribbonTab.IsContextualTab && this.ContextualTabGroup == ribbonTab.ContextualTabGroup)
                            yield return ribbonTab;
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return (IEnumerator)this.GetEnumerator();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MyRibbon MyRibbon { get; set; }


        private static void OnVisibilityChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

            DebugUtils.WriteLine("In onvisibilitychanged from " + e.OldValue + " to " + e.NewValue);
            ((MyRibbonContextualTabGroup)sender).CoerceTabsVisibility();
        }

        private void CoerceTabsVisibility()
        {
            RibbonDebugUtils.DumpPropertySource(this, VisibilityProperty);
            DebugUtils.WriteLine("Coerce tabs visibility");
            IEnumerable<MyRibbonTab> tabs = this.Tabs;
            if (tabs == null)
                return;
            foreach (DependencyObject dependencyObject in tabs)
            {
                DebugUtils.WriteLine("Calling CoerceValue Visibility property on " + dependencyObject);
                dependencyObject.CoerceValue(UIElement.VisibilityProperty);
                DebugUtils.WriteLine("value is " + dependencyObject.GetValue(VisibilityProperty));;

            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            RibbonDebugUtils.DumpPropertySource(this, VisibilityProperty);
        }

        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            base.OnTemplateChanged(oldTemplate, newTemplate);
            RibbonDebugUtils.DumpPropertySource(this, VisibilityProperty);
        }

        protected override void OnStyleChanged(Style oldStyle, Style newStyle)
        {
            base.OnStyleChanged(oldStyle, newStyle);

            RibbonDebugUtils.DumpPropertySource(this, VisibilityProperty);
            // if (DependencyPropertyHelper.IsTemplatedValueDynamic(this, VisibilityProperty))
            // {
                // DebugUtils.WriteLine($"Templated value is dynamic");

            // }
        }

        private static object CoerceVisibility(DependencyObject d, object basevalue)
        {
            DebugUtils.WriteLine("Coercing visibility for " + d + " (basevalue is " + basevalue + ")");
            var coerceVisibility = (Visibility)basevalue == Visibility.Hidden ? (object)Visibility.Collapsed : basevalue;
            DebugUtils.WriteLine("Result visibility for " + d + " is " + coerceVisibility);
            return coerceVisibility;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.Name == "Visibility")
            {
                DebugUtils.WriteLine("XONTEXTUAL TAB GROUP VISIBILTY SET " + e.NewValue);
            }
            RibbonDebugUtils.OnPropertyChanged(this.ToString(), this, e);
        }

        public override string ToString()
        {
            return $"MyContextualTabGroup[<{Header?.GetType().Name??"Null"}>{Header};{DataContext};{ControlId}]";
        }

        /// <inheritdoc />
        public Guid ControlId { get; } = Guid.NewGuid();


    }
}