using Autofac;
using Castle.DynamicProxy;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf;
using NLog;
using NLog.Fluent;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using RibbonLib;
using RibbonLib.Model;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MyRibbon : Ribbon, IContainItemStorage
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private RibbonContextualTabGroupItemsControl _contextualTabGroupItemsControl;

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            DebugUtils.WriteLine($"{this}.{nameof(OnApplyTemplate)}");
            base.OnApplyTemplate();
            DebugUtils.WriteLine($"post {this}.{nameof(OnApplyTemplate)}");
            MyContextualTabGroupItemsControl = GetTemplateChild("PART_ContextualTabGroupItemsControl") as MyRibbonContextualTabGroupItemsControl;
            if (MyContextualTabGroupItemsControl != null) MyContextualTabGroupItemsControl.Logger = Logger;
            _contextualTabGroupItemsControl =
                GetTemplateChild("PART_ContextualTabGroupItemsControl") as RibbonContextualTabGroupItemsControl;
        }

        private static void RibbonQATUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ribbon = ((MyRibbon)d);
            ribbon.MyQuickAccessToolBar = e.NewValue as MyRibbonQuickAccessToolBar;
            // RibbonQuickAccessToolBar oldValue = e.OldValue as RibbonQuickAccessToolBar;
            // RibbonQuickAccessToolBar newValue = e.NewValue as RibbonQuickAccessToolBar;
            // if (oldValue != null)
                // ribbon.RemoveLogicalChild((object)oldValue);
            // if (newValue == null)
                // return;
            // ribbon.AddLogicalChild((object)newValue);
        }

        static MyRibbon()
       {
           var type = typeof(MyRibbon);
           DefaultStyleKeyProperty.OverrideMetadata(type,
               new FrameworkPropertyMetadata(type));
           FrameworkElement.ContextMenuProperty.OverrideMetadata(type, (PropertyMetadata)new FrameworkPropertyMetadata(new PropertyChangedCallback(RibbonHelper.OnContextMenuChanged), new CoerceValueCallback(RibbonHelper.OnCoerceContextMenu)));
            ItemsPanelProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(new ItemsPanelTemplate(new FrameworkElementFactory(typeof(MyRibbonTabsPanel)))));
            EventManager.RegisterClassHandler(type, RibbonControlService.DismissPopupEvent, (Delegate)new RibbonDismissPopupEventHandler(MyRibbon.OnDismissPopupThunk));
            AttachedProperties.LifetimeScopeProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.Inherits, null, CoerceLifetimeScope));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding((ICommand)RibbonCommands.AddToQuickAccessToolBarCommand, new ExecutedRoutedEventHandler(MyRibbon.AddToQATExecuted), new CanExecuteRoutedEventHandler(MyRibbon.AddToQATCanExecute)));
            Ribbon.QuickAccessToolBarProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(null, RibbonQATUpdated));

        }

        private static void OnDismissPopupThunk(object sender, RibbonDismissPopupEventArgs e)
        {
        }

        private static void AddToQATCanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            DependencyObject thatCanBeAddedToQat = MyRibbon.FindElementThatCanBeAddedToQAT(args.OriginalSource as DependencyObject);
            if (thatCanBeAddedToQat == null || RibbonControlService.GetQuickAccessToolBarId(thatCanBeAddedToQat) == null || RibbonHelper.ExistsInQAT(thatCanBeAddedToQat))
                return;
            args.CanExecute = true;
        }

        private static DependencyObject FindElementThatCanBeAddedToQAT(DependencyObject obj)
        {
            while (obj != null && !RibbonControlService.GetCanAddToQuickAccessToolBarDirectly(obj))
                obj = TreeHelper.GetParent(obj);
            return obj;
        }

        private static void AddToQATExecuted(object sender, ExecutedRoutedEventArgs args)
        {
            var ribbon0 = (MyRibbon) sender;
            object model = AttachedProperties.GetModel(ribbon0);
            if ((model is PrimaryRibbonModel primary))
            {
                if (!(MyRibbon.FindElementThatCanBeAddedToQAT((DependencyObject)(args.OriginalSource as UIElement)) is UIElement thatCanBeAddedToQat))
                    return;
                RibbonQuickAccessToolBarCloneEventArgs barCloneEventArgs = new RibbonQuickAccessToolBarCloneEventArgs(thatCanBeAddedToQat);
                thatCanBeAddedToQat.RaiseEvent((RoutedEventArgs)barCloneEventArgs);
                MyRibbon ribbon = RibbonControlService.GetRibbon((DependencyObject)thatCanBeAddedToQat) as MyRibbon;
                if (barCloneEventArgs.CloneInstance == null)
                    return;
                primary.QuickAccessToolBar.Items.Add((object)barCloneEventArgs.CloneInstance);
                args.Handled = true;
            }
        }

        private static object CoerceLifetimeScope(DependencyObject d, object basevalue)
        {
            ILifetimeScope lifetimeScope = (ILifetimeScope) basevalue;
            return lifetimeScope.BeginLifetimeScope($"Scope for {d}");
        }

        /// <summary>
        /// 
        /// </summary>
        public MyRibbon()
        {
            AddHandler(AttachedProperties.LifetimeScopeChangedEvent, new RoutedPropertyChangedEventHandler<ILifetimeScope>(OnLifetimeScopeChanged));
            
        }

        public override void BeginInit()
        {
                base.BeginInit();
            var lifetimeScope = (ILifetimeScope)GetValue(AttachedProperties.LifetimeScopeProperty);
            var parent2 = GetUIParentCore();
            var parentLifetime = (ILifetimeScope)parent2?.GetValue(AttachedProperties.LifetimeScopeProperty);
            if (parentLifetime != null)
                SetValue(AttachedProperties.LifetimeScopeProperty, parentLifetime.BeginLifetimeScope("MyRibbon scope"));
        }

        protected override void OnInitialized(EventArgs e)
        {
            Logger.Info("OninitializeD");
            base.OnInitialized(e);
        }

        protected override void AddChild(object value)
        {
            Logger.Info("AddChild");
            base.AddChild(value);
        }

        protected override void AddText(string text)
        {
            Logger.Info("AddCText");
            base.AddText(text);
        }

        protected override void ParentLayoutInvalidated(UIElement child)
        {
            base.ParentLayoutInvalidated(child);
        }

        /// <inheritdoc />
        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            Logger.Info("VisualParentChanged");
            base.OnVisualParentChanged(oldParent);
            var p = GetUIParentCore();
            Logger.Info($"{p}");
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
            DebugUtils.WriteLine($"post {this}.{nameof(OnTemplateChanged)}");
        }

        protected override Size MeasureOverride(Size constraint)
        {
            DebugUtils.WriteLine($"{this}.{nameof(MeasureOverride)} {constraint}");
            var measureOverride = base.MeasureOverride(constraint);
            DebugUtils.WriteLine($"{this}.{nameof(MeasureOverride)} {constraint} =  {measureOverride}");
            return measureOverride;
        }

        public override void EndInit()
        {
            Logger.Info("EndInit");
            var myParent = LogicalTreeHelper.GetParent(this);
            var parent2 = GetUIParentCore();
            base.EndInit();
        }

        private void OnLifetimeScopeChanged(object sender, RoutedPropertyChangedEventArgs<ILifetimeScope> e)
        {
            string FormatLifetimeScope(ILifetimeScope value)
            {
                return value != null ? value.Tag?.ToString() : "null";
            }

            if (!ReferenceEquals(e.OriginalSource, this)) return;

            // object[] info = new[] { e.Source, e.OriginalSource, e.RoutedEvent };
            // var infos = string.Join(", ", info);
            var oldStr = FormatLifetimeScope(e.OldValue);
            var newStr = FormatLifetimeScope(e.NewValue);
            CreateLogBuilder().Message(
                $"{nameof(OnLifetimeScopeChanged)}     OLD [ {oldStr} ]  NEW [ {newStr}  ]").Write();

        }

        private LogBuilder CreateLogBuilder()
        {
            return new LogBuilder(Logger).Level(LogLevel);
        }

        /// <summary>
        /// 
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.Warn;

        /// <inheritdoc />
        protected override DependencyObject GetContainerForItemOverride()
        {
            ProxyGenerator proxyGen = new ProxyGenerator();
            var containerForItemOverride = new MyRibbonTab();
            if (EnableProxy)
            {
                var interceptor = new BaseInterceptorImpl(UseLogMethod, ProxyGeneratorHelper.ProxyGenerator);
                interceptor.Callback = ProxyCallback;
                var tabPRoxy = proxyGen.CreateClassProxyWithTarget(containerForItemOverride,
                    new ProxyGenerationOptions(new MyRibbonGenHook()),
                    interceptor);

                return tabPRoxy;
            }



            return containerForItemOverride;
        }

        private void ProxyCallback(IInvocation obj)
        {
            if (obj.Method.Name == "OnPropertyChanged")
            {
                DependencyPropertyChangedEventArgs args = (DependencyPropertyChangedEventArgs) obj.Arguments[0];
                var o =args.Property.OwnerType;
                DebugUtils.WriteLine($"{o} {obj.InvocationTarget} {args.Property.Name} {args.OldValue} {args.NewValue}");
            }
            else
            {
                if (obj.Method.Name == "get_Items")
                {

                }
            }
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool EnableProxy { get; set; } = false;

        /// <inheritdoc />
        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
        }

        /// <inheritdoc />
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return base.IsItemItsOwnContainerOverride(item);
        }

        /// <inheritdoc />
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
        }

        /// <inheritdoc />
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return base.OnCreateAutomationPeer();
        }

        /// <inheritdoc />
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
        }

        /// <inheritdoc />
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }

        /// <inheritdoc />
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        /// <inheritdoc />
        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
        }

        /// <inheritdoc />
        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusWithinChanged(e);
        }

        /// <inheritdoc />
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
        }

        /// <inheritdoc />
        protected override void OnDisplayMemberPathChanged(string oldDisplayMemberPath, string newDisplayMemberPath)
        {
            base.OnDisplayMemberPathChanged(oldDisplayMemberPath, newDisplayMemberPath);
        }

        /// <inheritdoc />
        protected override void OnItemTemplateSelectorChanged(DataTemplateSelector oldItemTemplateSelector,
            DataTemplateSelector newItemTemplateSelector)
        {
            base.OnItemTemplateSelectorChanged(oldItemTemplateSelector, newItemTemplateSelector);
        }

        /// <inheritdoc />
        protected override void OnItemStringFormatChanged(string oldItemStringFormat, string newItemStringFormat)
        {
            base.OnItemStringFormatChanged(oldItemStringFormat, newItemStringFormat);
        }

        /// <inheritdoc />
        protected override void OnItemBindingGroupChanged(BindingGroup oldItemBindingGroup, BindingGroup newItemBindingGroup)
        {
            base.OnItemBindingGroupChanged(oldItemBindingGroup, newItemBindingGroup);
        }

        /// <inheritdoc />
        protected override void OnItemContainerStyleChanged(Style oldItemContainerStyle, Style newItemContainerStyle)
        {
            base.OnItemContainerStyleChanged(oldItemContainerStyle, newItemContainerStyle);
        }

        /// <inheritdoc />
        protected override void OnItemContainerStyleSelectorChanged(StyleSelector oldItemContainerStyleSelector,
            StyleSelector newItemContainerStyleSelector)
        {
            base.OnItemContainerStyleSelectorChanged(oldItemContainerStyleSelector, newItemContainerStyleSelector);
        }

        /// <inheritdoc />
        protected override void OnItemsPanelChanged(ItemsPanelTemplate oldItemsPanel, ItemsPanelTemplate newItemsPanel)
        {
            base.OnItemsPanelChanged(oldItemsPanel, newItemsPanel);
        }

        /// <inheritdoc />
        protected override void OnGroupStyleSelectorChanged(GroupStyleSelector oldGroupStyleSelector, GroupStyleSelector newGroupStyleSelector)
        {
            base.OnGroupStyleSelectorChanged(oldGroupStyleSelector, newGroupStyleSelector);
        }

        /// <inheritdoc />
        protected override void OnAlternationCountChanged(int oldAlternationCount, int newAlternationCount)
        {
            base.OnAlternationCountChanged(oldAlternationCount, newAlternationCount);
        }

        /// <inheritdoc />
        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            base.OnTextInput(e);
        }

        /// <inheritdoc />
        protected override bool ShouldApplyItemContainerStyle(DependencyObject container, object item)
        {
            return base.ShouldApplyItemContainerStyle(container, item);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return base.ToString();
        }

        /// <inheritdoc />
        protected override void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDoubleClick(e);
        }

        /// <inheritdoc />
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
        }

        /// <inheritdoc />
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            return base.ArrangeOverride(arrangeBounds);
        }

        /// <inheritdoc />
        protected override void OnStyleChanged(Style oldStyle, Style newStyle)
        {
            base.OnStyleChanged(oldStyle, newStyle);
        }

        /// <inheritdoc />
        protected override Visual GetVisualChild(int index)
        {
            return base.GetVisualChild(index);
        }

        /// <inheritdoc />
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }

        /// <inheritdoc />
        protected override DependencyObject GetUIParentCore()
        {
            return base.GetUIParentCore();
        }

        /// <inheritdoc />
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
        }

        /// <inheritdoc />
        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            return base.GetLayoutClip(layoutSlotSize);
        }

        /// <inheritdoc />
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
        }

        /// <inheritdoc />
        protected override void OnToolTipOpening(ToolTipEventArgs e)
        {
            base.OnToolTipOpening(e);
        }

        /// <inheritdoc />
        protected override void OnToolTipClosing(ToolTipEventArgs e)
        {
            base.OnToolTipClosing(e);
        }

        /// <inheritdoc />
        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            base.OnContextMenuOpening(e);
        }

        /// <inheritdoc />
        protected override void OnContextMenuClosing(ContextMenuEventArgs e)
        {
            base.OnContextMenuClosing(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
        }

        /// <inheritdoc />
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
        }

        /// <inheritdoc />
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);
        }

        /// <inheritdoc />
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonDown(e);
        }

        /// <inheritdoc />
        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonUp(e);
        }

        /// <inheritdoc />
        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonUp(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
        }

        /// <inheritdoc />
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        /// <inheritdoc />
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
        }

        /// <inheritdoc />
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
        }

        /// <inheritdoc />
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
        }

        /// <inheritdoc />
        protected override void OnGotMouseCapture(MouseEventArgs e)
        {
            base.OnGotMouseCapture(e);
        }

        /// <inheritdoc />
        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            base.OnLostMouseCapture(e);
        }

        /// <inheritdoc />
        protected override void OnQueryCursor(QueryCursorEventArgs e)
        {
            base.OnQueryCursor(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewStylusDown(StylusDownEventArgs e)
        {
            base.OnPreviewStylusDown(e);
        }

        /// <inheritdoc />
        protected override void OnStylusDown(StylusDownEventArgs e)
        {
            base.OnStylusDown(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewStylusUp(StylusEventArgs e)
        {
            base.OnPreviewStylusUp(e);
        }

        /// <inheritdoc />
        protected override void OnStylusUp(StylusEventArgs e)
        {
            base.OnStylusUp(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewStylusMove(StylusEventArgs e)
        {
            base.OnPreviewStylusMove(e);
        }

        /// <inheritdoc />
        protected override void OnStylusMove(StylusEventArgs e)
        {
            base.OnStylusMove(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewStylusInAirMove(StylusEventArgs e)
        {
            base.OnPreviewStylusInAirMove(e);
        }

        /// <inheritdoc />
        protected override void OnStylusInAirMove(StylusEventArgs e)
        {
            base.OnStylusInAirMove(e);
        }

        /// <inheritdoc />
        protected override void OnStylusEnter(StylusEventArgs e)
        {
            base.OnStylusEnter(e);
        }

        /// <inheritdoc />
        protected override void OnStylusLeave(StylusEventArgs e)
        {
            base.OnStylusLeave(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewStylusInRange(StylusEventArgs e)
        {
            base.OnPreviewStylusInRange(e);
        }

        /// <inheritdoc />
        protected override void OnStylusInRange(StylusEventArgs e)
        {
            base.OnStylusInRange(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewStylusOutOfRange(StylusEventArgs e)
        {
            base.OnPreviewStylusOutOfRange(e);
        }

        /// <inheritdoc />
        protected override void OnStylusOutOfRange(StylusEventArgs e)
        {
            base.OnStylusOutOfRange(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewStylusSystemGesture(StylusSystemGestureEventArgs e)
        {
            base.OnPreviewStylusSystemGesture(e);
        }

        /// <inheritdoc />
        protected override void OnStylusSystemGesture(StylusSystemGestureEventArgs e)
        {
            base.OnStylusSystemGesture(e);
        }

        /// <inheritdoc />
        protected override void OnGotStylusCapture(StylusEventArgs e)
        {
            base.OnGotStylusCapture(e);
        }

        /// <inheritdoc />
        protected override void OnLostStylusCapture(StylusEventArgs e)
        {
            base.OnLostStylusCapture(e);
        }

        /// <inheritdoc />
        protected override void OnStylusButtonDown(StylusButtonEventArgs e)
        {
            base.OnStylusButtonDown(e);
        }

        /// <inheritdoc />
        protected override void OnStylusButtonUp(StylusButtonEventArgs e)
        {
            base.OnStylusButtonUp(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewStylusButtonDown(StylusButtonEventArgs e)
        {
            base.OnPreviewStylusButtonDown(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewStylusButtonUp(StylusButtonEventArgs e)
        {
            base.OnPreviewStylusButtonUp(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            base.OnPreviewKeyUp(e);
        }

        /// <inheritdoc />
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnPreviewGotKeyboardFocus(e);
        }

        /// <inheritdoc />
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnPreviewLostKeyboardFocus(e);
        }

        /// <inheritdoc />
        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnLostKeyboardFocus(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            base.OnPreviewQueryContinueDrag(e);
        }

        /// <inheritdoc />
        protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            base.OnQueryContinueDrag(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewGiveFeedback(GiveFeedbackEventArgs e)
        {
            base.OnPreviewGiveFeedback(e);
        }

        /// <inheritdoc />
        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewDragEnter(DragEventArgs e)
        {
            base.OnPreviewDragEnter(e);
        }

        /// <inheritdoc />
        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewDragOver(DragEventArgs e)
        {
            base.OnPreviewDragOver(e);
        }

        /// <inheritdoc />
        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewDragLeave(DragEventArgs e)
        {
            base.OnPreviewDragLeave(e);
        }

        /// <inheritdoc />
        protected override void OnDragLeave(DragEventArgs e)
        {
            base.OnDragLeave(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewDrop(DragEventArgs e)
        {
            base.OnPreviewDrop(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewTouchDown(TouchEventArgs e)
        {
            base.OnPreviewTouchDown(e);
        }

        /// <inheritdoc />
        protected override void OnTouchDown(TouchEventArgs e)
        {
            base.OnTouchDown(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewTouchMove(TouchEventArgs e)
        {
            base.OnPreviewTouchMove(e);
        }

        /// <inheritdoc />
        protected override void OnTouchMove(TouchEventArgs e)
        {
            base.OnTouchMove(e);
        }

        /// <inheritdoc />
        protected override void OnPreviewTouchUp(TouchEventArgs e)
        {
            base.OnPreviewTouchUp(e);
        }

        /// <inheritdoc />
        protected override void OnTouchUp(TouchEventArgs e)
        {
            base.OnTouchUp(e);
        }

        /// <inheritdoc />
        protected override void OnGotTouchCapture(TouchEventArgs e)
        {
            base.OnGotTouchCapture(e);
        }

        /// <inheritdoc />
        protected override void OnLostTouchCapture(TouchEventArgs e)
        {
            base.OnLostTouchCapture(e);
        }

        /// <inheritdoc />
        protected override void OnTouchEnter(TouchEventArgs e)
        {
            base.OnTouchEnter(e);
        }

        /// <inheritdoc />
        protected override void OnTouchLeave(TouchEventArgs e)
        {
            base.OnTouchLeave(e);
        }

        /// <inheritdoc />
        protected override void OnIsMouseDirectlyOverChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsMouseDirectlyOverChanged(e);
        }

        /// <inheritdoc />
        protected override void OnIsMouseCapturedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsMouseCapturedChanged(e);
        }

        /// <inheritdoc />
        protected override void OnIsMouseCaptureWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsMouseCaptureWithinChanged(e);
        }

        /// <inheritdoc />
        protected override void OnIsStylusDirectlyOverChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsStylusDirectlyOverChanged(e);
        }

        /// <inheritdoc />
        protected override void OnIsStylusCapturedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsStylusCapturedChanged(e);
        }

        /// <inheritdoc />
        protected override void OnIsStylusCaptureWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsStylusCaptureWithinChanged(e);
        }

        /// <inheritdoc />
        protected override void OnIsKeyboardFocusedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusedChanged(e);
        }

        /// <inheritdoc />
        protected override void OnChildDesiredSizeChanged(UIElement child)
        {
            base.OnChildDesiredSizeChanged(child);
        }

        /// <inheritdoc />
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRoundedRectangle(Brushes.Green, null,new Rect(10,10,70,70), 10,10);
        }

        /// <inheritdoc />
        protected override void OnAccessKey(AccessKeyEventArgs e)
        {
            base.OnAccessKey(e);
        }

        /// <inheritdoc />
        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            return base.HitTestCore(hitTestParameters);
        }

        /// <inheritdoc />
        protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters)
        {
            return base.HitTestCore(hitTestParameters);
        }

        /// <inheritdoc />
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
        }

        /// <inheritdoc />
        protected override void OnManipulationStarting(ManipulationStartingEventArgs e)
        {
            base.OnManipulationStarting(e);
        }

        /// <inheritdoc />
        protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
        {
            base.OnManipulationStarted(e);
        }

        /// <inheritdoc />
        protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        {
            base.OnManipulationDelta(e);
        }

        /// <inheritdoc />
        protected override void OnManipulationInertiaStarting(ManipulationInertiaStartingEventArgs e)
        {
            base.OnManipulationInertiaStarting(e);
        }

        /// <inheritdoc />
        protected override void OnManipulationBoundaryFeedback(ManipulationBoundaryFeedbackEventArgs e)
        {
            base.OnManipulationBoundaryFeedback(e);
        }

        /// <inheritdoc />
        protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
        {
            base.OnManipulationCompleted(e);
        }

        /// <inheritdoc />
        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
            DebugUtils.WriteLine($"{visualAdded} {visualRemoved}");
        }

        /// <inheritdoc />
        protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
        {
            base.OnDpiChanged(oldDpi, newDpi);
        }

        /// <summary>
        /// 
        /// </summary>
        public MyRibbonContextualTabGroupItemsControl MyContextualTabGroupItemsControl { get; set; }

        public MyRibbonQuickAccessToolBar MyQuickAccessToolBar { get; set; }


        
        private void UseLogMethod(string message)
        {
            Logger.Info(message);
        }

      
        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            // if (!e.Handled)
            // {
                // if (e.OriginalSource is FrameworkElement el)
                // {
                    // if (el.TemplatedParent != null)
                    // {
                        // if (el.TemplatedParent is ContentPresenter cp)
                        // {
                            // if (cp.Content is RibbonModelDropZone dz)
                            // {
                                // e.Effects= dz.OnDrop(e.Data, TODO);
                                // e.Handled = true;
                            // }
                        // }
                    // }
                // }
            // }
        }
    }

    public class MyRibbonGenHook : IProxyGenerationHook
    {
        /// <inheritdoc />
        public void MethodsInspected()
        {
        }

        /// <inheritdoc />
        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
        }

        /// <inheritdoc />
        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            if (methodInfo.Name == "get_Items")
                return true;
            if (methodInfo.MemberType == MemberTypes.Property)
                return false;
            if (methodInfo.Name.StartsWith("On") && methodInfo.Name.EndsWith("Changed"))
            {
                DebugUtils.WriteLine(methodInfo.Name);
                return true;
            }

            if (methodInfo.Name == "GetContainerForItemOverride")
                return true;
            if (methodInfo.Name.EndsWith("Override"))
                return true;
            if (methodInfo.Name.Contains("Automation"))
                return false;
            if (methodInfo.Name == "OnPropertyChanged")
                return true;
            return false;
        }
    }

    internal class RibbonHelper
    {
        internal static bool ExistsInQAT(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            var ribbon = (MyRibbon)element.GetValue(RibbonControlService.RibbonProperty);
            object quickAccessToolBarId = RibbonControlService.GetQuickAccessToolBarId(element);
            return ribbon != null && ribbon.MyQuickAccessToolBar != null && quickAccessToolBarId != null && ribbon.MyQuickAccessToolBar.ContainsId(quickAccessToolBarId);
        }

        public static object OnCoerceContextMenu(DependencyObject d, object basevalue)
        {
            return basevalue;
        }

        public static void OnContextMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }

    internal static class TreeHelper
    {
        public static DependencyObject GetParent(DependencyObject element)
        {
            DependencyObject dependencyObject = (DependencyObject)null;
            if (!(element is ContentElement))
                dependencyObject = VisualTreeHelper.GetParent(element);
            if (dependencyObject == null)
                dependencyObject = LogicalTreeHelper.GetParent(element);
            return dependencyObject;
        }

    }

    public class MyRibbonTabsPanel : RibbonTabsPanel {
        public override void OnApplyTemplate()
        {
            DebugUtils.WriteLine($"{this}.{nameof(OnApplyTemplate)}");
            base.OnApplyTemplate();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class MyRibbonApplicationMenuItem : RibbonApplicationMenuItem
    {
        private object _currentItem;


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
                        case RibbonApplicationMenuItem _:
                        case RibbonApplicationSplitMenuItem _:
                        case RibbonSeparator _:
                        case RibbonGallery _:
                            return obj as DependencyObject;
                        default:
                            throw new AppInvalidOperationException("invalid container");
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
    public class MyRibbonApplicationSplitMenuItem : RibbonApplicationSplitMenuItem
    {
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
    public class MyRibbonSplitMenuItem : RibbonSplitMenuItem
    {
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


        protected override Size MeasureOverride(Size constraint)
        {
            DebugUtils.WriteLine($"{this}.{nameof(MeasureOverride)}");
            return base.MeasureOverride(constraint);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            RibbonDebugUtils.OnPropertyChanged(this.ToString(), this, e);

        }

    }
    public class MyRibbonSeparator : RibbonSeparator
    {
        public override void OnApplyTemplate()
        {
            DebugUtils.WriteLine($"{this}.{nameof(OnApplyTemplate)}");
            base.OnApplyTemplate();
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