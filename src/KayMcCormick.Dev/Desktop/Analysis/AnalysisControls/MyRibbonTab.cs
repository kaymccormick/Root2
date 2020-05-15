using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Windows.Shapes;
using AnalysisControls.RibbonModel;
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
        protected readonly Logger Logger;
        private Regex _r = new Regex(@"[\. ;'""]");
        private string _loggerName;
        private Brush _originalBorderBrush;
        private Thickness _originalBorderThickness;
        private Grid _mainGrid;
        private Rectangle _overlayRect;

        static MyRibbonTab()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRibbonTab),
                new FrameworkPropertyMetadata(typeof(MyRibbonTab)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _mainGrid = GetTemplateChild("MainGrid") as Grid;
            _overlayRect = GetTemplateChild("OverlayRect") as Rectangle;
            if (_mainGrid != null)
                _mainGrid.LayoutUpdated += (sender, args) =>
                {
                    var grid = _mainGrid;
                    DebugUtils.WriteLine($"{this} LAyout updated: {grid?.ActualWidth} {grid?.ActualHeight}");
                };
        }

        private static object CoerceVisibility(DependencyObject d, object value)
        {
            Visibility visibility1 = (Visibility)value;
            Visibility visibility2 = Visibility.Visible;
            RibbonTab ribbonTab = (RibbonTab)d;
            bool flag = ribbonTab.ContextualTabGroupHeader != null;
            // if (ribbonTab.ContextualTabGroup == null & flag && ribbonTab.Ribbon != null && ribbonTab.Ribbon.ContextualTabGroupItemsControl != null)
                // ribbonTab.ContextualTabGroup = ribbonTab.Ribbon.ContextualTabGroupItemsControl.FindHeader(ribbonTab.ContextualTabGroupHeader);
            if (ribbonTab.ContextualTabGroup != null)
                visibility2 = ribbonTab.ContextualTabGroup.Visibility;
            else if (flag)
                visibility2 = Visibility.Collapsed;
            return visibility1 != Visibility.Visible || visibility2 != Visibility.Visible ? (object)Visibility.Collapsed : (object)Visibility.Visible;
        }

        private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
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
            if (!e.Handled)
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


                    DragDrop.DoDragDrop(this, ParentItemsControl.ItemContainerGenerator.ItemFromContainer(this),
                        DragDropEffects.Copy);
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
                tabName = tab.Header.ToString();
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
    }
}