using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using KayMcCormick.Dev;
using NLog;

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

        /// <summary>
        /// 
        /// </summary>
        public MyRibbonContextualTabGroup()
        {
            TargetUpdated += OnTargetUpdated;
            SourceUpdated += OnSourceUpdated;

        }

        private void OnSourceUpdated(object sender, DataTransferEventArgs e)
        {
            DebugUtils.WriteLine($"*** {e.Property.Name}", DebugCategory.DataBinding);
        }

        private void OnTargetUpdated(object sender, DataTransferEventArgs e)
        {
            DebugUtils.WriteLine($"*** {e.Property.Name}", DebugCategory.DataBinding);
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


        // private static void OnVisibilityChan?ged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        // {

        // }

        private void CoerceTabsVisibility()
        {
            RibbonDebugUtils.DumpPropertySource(this, VisibilityProperty);
            // Logger?.Info("Coerce tabs visibility");
            IEnumerable<MyRibbonTab> tabs = this.Tabs;
            if (tabs == null)
                return;
            foreach (DependencyObject dependencyObject in tabs)
            {
                // DebugUtils.WriteLine("Calling CoerceValue Visibility property on " + dependencyObject);
                dependencyObject.CoerceValue(UIElement.VisibilityProperty);
                // DebugUtils.WriteLine("value is " + dependencyObject.GetValue(VisibilityProperty));;

            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            // RibbonDebugUtils.DumpPropertySource(this, VisibilityProperty);
        }

        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            base.OnTemplateChanged(oldTemplate, newTemplate);
            // RibbonDebugUtils.DumpPropertySource(this, VisibilityProperty);
        }

        protected override void OnStyleChanged(Style oldStyle, Style newStyle)
        {
            DebugUtils.WriteLine($"Style changing: " + string.Join("\n", newStyle.Setters.Select(x =>
                    {
                        switch (x)
                        {
                            case EventSetter eventSetter:
                                break;
                            case Setter setter:
                                return $"{setter.Property.Name}={setter.Value}";
                            default:
                                return null;
                        }

                        return null;
                    }).Where(x=>x  != null)), DebugCategory.DataBinding);
                //MessageBox.Show("Test");, VisibilityProperty);
            // if (DependencyPropertyHelper.IsTemplatedValueDynamic(this, VisibilityProperty))
            // {
                // DebugUtils.WriteLine($"Templated value is dynamic");

            // }
        }

        private static object CoerceVisibility(DependencyObject d, object basevalue)
        {
            DebugUtils.WriteLine("Coercing visibility for " + d + " (basevalue is " + basevalue + ")", DebugCategory.DataBinding);
            var coerceVisibility = (Visibility)basevalue == Visibility.Hidden ? (object)Visibility.Collapsed : basevalue;
            DebugUtils.WriteLine("Result visibility for " + d + " is " + coerceVisibility, DebugCategory.DataBinding);
            return coerceVisibility;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            // DebugUtils.WriteLine($"Pre-base call {e.Property.Name}", DebugCategory.DataBinding);
            base.OnPropertyChanged(e);
            // DebugUtils.WriteLine($"return from base call {e.Property.Name}", DebugCategory.DataBinding);
            // if (e.Property.Name == "Visibility")
            // {
                // DebugUtils.WriteLine("XONTEXTUAL TAB GROUP VISIBILTY SET " + e.NewValue, DebugCategory.DataBinding);
            // }
            RibbonDebugUtils.OnPropertyChanged(this.ToString(), this, e);
        }

        public override string ToString()
        {
            return $"MyContextualTabGroup[<{Header?.GetType().Name??"Null"}>{Header};{DataContext};{ControlId}]";
        }

        /// <inheritdoc />
        public Guid ControlId { get; } = Guid.NewGuid();

        public Logger Logger { get; set; }


        private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DebugUtils.WriteLine("In onvisibilitychanged from " + e.OldValue + " to " + e.NewValue);
            ((MyRibbonContextualTabGroup)d).CoerceTabsVisibility();

        }
    }
}