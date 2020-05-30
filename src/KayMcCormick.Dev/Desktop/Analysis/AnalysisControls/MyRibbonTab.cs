using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;
using Autofac;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf;
using NLog;
using NLog.Fluent;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class MyRibbonTab : RibbonTab , IAppControl
    {
        /// <inheritdoc />
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
        }

        /// <inheritdoc />
        public override void BeginInit()
        {
            base.BeginInit();
        }

        /// <inheritdoc />
        public override void EndInit()
        {
            SetValue(ItemsSourceProperty, ((IHaveItems) DataContext).Items);
            base.EndInit();
        }

        /// <inheritdoc />
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            SetBinding(ItemsSourceProperty, new Binding("Items") {Source = DataContext});
            

        }

        /// <inheritdoc />
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
        }

        protected readonly Logger Logger;
        private Regex _r = new Regex(@"[\. ;'""]");
        private string _loggerName;
        private Brush _originalBorderBrush;
        private Thickness _originalBorderThickness;
        private Grid _mainGrid;
        private Rectangle _overlayRect;

        private static readonly DependencyProperty MyContextualTabGroupProperty = DependencyProperty.Register(nameof(MyContextualTabGroup), typeof(MyRibbonContextualTabGroup), typeof(MyRibbonTab), (PropertyMetadata)new FrameworkPropertyMetadata((object)null, new PropertyChangedCallback(MyRibbonTab.OnNotifyHeaderPropertyChanged)));
        private static MethodInfo _parentCoerce;

        private static void OnNotifyHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.MyRibbon.RibbonTab.ContextualTabGroup" /> dependency property.</summary>


        static MyRibbonTab()
        {
            _parentCoerce =
                typeof(RibbonTab).GetMethod("CoerceVisibility", BindingFlags.Static | BindingFlags.NonPublic);
            UIElement.VisibilityProperty.OverrideMetadata(typeof(MyRibbonTab), (PropertyMetadata)new FrameworkPropertyMetadata((object)Visibility.Visible, new PropertyChangedCallback(MyRibbonTab.OnVisibilityChanged),CoerceVisibility));

                DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRibbonTab),
                    new FrameworkPropertyMetadata(typeof(MyRibbonTab)));
            }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _mainGrid = GetTemplateChild("MainGrid") as Grid;
            _overlayRect = GetTemplateChild("OverlayRect") as Rectangle;
#if false
            if (_mainGrid != null)
                _mainGrid.LayoutUpdated += (sender, args) =>
                {
                    var grid = _mainGrid;
                    DebugUtils.WriteLine($"{this} LAyout updated: {grid?.ActualWidth} {grid?.ActualHeight}");
                };
#endif
        }

        private static object CoerceVisibility(DependencyObject d, object value)
        {
            if (_parentCoerce != null)
            {
                var result = _parentCoerce.Invoke(null, new object[] {d, value});
                // DebugUtils.WriteLine("parent coerce resulted in " + result + " for " + d);
            }
        
            Visibility visibility1 = (Visibility)value;
            // DebugUtils.WriteLine($"visibility1 = {visibility1}");
            Visibility visibility2 = Visibility.Visible;
            MyRibbonTab ribbonTab = (MyRibbonTab)d;
            // DebugUtils.WriteLine($"myRibbonTab = {ribbonTab}");
            bool flag = ribbonTab.ContextualTabGroupHeader != null;
            // DebugUtils.WriteLine($"flag = {flag} ({ribbonTab.ContextualTabGroupHeader}");
            if (ribbonTab.MyContextualTabGroup == null & flag && ribbonTab.MyRibbon != null && ribbonTab.MyRibbon.MyContextualTabGroupItemsControl != null)
                ribbonTab.MyContextualTabGroup = ribbonTab.MyRibbon.MyContextualTabGroupItemsControl.FindHeader(ribbonTab.ContextualTabGroupHeader);
            if (ribbonTab.MyContextualTabGroup != null)
            {
                visibility2 = ribbonTab.MyContextualTabGroup.Visibility;
                // DebugUtils.WriteLine($"visibility2 = {visibility2}");
            }
            else if (flag)
            {
                // DebugUtils.WriteLine("setting based on flag to collapsed");
                visibility2 = Visibility.Collapsed;
            }
            var coerceVisibility = visibility1 != Visibility.Visible || visibility2 != Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            if (coerceVisibility != visibility1)
            {
                DebugUtils.WriteLine("Visibility coercion resulted in " + coerceVisibility + " instead of " +
                                     visibility1);
            }
            return (object)coerceVisibility;
        }

        /// <summary>
        /// 
        /// </summary>
        public MyRibbonContextualTabGroup MyContextualTabGroup
        {
            get
            {
                return (MyRibbonContextualTabGroup) GetValue(MyContextualTabGroupProperty);
            }
            set
            {
                SetValue(MyContextualTabGroupProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MyRibbon MyRibbon
        {
            get
            {
                if (Ribbon == null)
                {

                }
                return Ribbon as MyRibbon;
            }
        }

        private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DebugUtils.WriteLine($"Visibility changed from {e.OldValue} to {e.NewValue}");
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            DebugUtils.WriteLine("Mouse up");
        }

        protected override void OnPreviewDrop(DragEventArgs e)
        {
            DebugUtils.WriteLine($"{e.OriginalSource} {e.Source}");
            base.OnPreviewDrop(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!e.Handled && e.Source == this)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (_overlayRect != null)
                    {
                        var val = _overlayRect.GetValue(Shape.StrokeProperty);
                    var src = DependencyPropertyHelper.GetValueSource(_overlayRect, Shape.StrokeProperty);
                    
                    DebugUtils.WriteLine($"{src.BaseValueSource}");
                    _overlayRect.SetCurrentValue(Shape.StrokeProperty, Brushes.Yellow);
                    _overlayRect.SetCurrentValue(Shape.StrokeThicknessProperty, 5.0);
                    }

                    try
                    {
                        DragDrop.DoDragDrop(this, ParentItemsControl.ItemContainerGenerator.ItemFromContainer(this),
                            DragDropEffects.Copy);
                    } catch{}
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MyRibbonTab()
        {
            AddHandler(AttachedProperties.LifetimeScopeChangedEvent, new RoutedPropertyChangedEventHandler<ILifetimeScope>(OnLifetimeScopeChanged));
            ControlId = Guid.NewGuid();
            UpdateLoggerName();
            Logger = LogManager.GetLogger($"RibbonTab.{ControlId.ToString().ToUpperInvariant()}");
        }

        private void UpdateLoggerName()
        {
            var headerStr = _r.Replace(Header?.ToString() ?? "null", "_");
            _loggerName = $"RibbonTab.{ControlId.ToString().ToUpperInvariant()}.{headerStr}";
        }

        private void OnLifetimeScopeChanged(object sender, RoutedPropertyChangedEventArgs<ILifetimeScope> e)
        {
            string FormatLifetimeScope(ILifetimeScope value)
            {
                return value != null ? value.Tag?.ToString() : "null";
            }

            if (ReferenceEquals(e.OriginalSource, this))
            {
                object[] info = new[] { e.Source, e.OriginalSource, e.RoutedEvent };
                var infos = string.Join(", ", info);
                var oldStr = FormatLifetimeScope(e.OldValue);
                var newStr = FormatLifetimeScope(e.NewValue);
                CreateLogBuilder().Message(
                    $"{nameof(OnLifetimeScopeChanged)}     OLD [ {oldStr} ]  NEW [ {newStr}  ]").Write();
            }
        }

        private LogBuilder CreateLogBuilder()
        {
            return new LogBuilder(Logger).LoggerName(LoggerName).Level(LogLevel);
        }

        public LogLevel LogLevel { get; set; } = LogLevel.Warn;

        public string LoggerName => _loggerName;

        /// <inheritdoc />
        protected override DependencyObject GetContainerForItemOverride()
        {
            var r = base.GetContainerForItemOverride();
            DebugUtils.WriteLine($"{nameof(GetContainerForItemOverride)} {r}");
            if (r.GetType() == typeof(RibbonGroup))
            {
                return new MyRibbonGroup();
            }
            else
            {
                DebugUtils.WriteLine("omg");
                return r;
            }
        }

        ItemsControl ParentItemsControl
        {
            get
            {
                return ItemsControl.ItemsControlFromItemContainer(this);
            }
        }


        /// <inheritdoc />
        protected override void OnSelected(RoutedEventArgs e)
        {
            var item = ParentItemsControl.ItemContainerGenerator.ItemFromContainer(this);
            var tabName = "";
            if (item is RibbonModelTab tab)
            {
                tabName = tab.Header?.ToString();
            }

            Logger.Info("RibbonTab selected: " + tabName);
            base.OnSelected(e);
        }

        /// <inheritdoc />
        protected override void OnUnselected(RoutedEventArgs e)
        {
            Logger.Info("RibbonTab unselected: " + this);
            base.OnUnselected(e);
        }

        /// <inheritdoc />
        protected override void OnHeaderChanged(object oldHeader, object newHeader)
        {
            Logger.Info("Header changed from {old} to {new}", oldHeader, newHeader);
            base.OnHeaderChanged(oldHeader, newHeader);
        }

        /// <inheritdoc />
        public Guid ControlId { get; }

        internal bool IsContextualTab
        {
            get
            {
                return this.ContextualTabGroupHeader != null;
            }
        }

    }

    public interface IHaveItems
    {
        IEnumerable Items { get; set; }
    }
}