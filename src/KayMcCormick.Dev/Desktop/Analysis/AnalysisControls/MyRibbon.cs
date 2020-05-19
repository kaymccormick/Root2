using Autofac;
using Castle.DynamicProxy;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf;
using NLog;
using NLog.Fluent;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using AnalysisControls.RibbonModel;

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
           DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRibbon),
               new FrameworkPropertyMetadata(typeof(MyRibbon)));
            ItemsPanelProperty.OverrideMetadata(typeof(MyRibbon), new FrameworkPropertyMetadata(new ItemsPanelTemplate(new FrameworkElementFactory(typeof(MyRibbonTabsPanel)))));
		    
            AttachedProperties.LifetimeScopeProperty.OverrideMetadata(typeof(MyRibbon), new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.Inherits, null, CoerceLifetimeScope));
            CommandManager.RegisterClassCommandBinding(typeof(MyRibbon), new CommandBinding((ICommand)RibbonCommands.AddToQuickAccessToolBarCommand, new ExecutedRoutedEventHandler(MyRibbon.AddToQATExecuted), new CanExecuteRoutedEventHandler(MyRibbon.AddToQATCanExecute)));
            Ribbon.QuickAccessToolBarProperty.OverrideMetadata(typeof(MyRibbon), new FrameworkPropertyMetadata(null, RibbonQATUpdated));

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
        }

        protected override Size MeasureOverride(Size constraint)
        {
            DebugUtils.WriteLine($"{this}.{nameof(MeasureOverride)}");
            return base.MeasureOverride(constraint);
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

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool EnableProxy { get; set; }

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

    /// <inheritdoc />
    public class MyRibbonQuickAccessToolBar : RibbonQuickAccessToolBar
    {
        public override void OnApplyTemplate()
        {
            DebugUtils.WriteLine($"{this}.{nameof(OnApplyTemplate)}");
            base.OnApplyTemplate();
        }

        static MyRibbonQuickAccessToolBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRibbonQuickAccessToolBar),
                new FrameworkPropertyMetadata(typeof(MyRibbonQuickAccessToolBar)));


        }

        
        protected override DependencyObject GetContainerForItemOverride()
        {
            return (DependencyObject)new MyRibbonControl();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return base.IsItemItsOwnContainerOverride(item);
        }
        internal bool ContainsId(object targetID)
        {
            foreach (object obj in (IEnumerable)this.Items)
            {
                if (obj is DependencyObject element && object.Equals(RibbonControlService.GetQuickAccessToolBarId(element), targetID))
                    return true;
            }
            return false;
        }
        protected override Size MeasureOverride(Size constraint)
        {
            DebugUtils.WriteLine($"{this}.{nameof(MeasureOverride)}");
            return base.MeasureOverride(constraint);
        }

    }
    public class MyRibbonTabsPanel : RibbonTabsPanel {
        public override void OnApplyTemplate()
        {
            DebugUtils.WriteLine($"{this}.{nameof(OnApplyTemplate)}");
            base.OnApplyTemplate();
        }

    }

    public class MyRibbonMenuItem : RibbonMenuItem
    {
        private object _currentItem;
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