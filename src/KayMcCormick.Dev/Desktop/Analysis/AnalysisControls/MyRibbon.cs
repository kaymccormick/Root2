using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Input;
using AnalysisControls.RibbonModel;
using Autofac;
using Castle.DynamicProxy;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf;
using NLog;
using NLog.Fluent;

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
            base.OnApplyTemplate();
            MyContextualTabGroupItemsControl = GetTemplateChild("PART_ContextualTabGroupItemsControl") as MyRibbonContextualTabGroupItemsControl;
            if (MyContextualTabGroupItemsControl != null) MyContextualTabGroupItemsControl.Logger = Logger;
            _contextualTabGroupItemsControl =
                GetTemplateChild("PART_ContextualTabGroupItemsControl") as RibbonContextualTabGroupItemsControl;
        }

        static MyRibbon()
       {
           DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRibbon),
               new FrameworkPropertyMetadata(typeof(MyRibbon)));
            ItemsPanelProperty.OverrideMetadata(typeof(MyRibbon), new FrameworkPropertyMetadata(new ItemsPanelTemplate(new FrameworkElementFactory(typeof(MyRibbonTabsPanel)))));
		    
            AttachedProperties.LifetimeScopeProperty.OverrideMetadata(typeof(MyRibbon), new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.Inherits, null, CoerceLifetimeScope));
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
                var tabPRoxy = proxyGen.CreateClassProxyWithTarget(containerForItemOverride,
                    new ProxyGenerationOptions(),
                    interceptor);

                return tabPRoxy;
            }

            return containerForItemOverride;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool EnableProxy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MyRibbonContextualTabGroupItemsControl MyContextualTabGroupItemsControl { get; set; }

        private void UseLogMethod(string message)
        {
            Logger.Info(message);
        }

      
        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            if (!e.Handled)
            {
                if (e.OriginalSource is FrameworkElement el)
                {
                    if (el.TemplatedParent != null)
                    {
                        if (el.TemplatedParent is ContentPresenter cp)
                        {
                            if (cp.Content is RibbonModelDropZone dz)
                            {
                                e.Effects= dz.OnDrop(e.Data);
                                e.Handled = true;
                            }
                        }
                    }
                }
            }
        }
    }

    /// <inheritdoc />
    public class MyRibbonQuickAccessToolbar : RibbonQuickAccessToolBar
    {
        static MyRibbonQuickAccessToolbar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRibbonQuickAccessToolbar),
                new FrameworkPropertyMetadata(typeof(MyRibbonQuickAccessToolbar)));


        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return (DependencyObject)new MyRibbonControl();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return base.IsItemItsOwnContainerOverride(item);
        }
    }
    public class MyRibbonTabsPanel : RibbonTabsPanel {
    
    }

    public class MyRibbonMenuItem : RibbonMenuItem
    {
        private object _currentItem;
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
    }

    public class MyRibbonApplicationMenu : RibbonApplicationMenu
    {
        private object _currentItem;

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
    }

    /// <summary>
    /// 
    /// </summary>
    public class MyRibbonApplicationMenuItem : RibbonApplicationMenuItem
    {
        private object _currentItem;

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
                            throw new AppInvalidOperationException("invalid container");
                    }
                }
            }
            return (DependencyObject)new MyRibbonApplicationMenuItem();

        }
    }
}