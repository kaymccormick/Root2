﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using AnalysisControls;
using AnalysisControls.ViewModel;
using Autofac;
using Autofac.Core;
using Autofac.Features.Metadata;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Lib.Wpf;
using NLog;
using NLog.Fluent;
using RibbonLib.Model;

namespace Client2
{
    /// <summary>
    /// Interaction logic for Client2Window1.xaml
    /// </summary>
    [ShortKeyMetadata("Client2Window1")]
    public sealed partial class Client2Window1 : RibbonWindow, IView<ClientModel>, INotifyPropertyChanged
    {
        private ClientModel _viewModel;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static Client2Window1()
        {
            Window.WindowStateProperty.OverrideMetadata(typeof(Client2Window1), new FrameworkPropertyMetadata((object) WindowState.Normal, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(_OnWindowStateChanged), new CoerceValueCallback(CoerceWindowState)));
	                AttachedProperties.LifetimeScopeProperty.OverrideMetadata(typeof(Client2Window1), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, null, CoerceLifetimeScope));
        }


        private static object CoerceLifetimeScope(DependencyObject d, object basevalue)
        {
            var vs = DependencyPropertyHelper.GetValueSource(d, AttachedProperties.LifetimeScopeProperty);
            var lifetimeScope = (ILifetimeScope)basevalue;
            if (lifetimeScope.Tag.ToString().StartsWith("Client2Window1"))
            {
                return lifetimeScope;
            }
            var win = (Client2Window1) d;
            var coerceLifetimeScope = lifetimeScope.BeginLifetimeScope("Client2Window1");
            return coerceLifetimeScope;
        }

        private static void _OnWindowStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static object CoerceWindowState(DependencyObject d, object basevalue)
        {
            return basevalue;
        }

        
        public Client2Window1()
        {
            InitializeComponent();
            ViewModel = new ClientModel(new PrimaryRibbonModel());
        }

        public Client2Window1(ILifetimeScope scope, ClientModel viewModel, Main1Model main1Model, Main1Mode2 main1Mode2, MyCacheTarget2 myCacheTarget)
        {
            AddHandler(Binding.SourceUpdatedEvent, new EventHandler<DataTransferEventArgs>(OnSourceUpdated));
            AddHandler(Binding.TargetUpdatedEvent, new EventHandler<DataTransferEventArgs>(OnTargetUpdated));
            ViewModel = viewModel;
            DataContext = ViewModel.PrimaryRibbon;
            SetValue(AttachedProperties.LifetimeScopeProperty, scope);
            InitializeComponent();
            Main1.ViewModel = main1Model;
            Main1.ViewModel2 = main1Mode2;
            _viewModel.Main1Model = Main1?.ViewModel;
            if (Main1 != null) Main1.ViewModel.ClientViewModel = _viewModel;
            // viewModel.Ribbon = myRibbon;

            myCacheTarget?.Cache.SubscribeOn(Scheduler.Default)
                .Buffer(TimeSpan.FromMilliseconds(100))
                .Where(x => x.Any())
                .ObserveOnDispatcher(DispatcherPriority.Background)
                .Subscribe(
                    infos =>
                    {
                        foreach (var logEvent in infos)
                        {
                            var i =
                                JsonSerializer.Deserialize<LogEventInstance>(
                                    logEvent
                                    , new
                                        JsonSerializerOptions()
                                );
                            ViewModel.LogEntries.Add(i);
                        }
                    }
                );
            //Task.Run(Action);
            var logControl = new LogEventInstancesControl();
            logControl.SetBinding(LogEventInstancesControl.EventsSourceProperty,
                new Binding("ViewModel.LogEntries") {Source = this});
            AddHandler(RibbonQuickAccessToolBar.CloneEvent, new RibbonQuickAccessToolBarCloneEventHandler(Clone));
            // if (QAT.Items is IEditableCollectionView ai)
            // {
                
            // }
        }

        private void Clone(object sender, RibbonQuickAccessToolBarCloneEventArgs e)
        {
            e.Handled = true;
            
        }

        protected override void OnPreviewDrop(DragEventArgs e)
        {
            foreach (var format in e.Data.GetFormats())
            {
                DebugUtils.WriteLine(format);
                // var o = e.Data.GetData(format);
                // DebugUtils.WriteLine(o?.GetType()?.FullName ?? "");
            }

            base.OnPreviewDrop(e);
        }

        private void OnTargetUpdated(object sender, DataTransferEventArgs e)
        {
            var t = e.TargetObject;
            var customDesc = AttachedProperties.GetCustomDescription(t);
            BindingExpression expr = null;
            Binding binding = null;
            try
            {
                binding = BindingOperations.GetBinding(t, e.Property);
                expr = BindingOperations.GetBindingExpression(t, e.Property);
            }
            catch (Exception ex)
            { }

            var desc = $"{customDesc ?? t}({ConversionUtils.TypeToText(t.GetType())}";

            var propVal = t.GetValue(e.Property);
            DebugUtils.WriteLine($"{nameof(OnTargetUpdated)}{e.Property.Name};{e.Property.OwnerType.FullName};[ {desc} ] = {propVal}",
                DebugCategory.DataBinding);
        }

        private void OnSourceUpdated(object sender, DataTransferEventArgs e)
        {
            var t = e.TargetObject;
            var customDesc = AttachedProperties.GetCustomDescription(t);
            try
            {
                var expr = BindingOperations.GetBindingExpression(t, e.Property);
            } catch(Exception ex)
            {}

            var propVal = t.GetValue(e.Property);
            DebugUtils.WriteLine($"{e.Property.Name};{e.Property.OwnerType.FullName};[{customDesc ?? t}] = {propVal}" ,
                DebugCategory.DataBinding);
        }

        public ClientModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                OnPropertyChanged();

                if (_viewModel != null)
                {
                    _viewModel.Main1Model = Main1?.ViewModel;
                    if (Main1 != null) Main1.ViewModel.ClientViewModel = _viewModel;


                    // foreach (var o in myRibbon.ItemsSource)
                    // {
                        // Logger.Info($"RibbonItem: {o}");
                        // if (o is RibbonModelTab tab)
                        // {
                            // var i = 0;
                            // foreach (var ribbonModelItem in tab.Items) Logger.Info($"[{i}] {ribbonModelItem}");
                        // }
                    // }


                    // if (_AppMenu.ItemsSource != null)
                    // {
                        // foreach (var appMenuItem in _AppMenu.ItemsSource)
                            // Logger.Info(appMenuItem.ToString());
                    // }
                    // else
                    // {
                        // foreach (var ribbonModelAppMenuElement in _viewModel.PrimaryRibbon.AppMenu.Items)
                            // Logger.Info($"{ribbonModelAppMenuElement}");

                        // Logger.Info("No app menu Items source");
                    // }

                    //_viewModel.Documents.Add(new DocModel { Title = "Log", Content = logControl });
                }
            }
        }

        private static void DumpRibbon(Ribbon ribbon)
        {
            foreach (var ribbonItem in ribbon.Items)
            {
                if (ribbonItem is RibbonTab tab)
                {
                    foreach (var tabItem in tab.Items)
                    {
                        if (tabItem is RibbonGroup grp)
                        {
                            foreach (var grpItem in grp.Items)
                            {
                                if (grpItem is RibbonComboBox c)
                                {
                                    foreach (var cItem in c.Items)
                                    {
                                        if (cItem is RibbonGallery g)
                                        {
                                            DumpRibbonGallery(g);
                                        }
                                        else
                                        {
                                            throw new AppInvalidOperationException();
                                        }
                                    }
                                }
                                else
                                {
                                    throw new AppInvalidOperationException();
                                }
                            }
                        }
                        else
                        {
                            throw new AppInvalidOperationException();
                        }
                    }
                }
                else if (ribbonItem is RibbonModelTab tab2)
                {
                    foreach (var ribbonModelItem in tab2.Items)
                    {
                        if (ribbonModelItem is RibbonModelGroup g)
                        {
                            foreach (var modelItem in g.Items)
                            {
                                if (modelItem is RibbonModelItemComboBox box)
                                {
                                    foreach (var boxItem in box.ItemsCollection)
                                    {
                                        if (boxItem is RibbonModelGallery gal)
                                        {
                                            foreach (var galItem in gal.Items)
                                            {
                                                if (galItem is RibbonModelGalleryCategory cat)
                                                {
                                                    foreach (var ribbonModelGalleryItem in cat.Items)
                                                    {
                                                        if (ribbonModelGalleryItem is RibbonModelGalleryItem item1)
                                                        {
                                                            DebugUtils.WriteLine(item1.ToString());
                                                        }
                                                        else
                                                        {
                                                            throw new AppInvalidOperationException();
                                                        }

                                                    }
                                                }
                                                else if (galItem is RibbonGalleryCategory cat0)
                                                {
                                                    DumpRibbonGalleryCategory(cat0);
                                                }
                                                else
                                                {
                                                    throw new AppInvalidOperationException();
                                                }

                                            }
                                        }
                                        else if (boxItem is RibbonGallery gal0)
                                        {
                                            DumpRibbonGallery(gal0);
                                        }
                                        else
                                        {
                                            throw new AppInvalidOperationException();
                                        }

                                    }
                                }
                                else
                                {
                                    //throw new AppInvalidOperationException();
                                }

                            }
                        }
                        else
                        {
                            throw new AppInvalidOperationException();
                        }

                    }
                }
                else
                {
                    throw new AppInvalidOperationException();
                }
            }

            DumpLogicalChildren(ribbon);
            DumpVisualChildren(ribbon);
        }

        private static void DumpRibbonGallery(RibbonGallery g)
        {
            foreach (var gItem in g.Items)
            {
                if (gItem is RibbonGalleryCategory cc)
                {
                    DumpRibbonGalleryCategory(cc);
                }
                else
                {
                    throw new AppInvalidOperationException();
                }
            }
        }

        private static void DumpRibbonGalleryCategory(RibbonGalleryCategory cc)
        {
            foreach (var ccItem in cc.Items)
            {
                if (ccItem is RibbonGalleryItem)
                {
                    DebugUtils.WriteLine(ccItem.ToString());
                }
                else
                {
                    throw new AppInvalidOperationException();
                }
            }
        }

        private static void DumpLogicalChildren(DependencyObject node, int depth = 0)
        {
            DebugUtils.WriteLine(String.Join("", Enumerable.Repeat("  ", depth)) + node);
            foreach (var child in LogicalTreeHelper.GetChildren(node))
            {
                DumpLogicalChildren((DependencyObject) child, depth + 1);
            }
        }
        private static void DumpVisualChildren(DependencyObject node, int depth = 0)
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(node);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(node, i);
                DebugUtils.WriteLine(String.Join("", Enumerable.Repeat("  ", depth)) + child);
                DumpVisualChildren(child, depth  + 1);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private LogBuilder CreateLogBuilder()
        {
            return new LogBuilder(Logger).Level(LogLevel);
        }

        public LogLevel LogLevel { get; set; } = LogLevel.Warn;

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            CreateLogBuilder().Message($"{e.RoutedEvent.Name} {e.Source} {e.OriginalSource} {e.GetPosition(this)}").Write();
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            // CreateLogBuilder().Message("MouseMove: " + e.OriginalSource).Write();
            ViewModel.HoverElement = e.OriginalSource;
            base.OnPreviewMouseMove(e);
        }

        private void Client2Window1_OnLoaded(object sender, RoutedEventArgs e)
        {
            DebugUtils.WriteLine("Window OnLoaded");
        }

        private void CanExecutePaste(object sender, CanExecuteRoutedEventArgs e)
        {
            var dataObject = Clipboard.GetDataObject();
            if (dataObject != null) e.CanExecute = dataObject.GetFormats().Any();
        }

        private void OnExecutedPaste(object sender, ExecutedRoutedEventArgs e)
        {
            // Main1.ViewModel.OnExecutedPaste(sender, e);
        }

        private void CustomizeQATExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            DebugUtils.WriteLine("eep");
        }
    }
}