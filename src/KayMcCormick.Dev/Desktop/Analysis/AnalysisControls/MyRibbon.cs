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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            MyContextualTabGroupItemsControl = GetTemplateChild("PART_ContextualTabGroupItemsControl") as MyRibbonContextualTabGroupItemsControl;
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

    public class MyRibbonTabsPanel : RibbonTabsPanel {
    
    }
    
}