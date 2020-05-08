using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using AnalysisAppLib;
using AnalysisControls;
using Autofac;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Lib.Wpf;
using Microsoft.Graph;
using NLog;

namespace Client2
{
    /// <summary>
    /// Interaction logic for Client2Window1.xaml
    /// </summary>
    [ShortKeyMetadata("Client2Window1")]
    public partial class Client2Window1 : RibbonWindow, IView<ClientModel>
    {
        public Client2Window1(ILifetimeScope scope, ClientModel viewModel, MyCacheTarget2 myCacheTarget)
        {
            ViewModel = viewModel;
            SetValue(AttachedProperties.LifetimeScopeProperty, scope);
            InitializeComponent();

            myCacheTarget?.Cache.SubscribeOn(Scheduler.Default)
                .Buffer(TimeSpan.FromMilliseconds(100))
                .Where(x => x.Any())
                .ObserveOnDispatcher(DispatcherPriority.Background)
                .Subscribe(
                    infos => {
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
            LogEventInstancesControl logControl = new LogEventInstancesControl();
            logControl.SetBinding(LogEventInstancesControl.EventsSourceProperty,
                new Binding("ViewModel.LogEntries") {Source = this});

            Main1.ViewModel?.Documents.Add(new DocModel { Title = "Log", Content = logControl} );
        }

        private void Action()
        {
            for (;;)
            {
                LogManager.GetCurrentClassLogger().Info("My event");
                Thread.Sleep(30000);
            }
        }

        public ClientModel ViewModel { get; }
    }

    public class ClientModel
    {
        public LogEventInstanceObservableCollection LogEntries { get; set; } = new LogEventInstanceObservableCollection();
        public RibbonModel Ribbon { get; set; } = new RibbonModel();
    }

    public class RibbonModel
    {
        public RibbonModelApplicationMenu AppMenu { get; set; } = new RibbonModelApplicationMenu();
        public ObservableCollection<RibbonModelAppMenuItem> AppMenuItems { get; } = new ObservableCollection<RibbonModelAppMenuItem>();

        public RibbonModel()
        {
            var menuItem = AppMenu.CreateAppMenuItem("Test");
            var split = menuItem.CreateSplitMenuItem("test 123");
            split.CreateAppMenuItem("Foo bar");
            
            var ribbonModelTab = new RibbonModelTab {Header = "Test"};
            var group = new RibbonModelTabItemGroup() {Header = "my group"};
            {
                var dictionary = new Dictionary<object, object>();
                dictionary["green"] = new[] {"foo", "Bar", "baz"};
                dictionary["test123"] = new[] {"foo", "Bar", "baz"};
                var combo = group.CreateRibbonComboBox(dictionary);
                var ribbonModelGallery = new RibbonModelGallery();
            }

            {
                var dictionary = new Dictionary<object, object>();

                dictionary["green"] = new[] {"foo", "Bar", "baz"};
                dictionary["test123"] = new[] {"foo", "Bar", "baz"};
                group.CreateRibbonSplitButton(dictionary);
            }

            // var addItemsTo = ribbonModelGalleryCatgory.Items;
            // var ribbonSplitButton = new RibbonModelItemSplitButton() {Label = "test"};
            // var modelGalleryCatgory = new RibbonModelGalleryCategory();
            // ribbonSplitButton.Items.Add(modelGalleryCatgory);
            // modelGalleryCatgory.Items.Add(new RibbonModelGalleryItem(){Content = "cheers"});
            // group.Items.Add(ribbonSplitButton);
            // var cmds = new[] {"Foo", "bar", "baaz"};
            // foreach (var cmd in cmds)
            // {
            // var item
            // = new RibbonModelGalleryItem();
            // item.Content = cmd;
            // addItemsTo.Add(item);
                
            // }

            // foreach (var solidColorBrush in new[] {Brushes.Pink, Brushes.Green, Brushes.Bisque})
            // {
                // var item = new RibbonModelGalleryItem()
                // {
                    // Content = new Border {Background = solidColorBrush, Child = new Rectangle {Width = 40, Height = 40}}

                // };
                // addItemsTo.Add(item);

            // }

            // var ribbonModelItemComboBox = new RibbonModelItemComboBox() { Label = "MCombo1" };
            // var ribbonModelGalleryCatgory = new RibbonModelGalleryCategory() { Content = "fo", Header = "test" };
            // ribbonModelGallery.Items.Add(ribbonModelGalleryCatgory);

//            ribbonModelItemComboBox.Items.Add(ribbonModelGallery);
            //group.Items.Add(ribbonModelItemComboBox);

            var group2 = ribbonModelTab.CreateGroup("Group 2");
            group2.Items.Add(new RibbonModelItemTextBox(){Label="EAt me"});
            var b = group2.CreateRibbonMenuButton("test");
            b.Items.Add(new RibbonModelItemMenuButton {Label = "derp"});
            //group2.CreateRibbonRadioButton("");
            ribbonModelTab.Items.Add(group);
        
            RibbonItems.Add(ribbonModelTab);
        }

        

        public ObservableCollection<RibbonModelTab> RibbonItems { get; } = new ObservableCollection<RibbonModelTab>();
    }

    public class RibbonModelApplicationMenu : RibbonModelAppMenuElement
    {
    }

    public class RibbonModelAppMenuItem  : RibbonModelAppMenuElement
    {
    }

    public class RibbonModelAppSplitMenuItem : RibbonModelAppMenuElement
    {
    }

    public class RibbonModelAppMenuElement
    {
        public string Header { get; set; }
        public string KeyTip { get; set; }
        public object ImageSource { get; set; }
        public RibbonModelAppSplitMenuItem CreateSplitMenuItem(string header)
        {
            var r = new RibbonModelAppSplitMenuItem { Header = header };
            Items.Add(r);
            return r;
        }
        public RibbonModelAppMenuItem CreateAppMenuItem(string Header)
        {
            var r = new RibbonModelAppMenuItem { Header = Header };
            Items.Add(r);
            return r;
        }

        public ObservableCollection<RibbonModelAppMenuElement> Items { get; } = new ObservableCollection<RibbonModelAppMenuElement>();

    }

    public class RibbonModelTab
    {
        public string Header { get; set; }
        public ObservableCollection<RibbonModelItem> Items { get; }= new ObservableCollection<RibbonModelItem>();

        public RibbonModelTabItemGroup CreateGroup(string @group)
        {
            var r = new RibbonModelTabItemGroup() {Header = @group};
            Items.Add(r);
            return r;
        }
    }


    public class RibbonModelTabItemGroup : RibbonModelItem
    {
        public string Header { get; set; }
        public ObservableCollection<RibbonModelItem> Items { get; }= new ObservableCollection<RibbonModelItem>();

        public RibbonModelItemMenuButton CreateRibbonMenuButton(string label)
        {
            var r = new RibbonModelItemMenuButton {Label = label};
            Items.Add(r);
            return r;
        }

        public RibbonModelItemComboBox CreateRibbonComboBox(Dictionary<object, object> dictionary)
        {
            var r= new RibbonModelItemComboBox();
            Items.Add(r);
            AddGalleryItems(dictionary, r.Items);
            var ribbonModelItemComboBox = CreateRibbonGallery(dictionary, "gal2");
            r.Items.Add(ribbonModelItemComboBox);
            return r;
        }

        public RibbonModelItemSplitButton CreateRibbonSplitButton(Dictionary<object, object> dictionary)
        {
            var r = new RibbonModelItemSplitButton();
            Items.Add(r);
            var ribbonModelItemComboBox = CreateRibbonGallery(dictionary, "gal1");
            r.Items.Add(ribbonModelItemComboBox);
            return r;
        }
        private static RibbonModelGallery CreateRibbonGallery(Dictionary<object, object> dictionary, string header)
        {
            var g = new RibbonModelGallery() {Header = header};

            AddGalleryItems(dictionary, g.Items);

            return g;
        }

        private static void AddGalleryItems(Dictionary<object, object> dictionary, ObservableCollection<RibbonModelItem> g)
        {
            foreach (var keyValuePair in dictionary)
            {
                var cat = new RibbonModelGalleryCategory() {Label = keyValuePair.Key};
                foreach (var vv in keyValuePair.Value as IEnumerable)

                {
                    var item = new RibbonModelGalleryItem() {Content = vv};
                    cat.Items.Add(item);
                }

                g.Add(cat);
            }
        }
    }

    public class RibbonModelItemMenuButton : RibbonModelItem
    {
        public ObservableCollection<RibbonModelItem> Items { get;  } = new ObservableCollection<RibbonModelItem>();
    }
    
    public class RibbonModelItem
    {

        public object Label { get; set; }
        public ICommand Command { get; set; }
        public object CommandTarget { get; set; }
        public object CommandParameter { get; set; }
    }

    public class RibbonModelItemComboBox : RibbonModelItem
    {
        public ObservableCollection<RibbonModelItem> Items { get; } = new ObservableCollection<RibbonModelItem>();
    }

    public class RibbonModelGallery : RibbonModelItem
    {
        public string Header { get; set; }
        public ObservableCollection<RibbonModelItem> Items { get; } = new ObservableCollection<RibbonModelItem>();
    }

    public class RibbonModelGalleryCategory :  RibbonModelItem
    {
        public RibbonModelGalleryCategory()
        {
        }

        public ObservableCollection<RibbonModelGalleryItem> Items { get; } = new ObservableCollection<RibbonModelGalleryItem>();

        public object Content { get; set; }

    }

    public class RibbonModelGalleryItem: RibbonModelItem
    {
        public object Content { get; set; }
    }

    public class RibbonModelItemButton : RibbonModelItem
    {
    }
    public class RibbonModelItemTextBox : RibbonModelItem
    {
    }
    public class RibbonModelItemSplitButton : RibbonModelItem
    {
        public ObservableCollection<RibbonModelItem> Items { get; } = new ObservableCollection<RibbonModelItem>();
    }

}
