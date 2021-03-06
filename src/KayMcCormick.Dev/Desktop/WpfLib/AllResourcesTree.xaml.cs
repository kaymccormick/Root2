﻿using System ;
using System.Text.Json ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Media ;

using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Lib.Wpf.ViewModel ;
using NLog ;

// ReSharper disable RedundantOverriddenMember

namespace KayMcCormick.Lib.Wpf
{
#if CUSTOM_TREEVIEW
    public class TreeViewItem1 : TreeViewItem
    {
        private bool _hasEffectiveKeyboardFocus ;
        private bool _isEnabledCore ;
        #region Overrides of ItemsControl
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="item"></param>
        protected override void PrepareContainerForItemOverride (
            DependencyObject element
      , object           item
        )
        {
            DebugUtils.WriteLine ( item ) ;
            ( ( TreeViewItem1 ) element ).SetCurrentValue(TagProperty, item) ;
            base.PrepareContainerForItemOverride ( element , item ) ;
        }

        #region Overrides of TreeViewItem
        protected override void OnExpanded ( RoutedEventArgs e ) { base.OnExpanded ( e ) ; }

        protected override void OnCollapsed ( RoutedEventArgs e ) { base.OnCollapsed ( e ) ; }

        protected override void OnSelected ( RoutedEventArgs e ) { base.OnSelected ( e ) ; }

        protected override void OnUnselected ( RoutedEventArgs e ) { base.OnUnselected ( e ) ; }

        protected override Size ArrangeOverride ( Size arrangeSize )
        {
            Size returnVal = new Size(20, 50);
            try
            {
                returnVal = base.ArrangeOverride ( arrangeSize ) ;
            }
            catch (Exception ex)
            {

                DebugUtils.WriteLine(Tag + ":" + ex.ToString());
            }

            return returnVal;
        }

        protected override void OnVisualParentChanged ( DependencyObject oldParent ) { base.OnVisualParentChanged ( oldParent ) ; }

        protected override void OnGotFocus ( RoutedEventArgs e ) { base.OnGotFocus ( e ) ; }

        protected override void OnMouseLeftButtonDown ( MouseButtonEventArgs e ) { base.OnMouseLeftButtonDown ( e ) ; }

        protected override void OnKeyDown ( KeyEventArgs e ) { base.OnKeyDown ( e ) ; }

        protected override bool IsItemItsOwnContainerOverride ( object item ) { return base.IsItemItsOwnContainerOverride ( item ) ; }

        protected override void OnItemsChanged ( NotifyCollectionChangedEventArgs e ) { base.OnItemsChanged ( e ) ; }

        protected override AutomationPeer OnCreateAutomationPeer ( ) { return base.OnCreateAutomationPeer ( ) ; }
        #endregion
        #region Overrides of HeaderedItemsControl
        protected override void OnHeaderChanged ( object oldHeader , object newHeader ) { base.OnHeaderChanged ( oldHeader , newHeader ) ; }

        protected override void OnHeaderTemplateChanged ( DataTemplate oldHeaderTemplate , DataTemplate newHeaderTemplate ) { base.OnHeaderTemplateChanged ( oldHeaderTemplate , newHeaderTemplate ) ; }

        protected override void OnHeaderTemplateSelectorChanged (
            DataTemplateSelector oldHeaderTemplateSelector
      , DataTemplateSelector newHeaderTemplateSelector
        )
        {
            base.OnHeaderTemplateSelectorChanged ( oldHeaderTemplateSelector , newHeaderTemplateSelector ) ;
        }

        protected override void OnHeaderStringFormatChanged ( string oldHeaderStringFormat , string newHeaderStringFormat ) { base.OnHeaderStringFormatChanged ( oldHeaderStringFormat , newHeaderStringFormat ) ; }

        public override string ToString ( ) { return base.ToString ( ) ; }
        #endregion
        #region Overrides of Control
        protected override void OnTemplateChanged ( ControlTemplate oldTemplate , ControlTemplate newTemplate ) { base.OnTemplateChanged ( oldTemplate , newTemplate ) ; }

        protected override void OnPreviewMouseDoubleClick ( MouseButtonEventArgs e ) { base.OnPreviewMouseDoubleClick ( e ) ; }

        protected override void OnMouseDoubleClick ( MouseButtonEventArgs e ) { base.OnMouseDoubleClick ( e ) ; }

        protected override Size MeasureOverride ( Size constraint )
        {
            Size measureOverride = new Size(20, 50);
            try
            {
                measureOverride = base.MeasureOverride ( constraint ) ;
            }
            catch ( Exception ex )
            {
                DebugUtils.WriteLine ( ex.ToString ( ) ) ;
            }

            return measureOverride ;
        }
        #endregion
        #region Overrides of FrameworkElement
        protected override void OnStyleChanged ( Style oldStyle , Style newStyle ) { base.OnStyleChanged ( oldStyle , newStyle ) ; }

        protected override void ParentLayoutInvalidated ( UIElement child ) { base.ParentLayoutInvalidated ( child ) ; }

        public override void OnApplyTemplate ( ) { base.OnApplyTemplate ( ) ; }

        protected override Visual GetVisualChild ( int index ) { return base.GetVisualChild ( index ) ; }

        protected override void OnPropertyChanged ( DependencyPropertyChangedEventArgs e ) { base.OnPropertyChanged ( e ) ; }

        protected override DependencyObject GetUIParentCore ( ) { return base.GetUIParentCore ( ) ; }

        protected override void OnRenderSizeChanged ( SizeChangedInfo sizeInfo ) { base.OnRenderSizeChanged ( sizeInfo ) ; }

        protected override Geometry GetLayoutClip ( Size layoutSlotSize ) { return base.GetLayoutClip ( layoutSlotSize ) ; }

        protected override void OnInitialized ( EventArgs e ) { base.OnInitialized ( e ) ; }

        protected override void OnToolTipOpening ( ToolTipEventArgs e ) { base.OnToolTipOpening ( e ) ; }

        protected override void OnToolTipClosing ( ToolTipEventArgs e ) { base.OnToolTipClosing ( e ) ; }

        protected override void OnContextMenuOpening ( ContextMenuEventArgs e ) { base.OnContextMenuOpening ( e ) ; }

        protected override void OnContextMenuClosing ( ContextMenuEventArgs e ) { base.OnContextMenuClosing ( e ) ; }
        #endregion
        #region Overrides of UIElement
        protected override void OnPreviewMouseDown ( MouseButtonEventArgs e ) { base.OnPreviewMouseDown ( e ) ; }

        protected override void OnMouseDown ( MouseButtonEventArgs e ) { base.OnMouseDown ( e ) ; }

        protected override void OnPreviewMouseUp ( MouseButtonEventArgs e ) { base.OnPreviewMouseUp ( e ) ; }

        protected override void OnMouseUp ( MouseButtonEventArgs e ) { base.OnMouseUp ( e ) ; }

        protected override void OnPreviewMouseLeftButtonDown ( MouseButtonEventArgs e ) { base.OnPreviewMouseLeftButtonDown ( e ) ; }

        protected override void OnPreviewMouseLeftButtonUp ( MouseButtonEventArgs e ) { base.OnPreviewMouseLeftButtonUp ( e ) ; }

        protected override void OnMouseLeftButtonUp ( MouseButtonEventArgs e ) { base.OnMouseLeftButtonUp ( e ) ; }

        protected override void OnPreviewMouseRightButtonDown ( MouseButtonEventArgs e ) { base.OnPreviewMouseRightButtonDown ( e ) ; }

        protected override void OnMouseRightButtonDown ( MouseButtonEventArgs e ) { base.OnMouseRightButtonDown ( e ) ; }

        protected override void OnPreviewMouseRightButtonUp ( MouseButtonEventArgs e ) { base.OnPreviewMouseRightButtonUp ( e ) ; }

        protected override void OnMouseRightButtonUp ( MouseButtonEventArgs e ) { base.OnMouseRightButtonUp ( e ) ; }

        protected override void OnPreviewMouseMove ( MouseEventArgs e ) { base.OnPreviewMouseMove ( e ) ; }

        protected override void OnMouseMove ( MouseEventArgs e ) { base.OnMouseMove ( e ) ; }

        protected override void OnPreviewMouseWheel ( MouseWheelEventArgs e ) { base.OnPreviewMouseWheel ( e ) ; }

        protected override void OnMouseWheel ( MouseWheelEventArgs e ) { base.OnMouseWheel ( e ) ; }

        protected override void OnMouseEnter ( MouseEventArgs e ) { base.OnMouseEnter ( e ) ; }

        protected override void OnMouseLeave ( MouseEventArgs e ) { base.OnMouseLeave ( e ) ; }

        protected override void OnGotMouseCapture ( MouseEventArgs e ) { base.OnGotMouseCapture ( e ) ; }

        protected override void OnLostMouseCapture ( MouseEventArgs e ) { base.OnLostMouseCapture ( e ) ; }

        protected override void OnQueryCursor ( QueryCursorEventArgs e ) { base.OnQueryCursor ( e ) ; }

        protected override void OnPreviewStylusDown ( StylusDownEventArgs e ) { base.OnPreviewStylusDown ( e ) ; }

        protected override void OnStylusDown ( StylusDownEventArgs e ) { base.OnStylusDown ( e ) ; }

        protected override void OnPreviewStylusUp ( StylusEventArgs e ) { base.OnPreviewStylusUp ( e ) ; }

        protected override void OnStylusUp ( StylusEventArgs e ) { base.OnStylusUp ( e ) ; }

        protected override void OnPreviewStylusMove ( StylusEventArgs e ) { base.OnPreviewStylusMove ( e ) ; }

        protected override void OnStylusMove ( StylusEventArgs e ) { base.OnStylusMove ( e ) ; }

        protected override void OnPreviewStylusInAirMove ( StylusEventArgs e ) { base.OnPreviewStylusInAirMove ( e ) ; }

        protected override void OnStylusInAirMove ( StylusEventArgs e ) { base.OnStylusInAirMove ( e ) ; }

        protected override void OnStylusEnter ( StylusEventArgs e ) { base.OnStylusEnter ( e ) ; }

        protected override void OnStylusLeave ( StylusEventArgs e ) { base.OnStylusLeave ( e ) ; }

        protected override void OnPreviewStylusInRange ( StylusEventArgs e ) { base.OnPreviewStylusInRange ( e ) ; }

        protected override void OnStylusInRange ( StylusEventArgs e ) { base.OnStylusInRange ( e ) ; }

        protected override void OnPreviewStylusOutOfRange ( StylusEventArgs e ) { base.OnPreviewStylusOutOfRange ( e ) ; }

        protected override void OnStylusOutOfRange ( StylusEventArgs e ) { base.OnStylusOutOfRange ( e ) ; }

        protected override void OnPreviewStylusSystemGesture ( StylusSystemGestureEventArgs e ) { base.OnPreviewStylusSystemGesture ( e ) ; }

        protected override void OnStylusSystemGesture ( StylusSystemGestureEventArgs e ) { base.OnStylusSystemGesture ( e ) ; }

        protected override void OnGotStylusCapture ( StylusEventArgs e ) { base.OnGotStylusCapture ( e ) ; }

        protected override void OnLostStylusCapture ( StylusEventArgs e ) { base.OnLostStylusCapture ( e ) ; }

        protected override void OnStylusButtonDown ( StylusButtonEventArgs e ) { base.OnStylusButtonDown ( e ) ; }

        protected override void OnStylusButtonUp ( StylusButtonEventArgs e ) { base.OnStylusButtonUp ( e ) ; }

        protected override void OnPreviewStylusButtonDown ( StylusButtonEventArgs e ) { base.OnPreviewStylusButtonDown ( e ) ; }

        protected override void OnPreviewStylusButtonUp ( StylusButtonEventArgs e ) { base.OnPreviewStylusButtonUp ( e ) ; }

        protected override void OnPreviewKeyDown ( KeyEventArgs e ) { base.OnPreviewKeyDown ( e ) ; }

        protected override void OnPreviewKeyUp ( KeyEventArgs e ) { base.OnPreviewKeyUp ( e ) ; }

        protected override void OnKeyUp ( KeyEventArgs e ) { base.OnKeyUp ( e ) ; }

        protected override void OnPreviewGotKeyboardFocus ( KeyboardFocusChangedEventArgs e ) { base.OnPreviewGotKeyboardFocus ( e ) ; }

        protected override void OnGotKeyboardFocus ( KeyboardFocusChangedEventArgs e ) { base.OnGotKeyboardFocus ( e ) ; }

        protected override void OnPreviewLostKeyboardFocus ( KeyboardFocusChangedEventArgs e ) { base.OnPreviewLostKeyboardFocus ( e ) ; }

        protected override void OnLostKeyboardFocus ( KeyboardFocusChangedEventArgs e ) { base.OnLostKeyboardFocus ( e ) ; }

        protected override void OnPreviewTextInput ( TextCompositionEventArgs e ) { base.OnPreviewTextInput ( e ) ; }

        protected override void OnPreviewQueryContinueDrag ( QueryContinueDragEventArgs e ) { base.OnPreviewQueryContinueDrag ( e ) ; }

        protected override void OnQueryContinueDrag ( QueryContinueDragEventArgs e ) { base.OnQueryContinueDrag ( e ) ; }

        protected override void OnPreviewGiveFeedback ( GiveFeedbackEventArgs e ) { base.OnPreviewGiveFeedback ( e ) ; }

        protected override void OnGiveFeedback ( GiveFeedbackEventArgs e ) { base.OnGiveFeedback ( e ) ; }

        protected override void OnPreviewDragEnter ( DragEventArgs e ) { base.OnPreviewDragEnter ( e ) ; }

        protected override void OnDragEnter ( DragEventArgs e ) { base.OnDragEnter ( e ) ; }

        protected override void OnPreviewDragOver ( DragEventArgs e ) { base.OnPreviewDragOver ( e ) ; }

        protected override void OnDragOver ( DragEventArgs e ) { base.OnDragOver ( e ) ; }

        protected override void OnPreviewDragLeave ( DragEventArgs e ) { base.OnPreviewDragLeave ( e ) ; }

        protected override void OnDragLeave ( DragEventArgs e ) { base.OnDragLeave ( e ) ; }

        protected override void OnPreviewDrop ( DragEventArgs e ) { base.OnPreviewDrop ( e ) ; }

        protected override void OnDrop ( DragEventArgs e ) { base.OnDrop ( e ) ; }

        protected override void OnPreviewTouchDown ( TouchEventArgs e ) { base.OnPreviewTouchDown ( e ) ; }

        protected override void OnTouchDown ( TouchEventArgs e ) { base.OnTouchDown ( e ) ; }

        protected override void OnPreviewTouchMove ( TouchEventArgs e ) { base.OnPreviewTouchMove ( e ) ; }

        protected override void OnTouchMove ( TouchEventArgs e ) { base.OnTouchMove ( e ) ; }

        protected override void OnPreviewTouchUp ( TouchEventArgs e ) { base.OnPreviewTouchUp ( e ) ; }

        protected override void OnTouchUp ( TouchEventArgs e ) { base.OnTouchUp ( e ) ; }

        protected override void OnGotTouchCapture ( TouchEventArgs e ) { base.OnGotTouchCapture ( e ) ; }

        protected override void OnLostTouchCapture ( TouchEventArgs e ) { base.OnLostTouchCapture ( e ) ; }

        protected override void OnTouchEnter ( TouchEventArgs e ) { base.OnTouchEnter ( e ) ; }

        protected override void OnTouchLeave ( TouchEventArgs e ) { base.OnTouchLeave ( e ) ; }

        protected override void OnIsMouseDirectlyOverChanged ( DependencyPropertyChangedEventArgs e ) { base.OnIsMouseDirectlyOverChanged ( e ) ; }

        protected override void OnIsKeyboardFocusWithinChanged ( DependencyPropertyChangedEventArgs e ) { base.OnIsKeyboardFocusWithinChanged ( e ) ; }

        protected override void OnIsMouseCapturedChanged ( DependencyPropertyChangedEventArgs e ) { base.OnIsMouseCapturedChanged ( e ) ; }

        protected override void OnIsMouseCaptureWithinChanged ( DependencyPropertyChangedEventArgs e ) { base.OnIsMouseCaptureWithinChanged ( e ) ; }

        protected override void OnIsStylusDirectlyOverChanged ( DependencyPropertyChangedEventArgs e ) { base.OnIsStylusDirectlyOverChanged ( e ) ; }

        protected override void OnIsStylusCapturedChanged ( DependencyPropertyChangedEventArgs e ) { base.OnIsStylusCapturedChanged ( e ) ; }

        protected override void OnIsStylusCaptureWithinChanged ( DependencyPropertyChangedEventArgs e ) { base.OnIsStylusCaptureWithinChanged ( e ) ; }

        protected override void OnIsKeyboardFocusedChanged ( DependencyPropertyChangedEventArgs e ) { base.OnIsKeyboardFocusedChanged ( e ) ; }

        protected override void OnChildDesiredSizeChanged ( UIElement child ) { base.OnChildDesiredSizeChanged ( child ) ; }

        protected override void OnRender ( DrawingContext drawingContext ) { base.OnRender ( drawingContext ) ; }

        protected override void OnAccessKey ( AccessKeyEventArgs e ) { base.OnAccessKey ( e ) ; }

        protected override HitTestResult HitTestCore ( PointHitTestParameters hitTestParameters ) { return base.HitTestCore ( hitTestParameters ) ; }

        protected override GeometryHitTestResult HitTestCore ( GeometryHitTestParameters hitTestParameters ) { return base.HitTestCore ( hitTestParameters ) ; }

        protected override void OnLostFocus ( RoutedEventArgs e ) { base.OnLostFocus ( e ) ; }

        protected override void OnManipulationStarting ( ManipulationStartingEventArgs e ) { base.OnManipulationStarting ( e ) ; }

        protected override void OnManipulationStarted ( ManipulationStartedEventArgs e ) { base.OnManipulationStarted ( e ) ; }

        protected override void OnManipulationDelta ( ManipulationDeltaEventArgs e ) { base.OnManipulationDelta ( e ) ; }

        protected override void OnManipulationInertiaStarting ( ManipulationInertiaStartingEventArgs e ) { base.OnManipulationInertiaStarting ( e ) ; }

        protected override void OnManipulationBoundaryFeedback ( ManipulationBoundaryFeedbackEventArgs e ) { base.OnManipulationBoundaryFeedback ( e ) ; }

        protected override void OnManipulationCompleted ( ManipulationCompletedEventArgs e ) { base.OnManipulationCompleted ( e ) ; }


        #endregion
        #region Overrides of Visual
        protected override void OnVisualChildrenChanged ( DependencyObject visualAdded , DependencyObject visualRemoved ) { base.OnVisualChildrenChanged ( visualAdded , visualRemoved ) ; }

        protected override void OnDpiChanged ( DpiScale oldDpi , DpiScale newDpi ) { base.OnDpiChanged ( oldDpi , newDpi ) ; }
        #endregion
        #region Overrides of DependencyObject
        protected override bool ShouldSerializeProperty ( DependencyProperty dp ) { return base.ShouldSerializeProperty ( dp ) ; }
        #endregion
        protected override void OnItemsSourceChanged ( IEnumerable oldValue , IEnumerable newValue ) { base.OnItemsSourceChanged ( oldValue , newValue ) ; }

        protected override void OnDisplayMemberPathChanged ( string oldDisplayMemberPath , string newDisplayMemberPath ) { base.OnDisplayMemberPathChanged ( oldDisplayMemberPath , newDisplayMemberPath ) ; }

        protected override void OnItemTemplateChanged ( DataTemplate oldItemTemplate , DataTemplate newItemTemplate ) { base.OnItemTemplateChanged ( oldItemTemplate , newItemTemplate ) ; }

        protected override void OnItemTemplateSelectorChanged (
            DataTemplateSelector oldItemTemplateSelector
      , DataTemplateSelector newItemTemplateSelector
        )
        {
            base.OnItemTemplateSelectorChanged ( oldItemTemplateSelector , newItemTemplateSelector ) ;
        }

        protected override void OnItemStringFormatChanged ( string oldItemStringFormat , string newItemStringFormat ) { base.OnItemStringFormatChanged ( oldItemStringFormat , newItemStringFormat ) ; }

        protected override void OnItemBindingGroupChanged (
            BindingGroup oldItemBindingGroup
      , BindingGroup newItemBindingGroup
        )
        {
            base.OnItemBindingGroupChanged ( oldItemBindingGroup , newItemBindingGroup ) ;
        }

        protected override void OnItemContainerStyleChanged ( Style oldItemContainerStyle , Style newItemContainerStyle ) { base.OnItemContainerStyleChanged ( oldItemContainerStyle , newItemContainerStyle ) ; }

        protected override void OnItemContainerStyleSelectorChanged (
            StyleSelector oldItemContainerStyleSelector
      , StyleSelector newItemContainerStyleSelector
        )
        {
            base.OnItemContainerStyleSelectorChanged ( oldItemContainerStyleSelector , newItemContainerStyleSelector ) ;
        }

        protected override void OnItemsPanelChanged ( ItemsPanelTemplate oldItemsPanel , ItemsPanelTemplate newItemsPanel ) { base.OnItemsPanelChanged ( oldItemsPanel , newItemsPanel ) ; }

        protected override void OnGroupStyleSelectorChanged (
            GroupStyleSelector oldGroupStyleSelector
      , GroupStyleSelector newGroupStyleSelector
        )
        {
            base.OnGroupStyleSelectorChanged ( oldGroupStyleSelector , newGroupStyleSelector ) ;
        }

        protected override void OnAlternationCountChanged ( int oldAlternationCount , int newAlternationCount ) { base.OnAlternationCountChanged ( oldAlternationCount , newAlternationCount ) ; }

        protected override void AddChild ( object value ) { base.AddChild ( value ) ; }

        protected override void AddText ( string text ) { base.AddText ( text ) ; }

        public override void BeginInit ( ) { base.BeginInit ( ) ; }

        public override void EndInit ( ) { base.EndInit ( ) ; }

        protected override void ClearContainerForItemOverride ( DependencyObject element , object item ) { base.ClearContainerForItemOverride ( element , item ) ; }

        protected override void OnTextInput ( TextCompositionEventArgs e ) { base.OnTextInput ( e ) ; }

        protected override bool ShouldApplyItemContainerStyle ( DependencyObject container , object item ) { return base.ShouldApplyItemContainerStyle ( container , item ) ; }

        #region Overrides of TreeViewItem
        protected override DependencyObject GetContainerForItemOverride ( )
        {
            return ( DependencyObject ) new TreeViewItem1 ( ) ;
        }
        #endregion
        #endregion
    }

    public class TreeView1 : TreeView
    {
    #region Overrides of TreeView
    #region Overrides of ItemsControl
        protected override DependencyObject GetContainerForItemOverride ( )
        {
            return ( DependencyObject ) new TreeViewItem1 ( ) ;
        }

        protected override void AddChild ( object value )
        {
            DebugUtils.WriteLine ( nameof ( AddChild ) ) ;
            base.AddChild ( value ) ;
        }

        protected override void PrepareContainerForItemOverride (
            DependencyObject element
      , object           item
        )
        {
            DebugUtils.WriteLine (
                             $"{GetType ( ).FullName}.{nameof ( PrepareContainerForItemOverride )} {element.GetType ( ).FullName} {item.GetType ( ).FullName} {item.ToString ( )}"
                            ) ;
            (element as TreeViewItem1).Tag = item;
            base.PrepareContainerForItemOverride ( element , item ) ;
        }
    #endregion

        protected override bool ExpandSubtree ( TreeViewItem container )
        {
            DebugUtils.WriteLine ( $"{GetType ( ).FullName}:{nameof ( ExpandSubtree )}" ) ;
            return base.ExpandSubtree ( container ) ;
        }
    #endregion
    }
#else
    /// <summary>
    /// </summary>
    public sealed class TreeView1 : TreeView
    {
    }
#endif

    /// <summary>Control for displaying all known resources to the application.</summary>
    /// <remarks>
    ///     This control is a UserControl which contains a merged dictionary with
    ///     the following resources:
    ///     &lt;ResourceDictionary.MergedDictionaries&gt;
    ///     &lt;ResourceDictionary
    ///     Source = &quot; SharedResources.xaml&quot; /&gt;
    ///     &lt;ResourceDictionary
    ///     Source = &quot; ResourcesTemplates.xaml&quot; /&gt;
    ///     &lt;ResourceDictionary Source = &quot; ResourcesMisc.xaml&quot; /&gt;
    /// </remarks>
    /// <seealso cref="System.Windows.Controls.UserControl" />
    /// <seealso cref="KayMcCormick.Dev.IViewWithTitle" />
    /// <seealso
    ///     cref="AllResourcesTreeViewModel" />
    /// <seealso cref="KayMcCormick.Lib.Wpf.IControlView" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    [ TitleMetadata ( "Resources Explorer" ) ]
    public sealed partial class AllResourcesTree : UserControl
      , IViewWithTitle
      , IView < AllResourcesTreeViewModel >
      , IControlView
    {
        private static readonly Logger Logger =
            LogManager.GetCurrentClassLogger ( ) ;

        private AllResourcesTreeViewModel _viewModel ;
        private readonly JsonSerializerOptions _options ;
        private string                    _viewTitle = "Resources Explorer" ;

        /// <summary>
        /// 
        /// </summary>
        public AllResourcesTree ( ) : this(null,null) {
        }

        /// <summary>Parameterless constructor.</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for #ctor
        //public AllResourcesTree() { InitializeComponent(); }
        public AllResourcesTree ( AllResourcesTreeViewModel viewModel, JsonSerializerOptions options )
        {
            _viewModel = viewModel ;
            _options = options ;
            _options.WriteIndented = true;
            InitializeComponent ( ) ;
        }

        #region Implementation of IView<out AllResourcesTreeViewModel>
        /// <summary>
        /// </summary>
        public AllResourcesTreeViewModel ViewModel
        {
            get { return _viewModel ; }
            set { _viewModel = value ; }
        }
        #endregion

        #region Implementation of IView1
        /// <summary>
        /// </summary>
        public string ViewTitle { get { return _viewTitle ; } set { _viewTitle = value ; } }
        #endregion
        #region Overrides of DependencyObject
        /// <summary>
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        protected override bool ShouldSerializeProperty ( DependencyProperty dp )
        {
            return base.ShouldSerializeProperty ( dp ) ;
        }
        #endregion
        #region Overrides of ContentControl
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override bool ShouldSerializeContent ( )
        {
            Logger.Warn ( nameof ( ShouldSerializeContent ) ) ;
            return base.ShouldSerializeContent ( ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        protected override void AddChild ( [ NotNull ] object value )
        {
            Logger.Warn (
                         "{method} {value}"
                       , nameof ( ShouldSerializeContent )
                       , value.GetType ( )
                        ) ;
            base.AddChild ( value ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="text"></param>
        protected override void AddText ( string text )
        {
            Logger.Warn ( nameof ( AddText ) ) ;
            base.AddText ( text ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="oldContent"></param>
        /// <param name="newContent"></param>
        protected override void OnContentChanged ( object oldContent , object newContent )
        {
            Logger.Warn ( nameof ( OnContentChanged ) ) ;
            base.OnContentChanged ( oldContent , newContent ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="oldContentTemplate"></param>
        /// <param name="newContentTemplate"></param>
        protected override void OnContentTemplateChanged (
            DataTemplate oldContentTemplate
          , DataTemplate newContentTemplate
        )
        {
            Logger.Warn ( nameof ( OnContentTemplateChanged ) ) ;

            base.OnContentTemplateChanged ( oldContentTemplate , newContentTemplate ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="oldContentTemplateSelector"></param>
        /// <param name="newContentTemplateSelector"></param>
        protected override void OnContentTemplateSelectorChanged (
            DataTemplateSelector oldContentTemplateSelector
          , DataTemplateSelector newContentTemplateSelector
        )
        {
            Logger.Warn ( nameof ( OnContentTemplateSelectorChanged ) ) ;

            base.OnContentTemplateSelectorChanged (
                                                   oldContentTemplateSelector
                                                 , newContentTemplateSelector
                                                  ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="oldContentStringFormat"></param>
        /// <param name="newContentStringFormat"></param>
        protected override void OnContentStringFormatChanged (
            string oldContentStringFormat
          , string newContentStringFormat
        )
        {
            Logger.Warn ( nameof ( OnContentStringFormatChanged ) ) ;

            base.OnContentStringFormatChanged ( oldContentStringFormat , newContentStringFormat ) ;
        }
        #endregion
        #region Overrides of Control
        /// <summary>
        /// </summary>
        /// <param name="oldTemplate"></param>
        /// <param name="newTemplate"></param>
        protected override void OnTemplateChanged (
            ControlTemplate oldTemplate
          , ControlTemplate newTemplate
        )
        {
            Logger.Warn ( nameof ( OnTemplateChanged ) ) ;

            base.OnTemplateChanged ( oldTemplate , newTemplate ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        protected override Size MeasureOverride ( Size constraint )
        {
            return base.MeasureOverride ( constraint ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="arrangeBounds"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride ( Size arrangeBounds )
        {
            return base.ArrangeOverride ( arrangeBounds ) ;
        }
        #endregion
        #region Overrides of FrameworkElement
        /// <summary>
        /// </summary>
        /// <param name="oldStyle"></param>
        /// <param name="newStyle"></param>
        protected override void OnStyleChanged ( Style oldStyle , Style newStyle )
        {
            base.OnStyleChanged ( oldStyle , newStyle ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="child"></param>
        protected override void ParentLayoutInvalidated ( UIElement child )
        {
            base.ParentLayoutInvalidated ( child ) ;
        }

        /// <summary>
        /// </summary>
        public override void OnApplyTemplate ( ) { base.OnApplyTemplate ( ) ; }

        /// <summary>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override Visual GetVisualChild ( int index )
        {
            return base.GetVisualChild ( index ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged ( DependencyPropertyChangedEventArgs e )
        {
            base.OnPropertyChanged ( e ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="oldParent"></param>
        protected override void OnVisualParentChanged ( DependencyObject oldParent )
        {
            base.OnVisualParentChanged ( oldParent ) ;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        protected override DependencyObject GetUIParentCore ( )
        {
            return base.GetUIParentCore ( ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="sizeInfo"></param>
        protected override void OnRenderSizeChanged ( SizeChangedInfo sizeInfo )
        {
            base.OnRenderSizeChanged ( sizeInfo ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="layoutSlotSize"></param>
        /// <returns></returns>
        protected override Geometry GetLayoutClip ( Size layoutSlotSize )
        {
            return base.GetLayoutClip ( layoutSlotSize ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus ( RoutedEventArgs e ) { base.OnGotFocus ( e ) ; }

        /// <summary>
        /// </summary>
        public override void BeginInit ( ) { base.BeginInit ( ) ; }

        /// <summary>
        /// </summary>
        public override void EndInit ( ) { base.EndInit ( ) ; }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInitialized ( EventArgs e ) { base.OnInitialized ( e ) ; }
        #endregion
        #region Overrides of UIElement
        /// <summary>
        /// </summary>
        /// <param name="drawingContext"></param>
        protected override void OnRender ( DrawingContext drawingContext )
        {
            base.OnRender ( drawingContext ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="hitTestParameters"></param>
        /// <returns></returns>
        protected override HitTestResult HitTestCore ( PointHitTestParameters hitTestParameters )
        {
            return base.HitTestCore ( hitTestParameters ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="hitTestParameters"></param>
        /// <returns></returns>
        protected override GeometryHitTestResult HitTestCore (
            GeometryHitTestParameters hitTestParameters
        )
        {
            return base.HitTestCore ( hitTestParameters ) ;
        }
        #endregion
        #region Overrides of Visual
        /// <summary>
        /// </summary>
        /// <param name="visualAdded"></param>
        /// <param name="visualRemoved"></param>
        protected override void OnVisualChildrenChanged (
            DependencyObject visualAdded
          , DependencyObject visualRemoved
        )
        {
            base.OnVisualChildrenChanged ( visualAdded , visualRemoved ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="oldDpi"></param>
        /// <param name="newDpi"></param>
        protected override void OnDpiChanged ( DpiScale oldDpi , DpiScale newDpi )
        {
            base.OnDpiChanged ( oldDpi , newDpi ) ;
        }
        #endregion

        private void ButtonBase_OnClick ( object sender , RoutedEventArgs e )
        {
            var o = (ResourceNodeInfo)Tv.SelectedItem ;

            try
            {
                TextBlock.Text = JsonSerializer.Serialize ( o.Data , _options ) ;
            }
            catch ( Exception ex )
            {
                TextBlock.Text = ex.ToString ( ) ;
            }
        }
    }
}