using System.Windows.Controls;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for PropertiesPanel.xaml
    /// </summary>
    public partial class PropertiesPanel : UserControl
    {
        #if UNATTACHED_PROP
        public static readonly RoutedEvent PropertiesCollectionChangedEvent =
            EventManager.RegisterRoutedEvent(
                                              nameof(PropertiesCollectionChangedEvent)
                                            , RoutingStrategy.Direct
                                            , typeof(RoutedPropertyChangedEventHandler<
                                                  ObservableCollection<PropInfo>>)
                                            , typeof(PropertiesWindow));

        public static readonly DependencyProperty PropertiesCollectionProperty = DependencyProperty.Register("PropertiesCollection", typeof(ObservableCollection<PropInfo>), typeof(PropertiesWindow),
                                                                                                             new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(OnPropertiesCollectionChanged), new CoerceValueCallback(CoercePropertiesCollection), false, UpdateSourceTrigger.PropertyChanged));

        private static void OnPropertiesCollectionChanged(
            DependencyObject d
          , DependencyPropertyChangedEventArgs e
        )
        {
            PropertiesWindow window = (PropertiesWindow)d;
            RoutedPropertyChangedEventArgs<ObservableCollection<PropInfo>> newEvent =
                new RoutedPropertyChangedEventArgs<ObservableCollection<PropInfo>>(
                                                                                          (ObservableCollection<PropInfo>)e.OldValue
                                                                                        , (ObservableCollection<PropInfo>)e.NewValue
                                                                                         );

            window.RaiseEvent(newEvent);
        }

        private static object CoercePropertiesCollection(DependencyObject d, object basevalue) { return basevalue; }

        public ObservableCollection<PropInfo> PropertiesCollection
        {
            get => (ObservableCollection<PropInfo>)GetValue(PropertiesCollectionProperty);
            set => SetValue(PropertiesCollectionProperty, value);
        }
        #endif
        public PropertiesPanel()
        {
            InitializeComponent();
        }
    }
}
